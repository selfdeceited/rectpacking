using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;
using RectPacking.Helpers;
using RectPacking.Models;
using RectPacking.Strategies;
using Action = RectPacking.Models.Action;

namespace RectPacking.Operations
{
    public class SimplePlacementProcess: AbstractPlacement
    {
        public int CurrentYOffset { get; set; }
        public int CurrentXOffset { get; set; }
        public int maxHeight { get; set; }
        public bool stoppingTrigger { get; set; }
        public List<Action> NotPlaced { get; set; }

        public SimplePlacementProcess(VibroTable vibroTable, List<Product> productList) : base(vibroTable, productList)
        {
            stoppingTrigger = false;
            this.NotPlaced = new List<Action>();
        }

        public override void Proceed(Strategy manager, bool debug = false, string folderTag = null)
        {
            var image = new ImageHelper(this, folderTag);
            var best = SampleBestAction();//sample is used to get into the cycle

            //initialize Left
            foreach (var product in ProductList)
            {
                Left.Add(new Action
                {
                    Product = product,
                    Width = product.Width,
                    Height = product.Height
                });
            }

            var iteration = 0;
            while (ResumePlacement(best))
            {
                iteration++;
                best = Place(iteration);
                if (best != null)
                {
                    Done.Add(best);
                    if (debug) image.UpdateStatus(this, best);
                    this.ProductList = Product.RemoveFromListWithId(best.Product.Identifier, this.ProductList);
                    this.Left.Remove(best);
                }
            }
            if (debug) image.UpdateStatus(this);
            var json = Export.ToJson(this);
            var folder = string.IsNullOrEmpty(folderTag) ? "" : folderTag + "\\";
            var textFile = new System.IO.StreamWriter("C:\\test\\" + folder + "data.json");

            textFile.Write(json);
            textFile.Close();
            this.Image = image;
            this.Left.AddRange(this.NotPlaced);
        }

        public override void ProceedFrom(Strategy manager, List<IAction> placed, ImageHelper image, int iteration, bool debug = false)
        {
            throw new NotImplementedException();
        }

        private Action Place(int iteration)
        {
            if (!Left.Any() || stoppingTrigger) return null;
            var action = (Action)Left.First();

           // TryPlace();//to the right
           // Enter();//down
            if (CurrentXOffset + action.Width < VibroTable.Width)  //to the right; if is fit to the right side
            {
                if (action.CanContainIn(VibroTable, CurrentXOffset, CurrentYOffset))
                {
                    action.Top = CurrentYOffset;
                    action.Left = CurrentXOffset;
                    CurrentXOffset += action.Width;
                    if (action.Height > maxHeight)
                        maxHeight = action.Height;
                }
                else
                {
                    //Take other
                    NotPlaced.Add(action);
                    Left.Remove(action);
                    return Place(iteration);
                }
            }
            else//try to increase Y Offset - if it could, like "Enter" action
            {
                if (action.CanContainIn(VibroTable, 0, CurrentYOffset + maxHeight))
                {
                    CurrentXOffset = 0;
                    CurrentYOffset += maxHeight;
                    maxHeight = 0;
                    action.Top = CurrentYOffset;
                    action.Left = CurrentXOffset;

                    CurrentXOffset += action.Width;
                }
                else
                {
                    stoppingTrigger = true;
                    return null;
                }
            }

            return action;

        }

        private Action SampleBestAction()
        {
            return new Action{Left = -50, Top = -50, Width = 100000, Height = 100000, Product = new Product(10000, 10000, false)};
        }

        public bool ResumePlacement(Action best)
        {
            return !stoppingTrigger || best != null;//todo: Stopping criteria?
        }


    }
}
