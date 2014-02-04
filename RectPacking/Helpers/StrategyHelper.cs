using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;

namespace RectPacking.Helpers
{
    public static class StrategyHelper
    {
        public static IEnumerable<IAction> MaxArea(IEnumerable<IAction> actions)
        {
            return actions.OrderByDescending(a => a.Width * a.Height);
        }
    }
}
