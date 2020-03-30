using System;
using System.Collections.Generic;
using System.Text;

namespace DapperExtensionsDemo.BaseDynamicDao
{
    public class UpdateAttribute : Attribute
    {
        public string Fields { get; set; }
        public UpdateAttribute(string fields)
        {
            this.Fields = fields;
        }
    }

  

   
}
