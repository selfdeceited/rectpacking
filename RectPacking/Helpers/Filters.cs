using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;
using RectPacking.Operations;

namespace RectPacking.Helpers
{
    public static class Filters
    {
        public static List<COA> FilterCOAs(PlacementProcess placement)
        {
            var coaList = placement.COAs;
            FilterIntersectionWithTable(placement.VibroTable, ref coaList);
            return coaList;
        }

        public static void FilterIntersectionWithTable(VibroTable vibroTable, ref List<COA> coaList )
        {
            foreach (var coa in coaList)
            {
                if (!coa.Product.CanContainIn(vibroTable)) continue;

        
            }
        }
    }
}
