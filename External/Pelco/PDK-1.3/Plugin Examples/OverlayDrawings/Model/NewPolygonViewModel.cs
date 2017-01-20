
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using drawing = Pelco.Phoenix.PluginHostInterfaces.Drawing;
using delegates = OverlayDrawings.Model.Utils.Delegates;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;

namespace OverlayDrawings.Model
{
    class NewPolygonViewModel : ViewModelBase
    {
        private bool _animate;
        private Color? _borderColor, _fillColor;
        private delegates.DrawOverlay _drawDelegate;

        public NewPolygonViewModel(delegates.DrawOverlay drawDelegate)
        {
            _drawDelegate = drawDelegate;
            _animate = false;

            DisplayName = "Polygon Configuration";
            Points = new ObservableCollection<Point>();
            BorderColor = Colors.Blue;
            FillColor = null;
            DrawCommand = new DelegateCommand(() => ExecuteDraw());
            ClearPointsCommand = new DelegateCommand(() => ExecuteClearPoints());
            AddNewPointCommand = new DelegateCommand<Point>(p => AddNewPoint(p));
            RemovePointsCommand = new DelegateCommand<IList<object>>(p => ExecuteRemovePoints(p));
        }

        #region Commands

        public ICommand DrawCommand { get; private set; }

        public ICommand AddNewPointCommand { get; private set; }

        public ICommand RemovePointsCommand { get; private set; }

        public ICommand ClearPointsCommand { get; private set; }

        #endregion

        #region Properties

        public override string DisplayName { get; set; }

        public ObservableCollection<Point> Points { get; private set; }

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

        #endregion

        private void ExecuteDraw()
        {
            drawing.NormalizedPoint[] points = Points.Select(v => new drawing.NormalizedPoint { X = v.X, Y = v.Y }).ToArray();

            drawing.Polygon polygon = new drawing.Polygon(points);
            polygon.SetLineColor(BorderColor);
            polygon.SetFillColor(FillColor);

            _drawDelegate(polygon, Animate);
        }

        private void ExecuteRemovePoints(IList<object> points)
        {
            foreach (Point point in points.Cast<Point>())
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => Points.Remove(point)));
            }
        }

        private void ExecuteClearPoints()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => Points.Clear()));
        }

        private void AddNewPoint(Point point)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => Points.Add(point)));
        }
    }
}
