﻿using System.Transactions;
using Npgsql;

namespace Infrastructure.Repositories.PostgersRepositories;

public class PgRepository
{
    private static bool _typesReloaded = false;
    private readonly InfrastructureSettings _settings;

    protected const int DefaultTimeoutInSeconds = 5;

    public PgRepository(InfrastructureSettings settings){
        _settings = settings;
    }
    
    protected async Task<NpgsqlConnection> GetConnection(){
        if (
            Transaction.Current is not null &&
            Transaction.Current.TransactionInformation.Status is TransactionStatus.Aborted
        ){
            throw new TransactionAbortedException("Transaction was aborted (probably by user cancellation request)");
        }

        var connection = new NpgsqlConnection(_settings.PostgresConnectionString);
        await connection.OpenAsync();

        // due to in-process migrations
        if (!_typesReloaded){
            await connection.ReloadTypesAsync();
            _typesReloaded = true;
        }

        return connection;
    }

    private TransactionScope CreateTransactionScope(
        IsolationLevel level = IsolationLevel.ReadCommitted
    )
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions{
                IsolationLevel = level,
                Timeout = TimeSpan.FromSeconds(5)
            },
            TransactionScopeAsyncFlowOption.Enabled
        );
    }
}