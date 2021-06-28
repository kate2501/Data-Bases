using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace lr11
{
    class CandidateProfile
    {
        [Key]
        [ForeignKey("Candidate")]
        public int Id { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
        public virtual Candidate Candidate { get; set; }
    }
}
