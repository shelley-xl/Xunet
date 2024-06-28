using System.Threading;
using System.Threading.Tasks;

namespace Xunet.MiniExcel
{
    internal interface IExcelWriter
    {
        void SaveAs();
        Task SaveAsAsync(CancellationToken cancellationToken = default(CancellationToken));
        void Insert();
    }
}
