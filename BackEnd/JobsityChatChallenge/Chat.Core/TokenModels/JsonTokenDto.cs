using Chat.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Core.TokenModels
{
    public class JsonTokenDto : BaseDto
    {
        public JsonTokenDto(string _token, long _expirationDate)
        {
            Token = _token;
            ExpirationDate = _expirationDate;
        }

        public JsonTokenDto()
        {
        }

        public string Token { get; set; }
        public long ExpirationDate { get; set; }
    }
}
