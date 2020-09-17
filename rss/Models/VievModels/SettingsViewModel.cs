using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rss.Models
{
    public class SettingsViewModel
    {
        [Display(Name = "Частота обновления")]
        public int UpdateFrequency { get; set; }
        [Display(Name = "Источники")]
        public string Urls { get; set; }
    }
}
