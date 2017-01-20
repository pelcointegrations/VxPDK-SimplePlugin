using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using drawing = Pelco.Phoenix.PluginHostInterfaces.Drawing;
using delegates = OverlayDrawings.Model.Utils.Delegates;
using System.Windows;
using System.Windows.Input;
using System.Collections;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Threading;

namespace OverlayDrawings.Model
{
    public class OverlayData : ViewModelBase
    {
        private static int _randSeed = 0;
        private bool _isAnimated;

        public OverlayData(drawing.Overlay overlay)
        {
            ActualOverlay = overlay;
            XOffset = 0.01;
            YOffset = 0.01;
            _isAnimated = false;

            InitAnimationDirection();
        }
        
        /// <summary>
        /// The type of overlay
        /// </summary>
        public drawing.OverlayType Type { get { return ActualOverlay.TypeOfOverlay; } }

        /// <summary>
        /// Flag inidicating if the shape is animated or not
        /// </summary>
        public bool Animated
        {
            get
            {
                return _isAnimated;
            }
            set
            {
                if (value != _isAnimated)
                {
                    _isAnimated = value;
                    OnPropertyChanged(nameof(Animated));
                }
            }
        }

        /// <summary>
        /// The id of the overlay
        /// </summary>
        public string ID { get { return ActualOverlay.ID; } }

        /// <summary>
        /// The X direction of an animated overlay
        /// </summary>
        private int XDirection { get; set; }

        /// <summary>
        /// The Y direction of an animated overlay
        /// </summary>
        private int YDirection { get; set; }

        /// <summary>
        /// The x offset to add to an overlay when it is being animated
        /// </summary>
        private double XOffset { get; set; }

        /// <summary>
        /// The y offset to add to an overlay when it is being animated
        /// </summary>
        private double YOffset { get; set; }

        /// <summary>
        /// The actual overlay instance created
        /// </summary>
        internal drawing.Overlay ActualOverlay { get; set; }

        /// <summary>
        /// This property is not used
        /// </summary>
        public override string DisplayName { get; set; }

        public void Translate()
        {
            switch (ActualOverlay.TypeOfOverlay)
            {
                case drawing.OverlayType.Line:
                    TranslateLine();
                    break;

                case drawing.OverlayType.Polygon:
                    TranslateShape();
                    break;

                case drawing.OverlayType.Ellipse:
                case drawing.OverlayType.Rectangle:
                    TranslateRectangle();
                    break;
                default:
                    break;
            }
        }

        private void TranslateLine()
        {
            drawing.Line line = ActualOverlay as drawing.Line;

            line.X1 += (XDirection * XOffset);
            line.Y1 += (YDirection * YOffset);
            line.X2 += (XDirection * XOffset);
            line.Y2 += (XDirection * YOffset);

            // Changing direction if we reach a border with the left hand point.
            if ((XDirection == 1) && (line.X1 > 1.0))  { XDirection = -1; }
            if ((XDirection == -1) && (line.X1 < 0.0)) { XDirection = 1;  }
            if ((YDirection == 1) && (line.Y1 > 1.0))  { YDirection = -1; }
            if ((YDirection == -1) && (line.Y1 < 0.0)) { YDirection = 1;  }

            ActualOverlay = line;
        }

        private void TranslateRectangle()
        {
            drawing.Rectangle rectangle = ActualOverlay as drawing.Rectangle;

            rectangle.UpperLeft.X   += (XDirection * XOffset);
            rectangle.UpperLeft.Y   += (YDirection * YOffset);
            rectangle.BottomRight.X += (XDirection * XOffset);
            rectangle.BottomRight.Y += (YDirection * YOffset);

            // Changing direction if we reached a border with the upper left corner.
            if ((XDirection == 1) && (rectangle.BottomRight.X > 1.0))  { XDirection = -1; }
            if ((XDirection == -1) && (rectangle.UpperLeft.X < 0.0))   { XDirection = 1;  }
            if ((YDirection == 1) && (rectangle.BottomRight.Y > 1.0))  { YDirection = -1; }
            if ((YDirection == -1) && (rectangle.UpperLeft.Y < 0.0))   { YDirection = 1;  }
        }

