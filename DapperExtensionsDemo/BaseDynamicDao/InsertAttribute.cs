using System;
using System.Collections.Generic;
using System.Text;

namespace DapperExtensionsDemo.BaseDynamicDao
{
    public class InsertAttribute : Attribute
    {
        public string Fields { get; set; }
        public InsertAttribute(string fields)
        {
            this.Fields = fields;
        }
    }
}
