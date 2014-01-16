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
            RemoveInvalidCOAs(ref coaList);//todo: don't like it dublicated
            FilterIntersectionWithPlacedCOAs(placement.PlacedCOAs, ref coaList);
            RemoveInvalidCOAs(ref coaList);
            FilterComplexIntersection(placement.PlacedCOAs, ref coaList);
            RemoveInvalidCOAs(ref coaList);
            return coaList;
        }

        public static void FilterIntersectionWithTable(VibroTable vibroTable, ref List<COA> coaList )
        {
            foreach (var coa in coaList)
            {
                 coa.IsValid = coa.Points.TrueForAll(point => point.IsValid && point.IsWithin(vibroTable));
            }
        }

        public static void FilterIntersectionWithPlacedCOAs(List<COA> placedCOAs, ref List<COA> coaList)
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
        public static void FilterComplexIntersection(List<COA> placedCOAs, ref List<COA> coaList)
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

        public static void RemoveInvalidCOAs(ref List<COA> coaList)
        {
            if (coaList.Any(coa=>!coa.IsValid))
            {
                coaList = coaList.Where(coa => coa.IsValid).ToList();
            }
            /* todo: FIX/REFACTOR
             * with every .Where().ToList() coaList deletes itself and occupies more place in CLR heap.
             * this is bad because it makes GarbageCollector work more and leads to slower performance.
             */
        }
    }
}

