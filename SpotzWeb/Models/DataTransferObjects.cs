using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotzWeb.Models
{
    public class DataTransferObjects
    {
    }

    public class AddedComment
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
    }

    public class UpdateDescriptionText
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }
}