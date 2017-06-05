using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dubno.Models
{
    public class EmailViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Attachment")]
        public ICollection<IFormFile> Attachments { get; set; }
    }

    public class BulkEmailViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string[] Email { get; set; }

        [Display(Name = "Attachment")]
        public ICollection<IFormFile> Attachments { get; set; }
    }
}