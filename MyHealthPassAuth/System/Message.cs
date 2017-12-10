using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealthPassAuth.System
{
    public enum MessageResult
    {
        SUCCESS = 1, 
        ERROR = 2
    }

    /// <summary>
    /// Encapsulates messages among system 
    /// </summary>
    public struct Message
    {
        private MessageResult messageResult; 
        public MessageResult Result
        {
            get { return messageResult; }
            set { messageResult = value; }
        }

        private string text; 
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
    }
}
