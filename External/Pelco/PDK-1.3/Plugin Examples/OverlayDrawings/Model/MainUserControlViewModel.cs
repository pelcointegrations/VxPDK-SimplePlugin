using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Pelco.Phoenix.PluginHostInterfaces;
using OverlayDrawings.Model.Utils;
using drawing = Pelco.Phoenix.PluginHostInterfaces.Drawing;
using Microsoft.Practices.Prism.Commands;

namespace OverlayDrawings.Model
{
    class MainUserControlViewModel : ViewModelBase, IDisposable
    {
        private string _displayName;
        private ViewModelBase _currentViewModel;
        private readonly List<ViewModelBase> _viewModels;
        private AllOverlaysViewModel _overlayListingModel;
        private IOCCHostOverlayDrawingService _drawingService;

        public MainUserControlViewModel(IOCCHostOverlayDrawingService drawingService)
        {
            Commands = new ReadOnlyCollection<CommandViewModel>(CreateCommands());

            _drawingService = drawingService;
            _viewModels = CreateViewModels(drawingService);

            ShowControl(typeof(NewLineViewModel));
        }

        public void Dispose()
        {
            if (_overlayListingModel != null)
            {
                _overlayListingModel.Dispose();
            }
        }

        public ReadOnlyCollection<CommandViewModel> Commands { get; private set; }

        public override string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; OnPropertyChanged("DisplayName"); }
        }

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }

            private set
            {
                if (value != null)
                {
                    _currentViewModel = value;
                    OnPropertyChanged("CurrentViewModel");
                    DisplayName = _currentViewModel.DisplayName;
                }
            }
        }

        private List<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel("Draw New Lines", new DelegateCommand(() => ShowControl(typeof(NewLineViewModel)))),
                new CommandViewModel("Draw New Rectangles", new DelegateCommand(() => ShowControl(typeof(NewRectangleViewModel)))),
                new CommandViewModel("Draw New Ellipses", new DelegateCommand(() => ShowControl(typeof(NewEllipseViewModel)))),
                new CommandViewModel("Draw New Polygon", new DelegateCommand(() => ShowControl(typeof(NewPolygonViewModel)))),
                //new CommandViewModel("Draw New TextBox", new DelegateCommand(() => ShowControl(typeof(NewTextBoxViewModel)))),
                new CommandViewModel("List All Overlays", new DelegateCommand(() => ShowOverlayListingControl()))
            };
        }

        private List<ViewModelBase> CreateViewModels(IOCCHostOverlayDrawingService service)
        {
            _overlayListingModel = new AllOverlaysViewModel(this.ExecuteRemoveOverlay, this.ExecuteClearOverlays, this.ExecuteUpdateOverlay);

            return new List<ViewModelBase>
            {
                new NewLineViewModel(this.ExecuteDrawOverlay),
                new NewRectangleViewModel(this.ExecuteDrawOverlay),
                new NewEllipseViewModel(this.ExecuteDrawOverlay),
                new NewPolygonViewModel(this.ExecuteDrawOverlay),
                //new NewTextBoxViewModel(this.ExecuteDrawOverlay),
                _overlayListingModel
            };
        }

        private void ExecuteDrawOverlay(drawing.Overlay overlay, bool animate)
        {
            if (_drawingService != null)
            {
                Task.Run(() => _drawingService.Draw(overlay));
                _overlayListingModel.Add(overlay, animate);
            }
            else
            {
                UI.ShowErrorDialog("IOCCHostOverlayDrawingService is not available, unable to draw overlay");
            }
        }

        private void ExecuteUpdateOverlay(drawing.Overlay overlay)
        {
            if (_drawingService != null)
            {
                Task.Run(() => _drawingService.Update(overlay));
            }
            else
            {
                UI.ShowErrorDialog("IOCCHostOverlayDrawingService is not available, unable to update overlay");
            }
        }

        private void ExecuteRemoveOverlay(drawing.Overlay overlay)
        {
            if (_drawingService != null)
            {
                Task.Run(() => _drawingService.RemoveOverlay(overlay.ID));
            }
            else
            {
                UI.ShowErrorDialog("IOCCHostOverlayDrawingService is not available, unable to remove overlay");
            }
        }

        private void ExecuteClearOverlays()
        {
            if (_drawingService != null)
            {
                Task.Run(() => _drawingService.ClearOverlays());
            }
            else
            {
                UI.ShowErrorDialog("IOCCHostOverlayDrawingService is not available, unable to clear overlays");
            }
        }

        private void ShowControl(Type type)
        {
            CurrentViewModel = _viewModels.Find(v => v.GetType() == type);
        }

        private void ShowOverlayListingControl()
        {
            CurrentViewModel = _overlayListingModel;
        }
    }  
}
