using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.General
{
    public class GenericMenu
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public int Position { get; set; }

        public bool IsSelected { get; set; }

        public List<GenericMenu> ChildMenu { get; set; }
    }
}
