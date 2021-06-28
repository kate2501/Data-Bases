using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Diagnostics;

namespace lr11
{
    class ElectionContext: DbContext
    {
        public ElectionContext() : base("connection")
        {
        }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateProfile> CandidateProfiles { get; set; }
        public DbSet<Promise> Promises { get; set; }
        public DbSet<Confident> Confidents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.Log = (query) => Debug.Write(query);
        }
    }
}
