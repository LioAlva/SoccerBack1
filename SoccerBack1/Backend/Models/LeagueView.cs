using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class LeagueView:League
    {
        public HttpPostedFileBase LogoFile { get; set; }
    }
}