using System;
using System.Collections.Generic;
using System.Text;

namespace EcoSave.Model
{
    class Collector : User
    {
        public string Address { get; set; }
        public List<string> MaterialCollection { get; set; }
    }
}
