using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScraping.Repository
{
    public class LinkDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LinkValue { get; set; }
        public int NumberOfRatings { get; set; }
        public decimal Rating { get; set; }
        public int IdCategory { get; set; }
    }
}
