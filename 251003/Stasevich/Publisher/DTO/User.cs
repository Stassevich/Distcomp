﻿namespace WebApplication1.DTO
{
    public class UserRequestTo
    {
        public long? Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
    }

    public class UserResponseTo
    {
        public long id { get; set; }
        public string login { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
    }
}
