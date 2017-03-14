using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotzWeb.Models
{
    public class TagsToSpotz
    {
        public int TagsToSpotzId { get; set; }
        public virtual IEnumerable<Spotz> Spotzes { get; set; }
    }

    public class CommentToSpotz
    {
        public int CommentToSpotzId { get; set; }
        public Guid SpotzId { get; set; }
        public virtual Spotz Spotz { get; set; }
    }
}