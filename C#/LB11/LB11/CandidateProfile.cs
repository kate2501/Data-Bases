using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Spatial;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LB11
{
    public class CandidateProfile
    {
        [Key]
        [ForeignKey("Candidate")]
        public int Id { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
        public virtual Candidate Candidate { get; set; }
    }
}

