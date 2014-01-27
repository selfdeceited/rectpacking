using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RectPacking.Models
{
    public interface IAction
    {
        int Width { get; set; }
        int Height { get; set; }

        int Left { get; set; }
        int Top { get; set; }

        double Distance { get; set; }
    }
}
