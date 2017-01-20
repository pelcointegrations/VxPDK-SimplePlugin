using Pelco.Phoenix.PluginHostInterfaces.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverlayDrawings.Model
{
    [Serializable]
    class Point : NormalizedPoint
    {
        public string DisplayString { get { return String.Format("{0} ,{1}", X, Y); } }
    }
}
