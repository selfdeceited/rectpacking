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
using RectPacking.Strategies.Heuristic;
using RectPacking.Strategies.Metaheuristic;
using RectPacking.Strategies.TimeEvaluating;

namespace WebUI
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetSimpleStaticSchedule(string tablewidth, string tableheight, string quantity, string ruleOption)
        {
            int tableRealWidth = Int32.Parse(tablewidth);
            int tableRealHeight = Int32.Parse(tableheight);
            int realQuantity = Int32.Parse(quantity);

            int realOption = Int32.Parse(ruleOption);

            var strategy = ChooseByRuleOption(realOption);
            var table = new VibroTable(tableRealWidth, tableRealHeight);
            var products = SampleInitializer.CreateRandomProdicts(realQuantity, true);
            var process = new StaticPlacement(table, products);
            process.Proceed(strategy);
            return process.JSON.ToString();
        }

        private static Strategy ChooseByRuleOption(int realOption)
        {
            switch (realOption) {
                case 1: 
                    return new EmptyStrategy();
                case 2:
                    return new SimpleHeuristicStrategy();
                case 3:
                    return new QuasiHumanHeuristicStrategy();
                case 4:
                    return new SimpleMetaheuristicStrategy();
                case 5:
                    return new CloseCommonFreezeTimeStrategy();
                case 6:
                    return new SlowFirstHeuristicStrategy();
                default: 
                    return new EmptyStrategy();
            }
        }
        [WebMethod]
        public static string GetSimpleDynamicSchedule(string tablewidth, string tableheight, string quantity, string ruleOption)
        {
            int tableRealWidth = Int32.Parse(tablewidth);
            int tableRealHeight = Int32.Parse(tableheight);
            int realQuantity = Int32.Parse(quantity);
            int realOption = Int32.Parse(ruleOption);

            var strategy = ChooseByRuleOption(realOption);
            var table = new VibroTable(tableRealWidth, tableRealHeight);
            var products = SampleInitializer.CreateRandomProdicts(realQuantity, true);
            var process = new DynamicPlacement(table, products);
            process.Proceed(strategy);
            return process.JSON.ToString();
        }
    }
}