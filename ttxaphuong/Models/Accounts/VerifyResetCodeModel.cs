﻿namespace ttxaphuong.Models.Accounts
{
    public class VerifyResetCodeModel
    {
        public string? Email { get; set; }
        public string? Code { get; set; }
        public string? NewPassword { get; set; }

    }
}
