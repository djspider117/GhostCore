using GhostCore.Animations.Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace GhostCore.Animations.Editor.Converters
{
    public class ColorGroupToSCBConverter : IValueConverter
    {
        public SolidColorBrush RedBrush { get; set; } = new SolidColorBrush(Color.FromArgb(255, 181, 56, 56));
        public SolidColorBrush GreenBrush { get; set; } = new SolidColorBrush(Color.FromArgb(255, 74, 164, 76));
        public SolidColorBrush BlueBrush { get; set; } = new SolidColorBrush(Color.FromArgb(255, 103, 125, 224));
        public SolidColorBrush VioletBrush { get; set; } = new SolidColorBrush(Color.FromArgb(255, 142, 44, 154));
        public SolidColorBrush YellowBrush { get; set; } = new SolidColorBrush(Color.FromArgb(255, 228, 216, 76));
        public SolidColorBrush OrangeBrush { get; set; } = new SolidColorBrush(Color.FromArgb(255, 232, 146, 13));
        public SolidColorBrush WhiteBrush { get; set; } = new SolidColorBrush(Color.FromArgb(255, 234, 234, 234));
        public SolidColorBrush GrayBrush { get; set; } = new SolidColorBrush(Color.FromArgb(255, 165, 165, 165));
        public SolidColorBrush BrownBrush { get; set; } = new SolidColorBrush(Color.FromArgb(255, 168, 150, 119));

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is ColorGroup cg)
            {
                switch (cg)
                {
                    case ColorGroup.Red: return RedBrush;
                    case ColorGroup.Green: return GreenBrush;
                    case ColorGroup.Blue: return BlueBrush;
                    case ColorGroup.Violet: return VioletBrush;
                    case ColorGroup.Yellow: return YellowBrush;
                    case ColorGroup.Orange: return OrangeBrush;
                    case ColorGroup.White: return WhiteBrush;
                    case ColorGroup.Gray: return GrayBrush;
                    case ColorGroup.Brown: return BrownBrush;
                    default: return new SolidColorBrush(Colors.DarkMagenta);
                }
            }

            return new SolidColorBrush(Colors.DarkMagenta);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
