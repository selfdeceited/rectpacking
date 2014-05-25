using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Helpers;
using RectPacking.Models;
using RectPacking.Strategies;

namespace RectPacking.Operations
{
    public class DynamicPlacement : PlacementProcess
    {

        public DynamicPlacement(VibroTable vibroTable, List<Product> productList)
            : base(vibroTable, productList)
        {

        }

        public void IncreaseSessionTime(COA coa)
        {
            this.TimeLine.Current.Add(coa.Product.FreezeTime);
        }

        public IEnumerable<Point> CreateInitialMainPoints()
        {
            var vibroTable = this.VibroTable;
            yield return (new Point(vibroTable.Left, vibroTable.Top, true));
            yield return (new Point(vibroTable.Left + vibroTable.Width, vibroTable.Top, true));
            yield return (new Point(vibroTable.Left, vibroTable.Top + vibroTable.Height, true));
            yield return (new Point(vibroTable.Left + vibroTable.Width, vibroTable.Top + vibroTable.Height, true));
        }

        public override void Proceed(Strategy manager, bool debug = false, string folderTag = null)
        {

            Image = new ImageHelper(this, folderTag);
            var newPoints = this.MainPoints;
            var best = SampleBestCOA(); //sample is used to get into the cycle

            this.TimeLine = new TimeLine(new DateTime(2013, 5, 15));
            this.TimeLine.AddInitialTimeStamp();
            var iteration = 0;
            
            while (PlacementIsOk())
            {
                if (TimeLine.NowIsOnTimeStamp())
                {
                    var deletedProducts = RemoveProductsFor(TimeLine.StartDate + TimeLine.Current);
                    if (debug) Image.DeleteProducts(this, deletedProducts);

                    if (iteration > 0)
                    {
                        newPoints = FilterValidPoints();
                    }

                    var startCycle = true;
                    while (startCycle)
                    {

                        iteration++;
                        CreateCOAsForPoints(newPoints);
                        best = ChooseBestCOA(manager, iteration);
                        AdjustTimeStampsState();
                        if (best != null)
                        {
                            best.Product.PlacedOnTable = TimeLine.StartDate + TimeLine.Current;
                            TimeLine.TimeStamps.Add(new TimeStamp(best.Product));

                            OnTable.Add(best);
                            
                            newPoints = ManagePointsFor(best);
                            DeleteCOAsWith(best.Product);
                            //if (debug) Image.UpdateStatus(this); 
                        }
                        else
                        {
                            if (debug) Image.UpdateStatus(this, newPoints);
                            startCycle = false;
                            TimeLine.ProlongCurrentToNextTimeStamp();
                            
                        }
                    }
                }
                else
                {
                    if (debug) Image.UpdateStatus(this, newPoints);
                    TimeLine.ProlongCurrentToNextTimeStamp();
                    
                }
            }

            if (debug) Image.UpdateStatus(this);

            TimeLine.EndTime = TimeLine.StartDate + TimeLine.Current;

            this.JSON = Export.ToJson(this);

        }

        private void AdjustTimeStampsState()
        {
            foreach (var stamp in this.TimeLine.TimeStamps)
            {
                if (stamp.Time <= TimeLine.StartDate + TimeLine.Current)
                {
                    stamp.State = TimeStampState.Initialized;
                }
            }
        }

        private List<COA> RemoveProductsFor(DateTime currentTimeStamp)
        {
            var list = new List<COA>();
            for (int i = 0; i < OnTable.Count; i++)
            {
                if (OnTable[i].Product.PlacedOnTable + OnTable[i].Product.FreezeTime == currentTimeStamp)
                {
                    list.Add(OnTable[i]);
                    DeletePointsFor(OnTable[i]);
                    DeleteCOAsWith(OnTable[i].Product);
                    OnTable.RemoveAt(i);
                    i--;
                }
            }
            Done.AddRange(list);
            return list;
            //points must be deleted too
        }

        private bool PlacementIsOk()
        {
            return
                TimeLine.TimeStamps.Any(
                    ts => ts.State == TimeStampState.IsFurther || ts.State == TimeStampState.InQueue);
        }

        private void DeletePointsFor(COA coa)
        {
            var pointsOfCOAsOnTable = OnTable.SelectMany(c => c.Points).ToList();
            foreach (var point in coa.Points)
            {
                if (!point.CheckIfHasPointInSameLocation(pointsOfCOAsOnTable))
                {
                   // MainPoints.RemovePointAtLocation(point.X, point.Y);
                }
            }
            // удалять только те точки, которые не принадлежат (по местоположению) другим из уже размещенных COA.

        }

        public override void ProceedFrom(Strategy manager, List<IAction> placed, ImageHelper image, int iteration, bool debug = false)
        {
            throw new NotImplementedException(
                "Операции с комплексным использованием разных технологий упаковки пока не работают для динамического размещения");
        }

        public bool ResumePlacement(COA best)
        {
            return best != null;//todo: Stopping criteria?
        }

        public void CreateCOAsForPoints(List<Point> points = null, bool withConstrains = true)
        {
            if (points == null) 
                points = this.MainPoints;
            List<COA> list = this.Left;
            foreach (var point in points)//because points can differ
            {
                foreach (var product in this.ProductList)
                {
                    COA.ToPack(this.VibroTable, product, point, ref list);
                }
            }
            if (withConstrains)
                list = Filters.FilterCOAs(this);
            this.Left = list;
        }

        public COA ChooseBestCOA(Strategy manager, int iteration)
        {
            manager.UpdateIterationStatFor(iteration, this);
            if (!Left.Any()) 
                return null;

            return (COA) manager.Solve(Left);
        }

        public void ManagePoints()
        {
            //двойной цикл по всем точкам: если i != j и совпадает с другой, удалить
            for (int j = 0; j < MainPoints.Count; j++)
            {
                if (MainPoints[j].CheckIfHasPointInSameLocation(MainPoints))
                {
                    MainPoints.RemoveAt(j);
                    j--;
                }
            }
        }
        public List<Point> ManagePointsFor(COA best)
        {

                var newPoints = new List<Point>();

                foreach (var point in best.Points)
                {
                    //check if has valid COAs from this point
                    if (!point.IsMain && !point.CheckIfHasPointInSameLocation(MainPoints))
                    {
                        point.IsMain = true;
                        newPoints.Add(point);
                        MainPoints.Add(point);
                    }
                }
                return newPoints;
        }

        private List<Point> FilterValidPoints()
        {
            //filter points which are contained at location of possible(that means, valid) COAs.
            //We filter not to enumerate all MainPoints, they are huge

            return MainPoints.Where(p => p.ContainsInCOAsFrom(OnTable)).ToList();
        }

        public void DeleteCOAsWith(Product bestProduct)
        {
            this.ProductList = Product.RemoveFromListWithId(bestProduct.Identifier, this.ProductList);
            this.Left = COA.RemoveFromListWithId(bestProduct.Identifier, this.Left);
        }

        public COA SampleBestCOA()
        {
            const int greatNumber = 1000000;
            return new COA(new Product(greatNumber, greatNumber, false),
                new Point(), COA.CornerType.TopLeft, false);
        }
    }
}
