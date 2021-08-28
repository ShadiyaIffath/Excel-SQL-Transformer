using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelTransformer.Models
{
    class TransformationDetails
    {
        public string table { get; set; }

        public string filePath { get; set; }

        public bool insertQuery { get; set; }
    }
}
