using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Pelco.Phoenix.PluginHostInterfaces;
using drawing = Pelco.Phoenix.PluginHostInterfaces.Drawing;
using System.Windows.Input;
using delegates = OverlayDrawings.Model.Utils.Delegates;
using Microsoft.Practices.Prism.Commands;

namespace OverlayDrawings.Model
{
    class NewLineViewModel : ViewModelBase
    {
        private Color? _color;
        private bool _animate;
        private delegates.DrawOverlay _drawDelegate;
        private double _x1, _y1, _x2, _y2;

        public NewLineViewModel(delegates.DrawOverlay drawDelegate)
        {
            _drawDelegate = drawDelegate;
            _color = Colors.Blue;
            _animate = false;

            DisplayName = "Line Configuration";
            DrawCommand = new DelegateCommand(() => ExecuteDraw());
        }

        public override string DisplayName { get; set; }

        public ICommand DrawCommand { get; private set; }

        public bool Animate
        {
            get
            {
                return _animate;
            }
            set
            {
                if (value != _animate)
                {
                    _animate = value;
                    OnPropertyChanged(nameof(Animate));
                }
            }
        }

        public double X1
        {
            get
            {
                return _x1;
            }
            set
            {
                _x1 = value;
                OnPropertyChanged(nameof(X1));
            }
        }

        public double Y1
        {
            get
            {
                return _y1;
            }
            set
            {
                _y1 = value;
                OnPropertyChanged(nameof(Y1));
            }
        }

        public double X2
        {
            get
            {
                return _x2;
            }
            set
            {
                _x2 = value;
                OnPropertyChanged(nameof(X2));
            }
        }

        public double Y2
        {
            get
            {
                return _y2;
            }

            set
            {
                _y2 = value;
                OnPropertyChanged(nameof(Y2));
            }
        }

        public Color? LineColor
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                OnPropertyChanged(nameof(LineColor));
            }
        }

        private void ExecuteDraw()
        {
            drawing.Line line = new drawing.Line
            {
                StartPoint = new drawing.NormalizedPoint { X = X1, Y = Y1 },
                EndPoint = new drawing.NormalizedPoint { X = X2, Y = Y2 },
            };

            line.SetLineColor(LineColor);

            _drawDelegate.Invoke(line, Animate);
        }
    }
}
