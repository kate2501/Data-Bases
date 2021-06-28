using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lr11
{
    class Promise
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public ICollection<Candidate> Candidates { get; set; }
        public Promise()
        {
            Candidates = new List<Candidate>();
        }   
    }
}
