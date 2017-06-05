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
    [Table("Subscribers")]
    public class Subscriber
    {

        [Key]
        public int SubscriberId { get; set; }
        //[Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        //[Required]
        public string FirstName { get; set; }
        //[Required]
        public string LastName { get; set; }
        //[Required]
        public string City { get; set; }
        //[Required]
        public string State { get; set; }

    }
}