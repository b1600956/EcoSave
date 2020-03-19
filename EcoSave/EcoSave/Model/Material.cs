using System;
using System.Collections.Generic;
using System.Text;

namespace EcoSave.Model
{
    class Material
    {
        public string MaterialID { get; set; }
        public string MaterialName { get; set; }
        public string Description { get; set; }
        public int PointsPerKg { get; set; }
        public List<string> CollectorList { get; set; }
    }
}
