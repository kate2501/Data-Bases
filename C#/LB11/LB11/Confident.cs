using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB11
{
    public class Confident
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PoliticalPreferences { get; set; }
        public int Age { get; set; }
        public int? CandidateId { get; set; }
        public Candidate Candidate { get; set; }
    }
}
