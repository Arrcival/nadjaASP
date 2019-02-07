using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadja.Models
{
    public class Craft
    {
        public int ID { get; set; }
        public int Amount { get; set; }

        public Item ItemCrafted { get; set; }

        public List<Item> ItemsNeeded { get; set; }
    }
}
