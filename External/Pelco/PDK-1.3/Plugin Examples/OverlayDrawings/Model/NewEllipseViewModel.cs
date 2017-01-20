using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Pelco.Phoenix.PluginHostInterfaces;
using drawing = Pelco.Phoenix.PluginHostInterfaces.Drawing;
using delegates = OverlayDrawings.Model.Utils.Delegates;
using Microsoft.Practices.Prism.Commands;

namespace OverlayDrawings.Model
{
    class NewEllipseViewModel : NewRectangleViewModel
    {
        public NewEllipseViewModel(delegates.DrawOverlay drawDelegate) : base(drawDelegate)
        {
            DrawCommand = new DelegateCommand(() => ExecuteDraw());
            DisplayName = "Ellipse Configuration";
        }

        public override string DisplayName { get; set; }

        public override ICommand DrawCommand { get; protected set; }

        private void ExecuteDraw()
        {
            drawing.Ellipse ellipse = new drawing.Ellipse
            {
                UpperLeft = new drawing.NormalizedPoint { X = Left, Y = Top },
                BottomRight = new drawing.NormalizedPoint { X = Right, Y = Bottom },
            };

            ellipse.SetLineColor(BorderColor);
            ellipse.SetFillColor(FillColor);

            _drawDelegate.Invoke(ellipse, Animate);
        }
    }
}
