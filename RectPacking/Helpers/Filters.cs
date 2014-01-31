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
            IEnumerable<COA> coaList = placement.Left;
            FilterIntersectionWithTable(placement.VibroTable, ref coaList);
            RemoveInvalidCOAs(ref coaList);//todo: don't like it dublicated
            FilterIntersectionWithPlacedCOAs(placement.Placed, ref coaList);
            RemoveInvalidCOAs(ref coaList);
            FilterComplexIntersection(placement.Placed, ref coaList);
            RemoveInvalidCOAs(ref coaList);
            return coaList.ToList();
        }

        public static void FilterIntersectionWithTable(VibroTable vibroTable, ref IEnumerable<COA> coaList )
        {
            foreach (var coa in coaList)
            {
                 coa.IsValid = coa.Points.TrueForAll(point => point.IsValid && point.IsWithin(vibroTable));
            }
        }

        public static void FilterIntersectionWithPlacedCOAs(List<COA> placedCOAs, ref IEnumerable<COA> coaList)
        {
            foreach (var coa in coaList)
            {
                foreach (var placedCOA in placedCOAs)
                {
                    coa.IsValid = coa.IsValid && coa.Points.TrueForAll(point => point.IsOutsideOf(placedCOA));
                    /* note: coa.IsValid = coa.IsValid && blahblah
                     * all COAs are valid at the beginning
                     * to make it in the way: once became invalid, always invalid
                     * Any other suggestions appreciated.
                     */
                }
            }
        }
        public static void FilterComplexIntersection(List<COA> placedCOAs, ref IEnumerable<COA> coaList)
        {
            foreach (var coa in coaList)
            {
                foreach (var placedCOA in placedCOAs)
                {
                    coa.IsValid = coa.IsValid && !coa.HasIntersectionWith(placedCOA);
                    /* note: coa.IsValid = coa.IsValid && blahblah
                     * all COAs are valid at the beginning
                     * to make it in the way: once became invalid, always invalid
                     * Any other suggestions appreciated.
                     */
                }
            }
        }

        public static void RemoveInvalidCOAs(ref IEnumerable<COA> coaList)
        {
            if (coaList.Any(coa=>!coa.IsValid))
            {
                coaList = coaList.Where(coa => coa.IsValid);
            }
        }
    }
}

