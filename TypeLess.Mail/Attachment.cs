using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TypeLess.Mail
{
    public class Attachment
    {
        public byte[] Content { get; set; }
        public string Name { get; set; }
        public string MediaType { get; set; }
        
        public Attachment(byte[] content, string name, string mediaType)
        {
            this.Content = content;
            this.Name = name;
            this.MediaType = mediaType;
        }

        internal Stream OpenContentStream() {
            return new MemoryStream(Content);
        }
        
    }
}
