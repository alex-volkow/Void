using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.IO
{
    public interface IEntryReader : IEntryInfo
    {
        Stream Open();
    }
}
