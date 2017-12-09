using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class DataContextLocal:DataContext
    {
        public System.Data.Entity.DbSet<Domain.Date> Dates { get; set; }
    }
}