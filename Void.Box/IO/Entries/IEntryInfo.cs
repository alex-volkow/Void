using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.IO
{
    public interface IEntryInfo
    {
        FilePath Path { get; }
        long Length { get; }
    }
}
