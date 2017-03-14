using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotzWeb.Models
{
    public class SpotzDetailViewModel
    {
        public SpotzDetailViewModel(Spotz spotz)
        {
            Spotz = spotz;
        }
        public Spotz Spotz { get; set; }

        public string GetLatestImage()
        {
            if (Spotz.Images.Count > 0)
            {
                return Spotz.Images.OrderByDescending(i => i.Timestamp).First().ImageUrl;
            }
            return "";
        }
    }
}