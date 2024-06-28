using System;

namespace Xunet.MiniExcel.OpenXml.Models
{
    internal class DrawingDto
    {
        internal string ID { get; set; } = $"R{Guid.NewGuid():N}";
    }
}