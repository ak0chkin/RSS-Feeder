using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rss.Models.VievModels
{
    public class FeedViewModel
    {
        public IEnumerable<string> Urls { get; set; }
        public IEnumerable<Feed> Data { get; set; }
    }
}
