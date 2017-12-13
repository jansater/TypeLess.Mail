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
        public int FileReferenceId { get; set; }

        public Attachment(byte[] content, string name, string mediaType)
        {
            this.Content = content;
            this.Name = name;
            this.MediaType = mediaType;
        }

        public Attachment(int fileReferenceId)
        {
            this.FileReferenceId = fileReferenceId;
        }

        internal Stream OpenContentStream() {

            if (Content == null) {
                throw new ApplicationException("Content has not been set");
            }

            return new MemoryStream(Content);
        }
        
    }
}
