﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Void.Net.Http
{
    public class JsonContent : StringContent
    {
        public JsonContent(string content)
            : this(content, Encoding.UTF8) {
        }

        public JsonContent(string content, Encoding encoding)
            : base(content, encoding, "application/json") {
        }
    }
}
