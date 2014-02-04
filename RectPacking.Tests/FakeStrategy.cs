using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;
using RectPacking.Strategies;

namespace RectPacking.Tests
{
    public abstract class FakeStrategy: Strategy
    {
        public FakeStrategy(Algorythm AlgorythmType, Complexity ComplexityType)
            : base(AlgorythmType, ComplexityType)
        { }

        public FakeStrategy()
            : base(Algorythm.Other, Complexity.Unknown)
        { }

    }
}
