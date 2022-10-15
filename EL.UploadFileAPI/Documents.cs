using System;
using System.Collections.Generic;
using System.Text;

namespace EL.UploadFileAPI
{
    public class Documents
    {
        public int documentID { get; set; }
        public string fileName { get; set; }
        public string contentType { get; set; }
        public long? fileSize { get; set; }
    }
}
