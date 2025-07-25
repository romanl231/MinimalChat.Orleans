using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ChatMessage
    {
        public string SenderId { get; set; } = string.Empty;
        public string RoomId { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}
