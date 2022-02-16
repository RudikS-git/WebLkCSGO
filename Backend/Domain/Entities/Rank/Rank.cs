using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Rank
{
    public class Rank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public string ImageSource { get; set; }
        public int Lvl { get; set; }
    }
}
