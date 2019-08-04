using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FileProcessor.Web.Models
{
    public class UserModel
    {
        [DisplayName("Adı")]
        public string Name { get; set; }

        [DisplayName("Soyadı")]
        public string Surname { get; set; }

        [DisplayName("Telefon")]
        public string PhoneNumber { get; set; }

        [DisplayName("Adres")]
        public string Address { get; set; }
    }
}