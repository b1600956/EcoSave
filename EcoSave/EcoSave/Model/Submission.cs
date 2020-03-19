using System;
using System.Collections.Generic;
using System.Text;

namespace EcoSave.Model
{
    class Submission
    {
        public string SubmissionID { get; set; }
        public DateTime ProposedDate { get; set; }
        public DateTime ActualDate { get; set; }
        public int WeightInKg { get; set; }
        public int PointsAwarded { get; set; }
        public string Status { get; set; }
        public string Collector { get; set; }
        public string Recycler { get; set; }
        public string Material { get; set; }
    }
}
