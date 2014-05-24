using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using RectPacking.Models;
using RectPacking.Operations;
using RectPacking.Strategies;
using RectPacking.Tests;

namespace WebUI
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetSimpleStaticSchedule(string tablewidth, string tableheight, string quantity)
        {
            int tableRealWidth = Int32.Parse(tablewidth);
            int tableRealHeight = Int32.Parse(tableheight);
            int realQuantity = Int32.Parse(quantity);
            var table = new VibroTable(tableRealWidth, tableRealHeight);
            var products = SampleInitializer.CreateRandomProdicts(realQuantity, false);
            var process = new StaticPlacement(table, products);
            process.Proceed(new EmptyStrategy());
            return process.JSON.ToString();
        }
    }
}