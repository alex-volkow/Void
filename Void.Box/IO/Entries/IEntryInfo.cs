﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.IO
{
    public interface IEntryInfo
    {
        string Name { get; }
        string Path { get; }
        long Length { get; }
    }
}
