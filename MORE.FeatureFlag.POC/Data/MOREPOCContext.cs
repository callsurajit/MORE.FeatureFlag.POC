using Microsoft.EntityFrameworkCore;
using MORE.FeatureFlag.POC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MORE.FeatureFlag.POC.Data
{
    public class MOREPOCContext : DbContext
    {
        public MOREPOCContext(DbContextOptions<MOREPOCContext> options) : base(options)
        {
        }

        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionDependency> QuestionDependencys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map entities to tables  
            modelBuilder.Entity<QuestionType>().ToTable("QuestionType");
            modelBuilder.Entity<Question>().ToTable("Question");
            modelBuilder.Entity<QuestionDependency>().ToTable("QuestionDependency");
        }
    }
}
