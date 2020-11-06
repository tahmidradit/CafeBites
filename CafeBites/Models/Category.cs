using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CafeBites.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(80), Display(Name ="Category Name")]
        public string Name { get; set; }
    }
}
