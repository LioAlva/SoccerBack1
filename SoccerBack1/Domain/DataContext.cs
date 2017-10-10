﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class DataContext:DbContext
    {
        public DataContext():base("DefaultConnection")
        {
            
        }

        //video II 1/4 desabilkitamos elborrado en carscada
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
        public DbSet<League> Leagues { get; set; }

        public System.Data.Entity.DbSet<Domain.Team> Teams { get; set; }
    }
}
