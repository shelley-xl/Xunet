using System;

namespace Xunet.MiniExcel.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ExcelIgnoreAttribute : Attribute
    {
        public bool ExcelIgnore { get; set; }
        public ExcelIgnoreAttribute(bool excelIgnore = true) => ExcelIgnore = excelIgnore;
    }
}
