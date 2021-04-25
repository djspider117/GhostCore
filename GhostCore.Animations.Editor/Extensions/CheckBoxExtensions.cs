using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace GhostCore.Animations.Editor.Extensions
{
    [Bindable]
    public static class CheckBoxExtensions
    {
        public static string GetCheckGlyph(DependencyObject obj)
        {
            return (string)obj.GetValue(CheckGlyphProperty);
        }

        public static void SetCheckGlyph(DependencyObject obj, string value)
        {
            obj.SetValue(CheckGlyphProperty, value);
        }

        public static readonly DependencyProperty CheckGlyphProperty =
            DependencyProperty.RegisterAttached("CheckGlyph", typeof(string), typeof(CheckBoxExtensions), new PropertyMetadata("&#xE001;"));
    }
}
