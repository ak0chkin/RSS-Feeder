using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rss.Models
{
    public class Settings
    {
        public int UpdateFrequency { get; set; }
        public List<string> Urls { get; set; }
    }
}
