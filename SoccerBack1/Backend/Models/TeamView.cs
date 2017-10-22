using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class TeamView:Team
    {
        [Display(Name="Logo")]
        public HttpPostedFileBase LogoFile{ get; set; }
    }
}