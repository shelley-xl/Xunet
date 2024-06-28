using Xunet.MiniExcel.Attributes;
using System.Globalization;

namespace Xunet.MiniExcel
{
    public interface IConfiguration { }
    public abstract class Configuration : IConfiguration
    {
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
        public DynamicExcelColumn[] DynamicColumns { get; set; }
        public int BufferSize { get; set; } = 1024 * 512;
        public bool FastMode { get; set; } = false;
    }
}
