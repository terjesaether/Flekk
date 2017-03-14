using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotzWeb.Models
{
    public class Tag
    {
        public Guid TagId { get; set; }
        public string TagName { get; set; }
        public virtual List<Spotz> Spotzes { get; set; }
    }
}