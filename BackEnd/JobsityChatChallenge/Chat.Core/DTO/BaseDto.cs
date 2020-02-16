using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Core.DTO
{
    public class BaseDto
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
    }
}
