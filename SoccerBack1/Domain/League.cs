using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class League
    {
        [Key]
        public int LeagueId{ get; set; }

        [Required(ErrorMessage ="The field {0} is required")]
        [MaxLength(50,ErrorMessage ="The Maximun lenght for  field {0} is {1} characters")]
        [Index("League_Name_Index",IsUnique =true)]
        [Display(Name="League")]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

    }
}
