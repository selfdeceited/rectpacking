using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RectPacking.Helpers;
using RectPacking.Models;
using RectPacking.Strategies;
using Point = RectPacking.Models.Point;

namespace RectPacking.Operations
{
    public class StaticPlacement : PlacementProcess
    {
        public List<Frame> FrameList { get; set; }

        public StaticPlacement(VibroTable vibroTable, List<Product> productList) : base(vibroTable, productList)
        {
            FrameList = new List<Frame>();
        }


        public override void Proceed(Strategy manager, bool debug = false, string folderTag = null)
        {
            
            Image = new ImageHelper(this, folderTag);
            

            var iteration = 0;
            var frameIteration = 0;
            while (ResumeProcess())
            {
                MainPoints = CreateInitialMainPoints().ToList();
                var newPoints = CreateInitialMainPoints().ToList();
                var best = SampleBestCOA();//sample is used to get into the cycle
                var currentFrame = new Frame
                {   
                    VibroTable = this.VibroTable,
                    StartDate = TimeLine.StartDate + TimeLine.Current,
                    Index = frameIteration,
                    SetList = new List<COA>()
                };

                while (ResumePlacement(best))
                {
                    iteration++;
                    CreateCOAsForPoints(newPoints);
                    best = ChooseBestCOA(manager, iteration);

                    if (best != null)
                    {
                        OnTable.Add(best);
                        currentFrame.SetList.Add(best);
                        if (debug) Image.UpdateStatus(this, newPoints, best);
                        newPoints = ManagePointsFor(best);
                        DeleteCOAsWith(best.Product);
                    }
                }
                OnTable.RemoveAll(coa => true);
                currentFrame.GetStats();
                this.FrameList.Add(currentFrame);
                frameIteration++;
                TimeLine.Current.Add(currentFrame.TimeForFrame);
            }

            if (debug) Image.UpdateStatus(this);
            this.JSON = new StringBuilder(JsonConvert.SerializeObject(this.FrameList));
            //var folder = string.IsNullOrEmpty(folderTag) ? "" : folderTag + "\\";
            //var textFile = new System.IO.StreamWriter("C:\\test\\" + folder + "data.json");

            //textFile.Write(json);
            //textFile.Close();

        }

        private bool ResumeProcess()
        {
            return this.ProductList.Any();
        }

        public void CreateInitialMainPoints(VibroTable vibroTable)
        {
            this.MainPoints.Add(new Point(vibroTable.Left, vibroTable.Top, true));
            this.MainPoints.Add(new Point(vibroTable.Left + vibroTable.Width, vibroTable.Top, true));
            this.MainPoints.Add(new Point(vibroTable.Left, vibroTable.Top + vibroTable.Height, true));
            this.MainPoints.Add(new Point(vibroTable.Left + vibroTable.Width, vibroTable.Top + vibroTable.Height, true));
        }

        public override void ProceedFrom(Strategy manager, List<IAction> placed, ImageHelper image, int iteration, bool debug = false)
        {
            this.Image = image;
            this.Iteration = iteration;

            var newPoints = MainPoints;
            if (placed.TrueForAll(iAction => iAction is COA))
            {
                var newPlaced = placed.OfType<COA>().ToList();
                foreach (var action in newPlaced)
                {//перенести все точки
                    foreach (var point in action.Points)
                    {
                        point.X += action.VibroTable.Left;
                        point.Y += action.VibroTable.Top;
                        MainPoints.Add(point);
                    }
                    //                    this.MainPoints.AddRange(action.Points);
                }
                this.OnTable = newPlaced;
            }
            else//если нет, до делаем их таковыми
            {
                this.OnTable.Clear();

                foreach (var action in placed)
                {
                    var coa = new COA(action.Product, new Point(action.Left, action.Top), COA.CornerType.TopLeft, false, this.VibroTable);
                    this.OnTable.Add(coa);
                }

                foreach (var action in this.OnTable)
                {
                    foreach (var point in action.Points)
                    {
                        point.X += action.VibroTable.Left;
                        point.Y += action.VibroTable.Top;
                        MainPoints.Add(point);
                    }
                }

            }

            MainPoints.AddRange(CreateInitialMainPoints());

            //+ generating points for previous iterations 

            //+ link to usual Proceed cycle
            var best = SampleBestCOA();
            ManagePoints();

            while (ResumePlacement(best))
            {
                this.Iteration++;
                CreateCOAsForPoints(newPoints);
                best = ChooseBestCOA(manager, iteration);

                if (best != null)
                {
                    this.OnTable.Add(best);
                    if (debug) Image.UpdateStatus(this, newPoints, best);
                    newPoints = ManagePointsFor(best);
                    DeleteCOAsWith(best.Product);
                }

            }

            if (debug) Image.UpdateStatus(this);
            //base.OnTable = this.OnTable.Cast<IAction>().ToList();
        }
        public bool ResumePlacement(COA best)
        {
            return best != null;//todo: Stopping criteria?
        }

        public IEnumerable<Point> CreateInitialMainPoints()
        {
            var vibroTable = this.VibroTable;
            yield return (new Point(vibroTable.Left, vibroTable.Top, true));
            yield return (new Point(vibroTable.Left + vibroTable.Width, vibroTable.Top, true));
            yield return (new Point(vibroTable.Left, vibroTable.Top + vibroTable.Height, true));
            yield return (new Point(vibroTable.Left + vibroTable.Width, vibroTable.Top + vibroTable.Height, true));
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

            return (COA)manager.Solve(Left);
        }

        public void ManagePoints()
        {
            //двойной цикл по всем точкам: если i != j и совпадает с другой, удалить
            for (int j = 0; j < MainPoints.Count; j++)
            {
                if (MainPoints[j].CheckIfHasPointInSameLocation(MainPoints))
                {
                    MainPoints.RemoveAt(j);
                }
            }
        }
        public List<Point> ManagePointsFor(COA best)
        {
            var newPoints = new List<Point>();
            foreach (var point in best.Points)
            {
                //check if has valid COAs from this point
                for (int index = 0; index < MainPoints.Count; index++)
                {
                    var hasValidCOAs = Left.Any(c => c.MainPoint == MainPoints[index]);
                    if (!hasValidCOAs) MainPoints.RemoveAt(index);
                }

                if (!point.IsMain && !point.CheckIfHasPointInSameLocation(MainPoints))
                {
                    point.IsMain = true;
                    newPoints.Add(point);
                }
            }
            return newPoints;
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
