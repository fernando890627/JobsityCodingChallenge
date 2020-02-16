using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Core.DTO
{
    public class NewUserResponseDto : BaseDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
    }
}
