using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Spaces
    {

        public int SpaceId { get; set; }

        public string SpaceName { get; set; }

        public bool SpaceIsAccessible { get; set; }

        public int SpaceOpenFrom { get; set; }

        public int SpaceOpenTo { get; set; }

        public decimal SpaceDailyRate { get; set; }

        public int SpaceMaxOccupancy { get; set; }
        
    }
}
