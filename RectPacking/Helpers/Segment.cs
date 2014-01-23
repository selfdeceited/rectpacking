using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;

namespace RectPacking.Helpers
{
    public class Segment
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public enum Direction
        {
            Left, Right, Up, Down, Unknown
        }

        public Direction ToGo { get; set; }

        public Segment(Direction dir)
        {
            ToGo = dir;
        }

        //todo:do we need this?
        public void Define(COA coa, List<COA> placed)
        {
            //search for local optimum and define min & max
        }
    }
}
