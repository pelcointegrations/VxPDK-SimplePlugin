using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PluginNs.Controls
{
    public class SeButton : Button
    {
        #region HoverBackground
        public static readonly DependencyProperty HoverBackgroundProperty = DependencyProperty.Register("HoverBackground",
            typeof(Brush), typeof(SeButton), new FrameworkPropertyMetadata(Brushes.Transparent));

        public Brush HoverBackground
        {
            get { return (Brush)GetValue(HoverBackgroundProperty); }
            set { SetValue(HoverBackgroundProperty, value); }
        }
        #endregion

        #region HoverBorderBrush
        public static readonly DependencyProperty HoverBorderBrushProperty = DependencyProperty.Register("HoverBorderBrush",
            typeof(Brush), typeof(SeButton), new FrameworkPropertyMetadata(Brushes.Transparent));

        public Brush HoverBorderBrush
        {
            get { return (Brush)GetValue(HoverBorderBrushProperty); }
            set { SetValue(HoverBorderBrushProperty, value); }
        }
        #endregion

        #region PressedBackground
        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register("PressedBackground",
            typeof(Brush), typeof(SeButton), new FrameworkPropertyMetadata(Brushes.Transparent));

        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }
        #endregion

        #region PressedBorderBrush
        public static readonly DependencyProperty PressedBorderBrushProperty = DependencyProperty.Register("PressedBorderBrush",
            typeof(Brush), typeof(SeButton), new FrameworkPropertyMetadata(Brushes.Transparent));

        public Brush PressedBorderBrush
        {
            get { return (Brush)GetValue(PressedBorderBrushProperty); }
            set { SetValue(PressedBorderBrushProperty, value); }
        }
        #endregion

        #region DisabledBackground
        public static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.Register("DisabledBackground",
            typeof(Brush), typeof(SeButton), new FrameworkPropertyMetadata(Brushes.Transparent));

        public Brush DisabledBackground
        {
            get { return (Brush)GetValue(DisabledBackgroundProperty); }
            set { SetValue(DisabledBackgroundProperty, value); }
        }
        #endregion

        #region DisabledBorderBrush
        public static readonly DependencyProperty DisabledBorderBrushProperty = DependencyProperty.Register("DisabledBorderBrush",
            typeof(Brush), typeof(SeButton), new FrameworkPropertyMetadata(Brushes.Transparent));

        public Brush DisabledBorderBrush
        {
            get { return (Brush)GetValue(DisabledBorderBrushProperty); }
            set { SetValue(DisabledBorderBrushProperty, value); }
        }
        #endregion

        #region DisabledForeground
        public static readonly DependencyProperty DisabledForegroundProperty = DependencyProperty.Register("DisabledForeground",
            typeof(Brush), typeof(SeButton), new FrameworkPropertyMetadata(Brushes.Transparent));

        public Brush DisabledForeground
        {
            get { return (Brush)GetValue(DisabledForegroundProperty); }
            set { SetValue(DisabledForegroundProperty, value); }
        }
        #endregion
    }
}
