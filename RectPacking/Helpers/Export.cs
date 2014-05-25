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
            if (placement is DynamicPlacement) {
                var dynamicPlacement = placement as DynamicPlacement;
                sb.Append("{ \"coas\" : [");
                foreach (var coa in dynamicPlacement.Done) 
                {
                    sb.Append("{");

                    sb.Append(" \"Left\" : \"");
                    sb.Append(coa.Left);
                    sb.Append("\",");

                    sb.Append(" \"Top\" : \"");
                    sb.Append(coa.Top);
                    sb.Append("\",");

                    sb.Append(" \"Width\" : \"");
                    sb.Append(coa.Width);
                    sb.Append("\",");

                    sb.Append(" \"Height\" : \"");
                    sb.Append(coa.Height);
                    sb.Append("\",");

                    sb.Append(" \"Start\" : \"");
                    sb.Append(coa.Product.PlacedOnTable.ToString("R"));
                    sb.Append("\",");

                    sb.Append(" \"Finish\" : \"");
                    sb.Append(coa.Product.PlacedOnTable.Add(coa.Product.FreezeTime).ToString("R"));
                    sb.Append("\"");


                    sb.Append("}");
                    if (coa != dynamicPlacement.Done.Last())
                    {
                        sb.Append(",");
                    }

                }

                sb.Append("], \"Table\" :{");

                sb.Append(" \"Width\" : \"");
                sb.Append(dynamicPlacement.VibroTable.Width);
                sb.Append("\",");

                sb.Append(" \"Height\" : \"");
                sb.Append(dynamicPlacement.VibroTable.Height);
                sb.Append("\"");

                sb.Append("}");

                sb.Append("}");
            }
            return sb;
        }

        public static long CalculateArea(List<IAction> list )
        {
            return list.Sum(coa => coa.Product.Area);
        }
    }
}
