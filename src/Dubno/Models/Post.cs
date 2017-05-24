using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Dubno.Models
{
    [Table("Posts")]
    public class Post
    {

        [Key]
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public bool Approved { get; set; }
        public bool Pending { get; set; }
        public string AdminComment { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }
 
    }
}