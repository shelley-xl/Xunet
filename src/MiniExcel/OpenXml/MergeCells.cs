﻿using System.Collections.Generic;

namespace Xunet.MiniExcel.OpenXml
{
    internal class MergeCells
    {
        public Dictionary<string, object> MergesValues { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, string> MergesMap { get; set; } = new Dictionary<string, string>();
    }
}
