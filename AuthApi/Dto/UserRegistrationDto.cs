﻿namespace AuthApi.Dto
{
    public class UserRegistrationDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}
