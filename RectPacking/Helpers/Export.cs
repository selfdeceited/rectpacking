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
        public static StringBuilder ToJson(AbstractPlacement placement)
        {
            var sb = new StringBuilder();
            sb.Append("{\"tables\":{");
            sb.Append("    \"type\": \"VibroTable\",");
            sb.Append("    \"index\": \"0\",");
            sb.Append("    \"width\": \"" + placement.VibroTable.Width + "\",");
            sb.Append("    \"height\": \"" + placement.VibroTable.Height + "\"");
            sb.Append("        },");

            sb.Append("\"coas\": [");

            if (placement is PlacementProcess)
            {
                foreach (var coa in (placement as PlacementProcess).OnTable)
                {
                    sb.Append("    {");
                    sb.Append("    \"type\": \"COA\",");
                    sb.Append("    \"index\": \"" + placement.OnTable.IndexOf(coa) + "\",");
                    sb.Append("    \"X\": \"" + coa.Left + "\",");
                    sb.Append("    \"Y\": \"" + coa.Top + "\",");
                    sb.Append("    \"width\": \"" + coa.Width + "\",");
                    sb.Append("    \"height\": \"" + coa.Height + "\"");
                    sb.Append("    }");

                    if ((placement as PlacementProcess).OnTable.Last() != coa)
                    {
                        sb.Append("    ,");
                    }

                }
                sb.Append("],");
            }
            else
            {
                foreach (var coa in placement.OnTable)
                {
                    sb.Append("    {");
                    sb.Append("    \"type\": \"COA\",");
                    sb.Append("    \"index\": \"" + placement.OnTable.IndexOf(coa) + "\",");
                    sb.Append("    \"X\": \"" + coa.Left + "\",");
                    sb.Append("    \"Y\": \"" + coa.Top + "\",");
                    sb.Append("    \"width\": \"" + coa.Width + "\",");
                    sb.Append("    \"height\": \"" + coa.Height + "\"");
                    sb.Append("    }");

                    if (placement.OnTable.Last() != coa)
                    {
                        sb.Append("    ,");
                    }

                }
                sb.Append("],");
            }


            sb.Append("\"leftProducts\": [");
            foreach (var product in placement.ProductList)
            {
                sb.Append("    {");
                sb.Append("    \"index\": \"" + placement.ProductList.IndexOf(product) + "\",");
                sb.Append("    \"width\": \"" + product.Width + "\",");
                sb.Append("    \"height\": \"" + product.Height + "\"");
                sb.Append("    }");

                if (placement.ProductList.Last() != product)
                {
                    sb.Append("    ,");
                }

            }
            sb.Append("],");

            sb.Append("\"commonData\":{");
            sb.Append("    \"total area\": \" "+ placement.VibroTable.Area + "\",");
            var placedArea = CalculateArea(placement.OnTable);
            sb.Append("    \"placed area\": \" " + placedArea + "\",");
            var persentage = placedArea * 100 / placement.VibroTable.Area;
            sb.Append("    \"persentage\": \"" + persentage + "\"");
            sb.Append("        }");

            sb.Append("}");
            return sb;
        }

        public static long CalculateArea(List<IAction> list )
        {
            return list.Sum(coa => coa.Product.Area);
        }

        public static StringBuilder ToJson(List<Frame> frameList)
        {
            throw new NotImplementedException();
        }
    }
}
