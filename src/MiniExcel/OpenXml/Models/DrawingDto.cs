using System;

namespace Xunet.MiniExcels.OpenXml.Models
{
    internal class DrawingDto
    {
        internal string ID { get; set; } = $"R{Guid.NewGuid():N}";
    }
}