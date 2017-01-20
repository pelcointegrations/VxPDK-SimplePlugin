using System.Windows.Media;
using drawing = Pelco.Phoenix.PluginHostInterfaces.Drawing;
using delegates = OverlayDrawings.Model.Utils.Delegates;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace OverlayDrawings.Model
{
    class NewTextBoxViewModel : NewRectangleViewModel
    {
        private drawing.TextWrap _selectedWrapping;
        private drawing.HorizontalAlignment _selectedHAlign;
        private drawing.VerticalAlignment _selectedVAlign;

        public NewTextBoxViewModel(delegates.DrawOverlay drawDelegate) : base(drawDelegate)
        {
            DisplayName = "TextBox Configuration";
            Text = "This is a sample message";
            FontSize = 9;
            Forground = Colors.Black;
            SelectedTextWrapping = drawing.TextWrap.Wrap;
            SelectedHAlignment = drawing.HorizontalAlignment.Center;
            SelectedVAlignment = drawing.VerticalAlignment.Center;
            DrawCommand = new DelegateCommand(() => ExecuteDraw());
        }

        #region Properties

        /// <summary>
        /// The display name of the TextBox configuration
        /// </summary>
        public override string DisplayName { get; set; }

        /// <summary>
        /// The Text message of the TextBox
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The Font size of the text
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// The forground color of the text
        /// </summary>
        public Color? Forground { get; set; }

        /// <summary>
        /// The available text wrapping options
        /// </summary>
        public drawing.TextWrap[] AvailableWrapping
        {
            get
            {
                return new drawing.TextWrap[] { drawing.TextWrap.NoWrap, drawing.TextWrap.Wrap };
            }
        }

        /// <summary>
        /// Holds the selected wrapping value.
        /// </summary>
        public drawing.TextWrap SelectedTextWrapping
        {
            get
            {
                return _selectedWrapping;
            }
            set
            {
                if (value != _selectedWrapping)
                {
                    _selectedWrapping = value;
                    OnPropertyChanged(nameof(SelectedTextWrapping));
                }
            }
        }

        /// <summary>
        /// The available horizontal alignment options
        /// </summary>
        public drawing.HorizontalAlignment[] AvailableH_Alignments
        {
            get
            {
                return new drawing.HorizontalAlignment[]
                {
                    drawing.HorizontalAlignment.Left,
                    drawing.HorizontalAlignment.Right,
                    drawing.HorizontalAlignment.Center
                };
            }
        }

        /// <summary>
        /// Holds the selected horizontal alignment value.
        /// </summary>
        public drawing.HorizontalAlignment SelectedHAlignment
        {
            get
            {
                return _selectedHAlign;
            }
            set
            {
                if (value != _selectedHAlign)
                {
                    _selectedHAlign = value;
                    OnPropertyChanged(nameof(SelectedHAlignment));
                }
            }
        }

        /// <summary>
        /// The available vertical alignment options
        /// </summary>
        public drawing.VerticalAlignment[] AvailableV_Alignments
        {
            get
            {
                return new drawing.VerticalAlignment[]
                {
                    drawing.VerticalAlignment.Top,
                    drawing.VerticalAlignment.Bottom,
                    drawing.VerticalAlignment.Center
                };
            }
        }

        /// <summary>
        /// Holds the selected vertical alignment
        /// </summary>
        public drawing.VerticalAlignment SelectedVAlignment
        {
            get
            {
                return _selectedVAlign;
            }
            set
            {
                if (_selectedVAlign != value)
                {
                    _selectedVAlign = value;
                    OnPropertyChanged(nameof(SelectedVAlignment));
                }
            }
        }

        #endregion

        #region Commands

        public override ICommand DrawCommand { get; protected set; }

        private void ExecuteDraw()
        {
            drawing.Rectangle box = new drawing.Rectangle
            {
                UpperLeft = new drawing.NormalizedPoint { X = Left, Y = Top },
                BottomRight = new drawing.NormalizedPoint { X = Right, Y = Right },
            };

            box.SetLineColor(BorderColor);
            box.SetFillColor(FillColor);

            drawing.TextBox textBox = new drawing.TextBox
            {
                Box = box,
                Text = Text,
                FontSize = FontSize,
                TextWrapping = SelectedTextWrapping,
                H_Alighnment = SelectedHAlignment,
                V_Alighnment = SelectedVAlignment,
            };

            textBox.SetTextColor(Forground.Value);

            _drawDelegate(textBox, Animate);
        }

        #endregion
    }
}
