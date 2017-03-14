using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpotzWeb.Models
{
    public class Spotz
    {
        public Guid SpotzId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Tag> Tags { get; set; }
        public virtual List<Image> Images { get; set; }
        public virtual List<TagsToSpotz> TagsToSpotzes { get; set; }
        public virtual List<CommentToSpotz> CommentToSpotzes { get; set; }


    }
}