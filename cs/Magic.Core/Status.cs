using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    // 110.6. A permanent's status is its physical state. There are four status categories, each of which has 
    // two possible values: tapped/untapped, flipped/unflipped, face up/face down, and phased in/phased out. 
    // Each permanent always has one of these values for each of these categories.
    public class Status
    {
        public bool Tapped { get; set; }
        public bool FaceDown { get; set; }
        public bool Flipped { get; set; }
        public bool Transformed { get; set; }
    }
}
