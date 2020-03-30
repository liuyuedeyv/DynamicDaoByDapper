using System;
using System.Collections.Generic;
using System.Text;

namespace DapperExtensionsDemo.BaseDynamicDao
{
    public class TableAttribute : Attribute
    {
        public string Table { get; set; }
        public TableAttribute(string table)
        {
            this.Table = table;
        }
    }
}
