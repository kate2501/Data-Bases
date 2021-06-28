using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB11
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    class ElectionContext: DbContext
    {
        public ElectionContext() : base("DBConnection")
        {
        }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateProfile> CandidateProfiles { get; set; }
        public DbSet<Promise> Promises { get; set; }
        public DbSet<Confident> Confidents { get; set; }
    
    }

}