        private void TranslateShape()
        {
            drawing.Shape shape = ActualOverlay as drawing.Shape;

            int xDirection = XDirection, yDirection = YDirection;
            foreach (drawing.NormalizedPoint point in shape.Points)
            {
                point.X += (XDirection * XOffset);
                point.Y += (YDirection * YOffset);

                if (XDirection == 1 && point.X > 1.0)       { xDirection = -1; }
                else if (XDirection == -1 && point.X < 0.0) { xDirection = 1;  }
                else if (YDirection == 1 && point.Y > 1.0)  { yDirection = -1; }
                else if (YDirection == -1 && point.Y < 0.0) { yDirection = 1;  }
            }

            XDirection = xDirection;
            YDirection = yDirection;
        }

        private void InitAnimationDirection()
        {
            Random rand = new Random(_randSeed++);
            XDirection = (rand.Next(0, 2) == 0) ? 1 : -1;
            YDirection = (rand.Next(0, 2) == 0) ? 1 : -1;
        }
    }

    public class AllOverlaysViewModel : ViewModelBase, IDisposable
    {
        private const Int32 AnimationUpdateInterval = 67; // Approximately a 15 frame/second rate

        private System.Threading.Timer _animationTimer;
        private delegates.UpdateOverlay _updateOverlay;
        private delegates.ClearOverlays _clearOverlays;
        private delegates.RemoveOverlay _removeOverlay;

        public AllOverlaysViewModel(delegates.RemoveOverlay remove, delegates.ClearOverlays clear, delegates.UpdateOverlay update)
        {
            _clearOverlays = clear;
            _removeOverlay = remove;
            _updateOverlay = update;

            DisplayName = "Available Overlays";
            RemoveCommand = new DelegateCommand<object>(p => RemoveSelected(p));
            AnimateCommand = new DelegateCommand<IList<object>>(p => Animate(p));
            StopAnimationCommand = new DelegateCommand<IList<object>>(p => StopAnimation(p));
            RemoveAllCommand = new DelegateCommand(() => Clear());
            Overlays = new ObservableCollection<OverlayData>();

            InitializeAnimationTimer();
        }

        public void Dispose()
        {
            _animationTimer.Dispose();
        }

        public void Add(drawing.Overlay overlay, bool animate)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action((() =>
                Overlays.Add(new OverlayData(overlay) { Animated = animate })
            )));
        }

        private void RemoveSelected(object selectedItems)
        {
            if (selectedItems != null)
            {
                var list = selectedItems as IList;
                foreach (object item in list)
                {
                    OverlayData data = item as OverlayData;
                    _removeOverlay(data.ActualOverlay);
                    Dispatcher.CurrentDispatcher.BeginInvoke(new Action((() => Overlays.Remove(data))));
                }
            }
        }

        private void Animate(IList<object> selected)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                foreach (OverlayData data in selected.Cast<OverlayData>())
                {
                    data.Animated = true;
                }
            }));
        }

        private void StopAnimation(IList<object> selected)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                foreach (OverlayData data in selected.Cast<OverlayData>())
                {
                    data.Animated = false;
                }
            }));
        }

        public void Clear()
        {
            _clearOverlays();

            Dispatcher.CurrentDispatcher.BeginInvoke(new Action((() => Overlays.Clear())));
        }

        public ObservableCollection<OverlayData> Overlays { get; private set; }

        public ICommand RemoveCommand { get; private set; }

        public ICommand RemoveAllCommand { get; private set; }

        public ICommand AnimateCommand { get; private set; }

        public ICommand StopAnimationCommand { get; private set; }

        public override string DisplayName { get; set; }

        private void InitializeAnimationTimer()
        {
            _animationTimer = new System.Threading.Timer(x =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    foreach (OverlayData data in Overlays)
                    {
                        if (data.Animated)
                        {
                            data.Translate();
                            _updateOverlay(data.ActualOverlay);
                        }
                    }
                }));

            }, null, AnimationUpdateInterval, AnimationUpdateInterval);
        }
    }
}
