using System;
using System.Collections.Generic;
using System.Text;

namespace DapperExtensionsDemo.BaseDynamicDao
{
    public class SelectAttribute : Attribute
    {
        public SelectAttribute()
        {
        }
        public string Fields { get; set; }
        public SelectAttribute(string fields)
        {
            this.Fields = fields;
        }
    }
}
