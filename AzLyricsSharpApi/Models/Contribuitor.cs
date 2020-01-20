using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzLyricsSharpApi
{
    public abstract class Contribuitor
    {
        public string Name { get; set; }
        public Contribuitor(string name)
        {
            Name = name;
        }
    }
}
