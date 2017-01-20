using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Pelco.Phoenix.PluginHostInterfaces;
using drawing = Pelco.Phoenix.PluginHostInterfaces.Drawing;
using delegates = OverlayDrawings.Model.Utils.Delegates;
using Microsoft.Practices.Prism.Commands;

namespace OverlayDrawings.Model
{
    class NewRectangleViewModel : ViewModelBase
    {
        protected delegates.DrawOverlay _drawDelegate;

        private bool _animate;
        private Color? _borderColor, _fillColor;
        private double _top, _left, _bottom, _right, _borderThickness;

        public NewRectangleViewModel(delegates.DrawOverlay drawDelegate)
        {
            _drawDelegate = drawDelegate;
            _animate = false;

            BorderColor = Colors.Blue;
            FillColor = null;
            DisplayName = "Rectangle Configuration";
            DrawCommand = new DelegateCommand(() => ExecuteDraw());
        }

        public override string DisplayName { get; set; }

        public virtual ICommand DrawCommand { get; protected set; }

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

        public double Top
        {
            get
            {
                return _top;
            }
            set
            {
                _top = value;
                OnPropertyChanged(nameof(Top));
            }
        }

        public double Bottom
        {
            get
            {
                return _bottom;
            }
            set
            {
                _bottom = value;
                OnPropertyChanged(nameof(Bottom));
            }
        }

        public double Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
                OnPropertyChanged(nameof(Left));
            }
        }

        public double Right
        {
            get
            {
                return _right;
            }
            set
            {
                _right = value;
                OnPropertyChanged(nameof(Right));
            }
        }

        public Color? BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                OnPropertyChanged(nameof(BorderColor));
            }
        }

        public Color? FillColor
        {
            get
            {
                return _fillColor;
            }
            set
            {
                _fillColor = value;
                OnPropertyChanged(nameof(FillColor));
            }
        }

        private void ExecuteDraw()
        {
            drawing.Rectangle rectangle = new drawing.Rectangle
            {
                UpperLeft = new drawing.NormalizedPoint { X = Left, Y = Top },
                BottomRight = new drawing.NormalizedPoint { X = Right, Y = Right },
            };

            rectangle.SetLineColor(BorderColor);
            rectangle.SetFillColor(FillColor);

            _drawDelegate.Invoke(rectangle, Animate);
        }
    }
}
