﻿using System;

namespace Xunet.MiniExcel.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ExcelFormatAttribute : Attribute
    {
        public string Format { get; set; }
        public ExcelFormatAttribute(string format) => Format = format;
    }
}
