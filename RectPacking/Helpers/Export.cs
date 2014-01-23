using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Models;
using RectPacking.Operations;

namespace RectPacking.Helpers
{
    public static class Export
    {
        public static string ToJson(PlacementProcess placement)
        {
            var sb = new StringBuilder();
            sb.Append("{\"tables\":{");
            sb.Append("    \"type\": \"VibroTable\",");
            sb.Append("    \"index\": \"0\",");
            sb.Append("    \"width\": \"" + placement.VibroTable.Width + "\",");
            sb.Append("    \"height\": \"" + placement.VibroTable.Height + "\",");
            sb.Append("        },");

            sb.Append("\"coas\": [");
            foreach (var coa in placement.PlacedCOAs)
            {
                var rect = coa.ToRectangle();
                sb.Append("    {");
                sb.Append("    \"type\": \"COA\",");
                sb.Append("    \"index\": \"" + placement.PlacedCOAs.IndexOf(coa) + "\",");
                sb.Append("    \"x\": \"" + rect.X + "\",");
                sb.Append("    \"Y\": \"" + rect.Y + "\",");
                sb.Append("    \"width\": \"" + rect.Width + "\",");
                sb.Append("    \"height\": \"" + rect.Height + "\",");
                sb.Append("    }");

                if (placement.PlacedCOAs.Last() != coa)
                {
                    sb.Append("    ,");
                }

            }
            sb.Append("],");

            sb.Append("\"commonData\":{");
            sb.Append("    \"total area\": \" "+ placement.VibroTable.Area + "\",");
            var placedArea = CalculateArea(placement.PlacedCOAs);
            sb.Append("    \"placed area\": \" " + placedArea + "\",");
            var persentage = placedArea * 100 / placement.VibroTable.Area;
            sb.Append("    \"persentage\": \"" + persentage + "\",");
            sb.Append("        }");

            sb.Append("}");
            return sb.ToString();
        }

        public static long CalculateArea(List<COA> list )
        {
            return list.Sum(coa => coa.Product.Area);
        }
    }
}
