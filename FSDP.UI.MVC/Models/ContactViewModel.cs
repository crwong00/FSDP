using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FSDP.UI.MVC.Models
{
    public class ContactViewModel
    {
        public string Name { get; set; }


        public string Email { get; set; }

        [UIHint("MultilineText")]
        public string Message { get; set; }


    }
}