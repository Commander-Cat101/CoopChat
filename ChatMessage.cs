using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopChat
{
    public class ChatMessage
    { 
        public string MessageText { get; set; }
        public int SenderID { get; set; }
        public string SenderName { get; set; }

        public ChatMessage(string messageText, int senderID, string senderName)
        {
            MessageText = messageText;
            SenderID = senderID;
            SenderName = senderName;
        }
    }
}
