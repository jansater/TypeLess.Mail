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
        public Stream ContentStream { get; set; }
        public string Name { get; set; }
        public string MediaType { get; set; }

        public Attachment(Stream contentStream, string name, string mediaType)
        {
            this.ContentStream = contentStream;
            this.Name = name;
            this.MediaType = mediaType;
        }
    }
}
