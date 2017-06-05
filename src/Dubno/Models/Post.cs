using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Dubno.Models
{
    [Table("Posts")]
    public class Post
    {

        [Key]
        public int PostId { get; set; }
        //[Required]
        public string Title { get; set; }
        //[Required]
        public string Description { get; set; }
        //[Required]
        public string Name { get; set; }
        //[Required]
        public string City { get; set; }
        //[Required]
        public string State { get; set; }

        //[Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool Approved { get; set; }
        public bool Pending { get; set; }
        public string AdminComment { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        public string ImageName { get; set; }
    }
}