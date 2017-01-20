using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UI.Components
{
    /// <summary>
    /// A standard button that contains two DependencyProperties that hold the normal image and the hover image which can then be binded to in styles and triggers.
    /// </summary>
    public class HoverImageButton : Button
    {
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        #region NormalImage
        public static readonly DependencyProperty NormalImageProperty = DependencyProperty.Register("NormalImage",
            typeof(ImageSource), typeof(HoverImageButton), new FrameworkPropertyMetadata(null));

        public ImageSource NormalImage
        {
            get { return (ImageSource)GetValue(NormalImageProperty); }
            set { SetValue(NormalImageProperty, value); }
        }
        #endregion

        #region HoverImage
        public static readonly DependencyProperty HoverImageProperty = DependencyProperty.Register("HoverImage",
            typeof(ImageSource), typeof(HoverImageButton), new FrameworkPropertyMetadata(null));

        public ImageSource HoverImage
        {
            get { return (ImageSource)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }
        #endregion

        #region DisabledImage
        public static readonly DependencyProperty DisabledImageProperty = DependencyProperty.Register("DisabledImage",
            typeof(ImageSource), typeof(HoverImageButton), new FrameworkPropertyMetadata(null));

        public ImageSource DisabledImage
        {
            get { return (ImageSource)GetValue(DisabledImageProperty); }
            set { SetValue(DisabledImageProperty, value); }
        }
        #endregion
    }
}
