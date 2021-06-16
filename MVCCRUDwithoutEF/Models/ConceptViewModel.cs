using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCCRUDwithoutEF.Models
{
    public class ConceptViewModel
    {
        [Key]
        public int ConceptID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Range(1,int.MaxValue,ErrorMessage ="Should be greated than or equal to 1")]
        public int Price { get; set; }
    }
}
