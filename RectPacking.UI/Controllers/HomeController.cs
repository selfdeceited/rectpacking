using Newtonsoft.Json;
using RectPacking.Models;
using RectPacking.Operations;
using RectPacking.Strategies;
using RectPacking.Strategies.Heuristic;
using RectPacking.Strategies.Metaheuristic;
using RectPacking.Strategies.TimeEvaluating;
using RectPacking.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace RectPacking.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [WebMethod]
        public string GetSimpleStaticSchedule(string tablewidth, string tableheight, string quantity, string ruleOption)
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
            switch (realOption)
            {
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
        public string GetSimpleDynamicSchedule(string tablewidth, string tableheight, string quantity, string ruleOption)
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

        [WebMethod]
        public string GetProceededStaticSchedule(string staticData, string frameId, string ruleOption)
        {
            int frameRealId = Int32.Parse(frameId);
            int realOption = Int32.Parse(ruleOption);
            if (string.IsNullOrEmpty(staticData)) 
                return "no static data available";

            var frameList = JsonConvert.DeserializeObject<List<Frame>>(staticData);
            var strategy = ChooseByRuleOption(realOption);

            var process = new StaticPlacement(frameList);
            process.ProceedFromFrame(frameRealId, ChooseByRuleOption(realOption));
            return process.JSON.ToString();
        }

        public ActionResult Primitive()
        {
            return View();
        }

        public ActionResult Shop()
        {
            return View();
        }
    }
}
