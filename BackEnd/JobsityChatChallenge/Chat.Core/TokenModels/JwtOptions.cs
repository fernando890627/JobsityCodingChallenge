using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Core.TokenModels
{
    public class JwtOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string ExpirationDays { get; set; }
        public string TokenSecretKey { get; set; }
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(1);
        public SigningCredentials SigningCredentials { get; set; }
        
    }
}
