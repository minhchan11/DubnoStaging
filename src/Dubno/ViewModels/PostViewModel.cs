using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Dubno.Models;

namespace Dubno.ViewModels
{
    public class PostViewModel
    {
        [Required]
        [Display(Name = "Image")]
        public ICollection<IFormFile> files { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }


        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }




    }
}