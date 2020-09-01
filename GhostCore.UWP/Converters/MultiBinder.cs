using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters
{
    public class MultiBinder : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty SourcesProperty =
            DependencyProperty.Register("Sources", typeof(List<MultiBindingSource>), typeof(MultiBinder), new PropertyMetadata(new List<MultiBindingSource>()));

        public static readonly DependencyProperty EvaluatorProperty =
            DependencyProperty.Register("Evaluator", typeof(MultiValueEvaluator), typeof(MultiBinder), new PropertyMetadata(null));


        public List<MultiBindingSource> Sources
        {
            get { return (List<MultiBindingSource>)GetValue(SourcesProperty); }
            private set { SetValue(SourcesProperty, value); }
        }

        public MultiValueEvaluator Evaluator
        {
            get { return (MultiValueEvaluator)GetValue(EvaluatorProperty); }
            set { SetValue(EvaluatorProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (Evaluator == null)
            {
                Debug.WriteLine("MultiBinder has a null MultiBinder. Value passed forward as is.");
                return value;
            }

            return Evaluator.Evaluate(Sources, value, targetType, parameter, language);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }

    public abstract class MultiValueEvaluator : DependencyObject
    {
        public abstract object Evaluate(IList<MultiBindingSource> sources, object contextValue, Type targetType, object parameter, string lang);
    }

    public class PassthroughEvaluator : MultiValueEvaluator
    {
        public override object Evaluate(IList<MultiBindingSource> sources, object contextValue, Type targetType, object parameter, string lang)
        {
            return contextValue;
        }
    }

    public class MultiBindingSource : DependencyObject
    {
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(MultiBindingSource), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty SourceValueProperty =
            DependencyProperty.Register("SourceValue", typeof(object), typeof(MultiBindingSource), new PropertyMetadata(null));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public object SourceValue
        {
            get { return GetValue(SourceValueProperty); }
            set { SetValue(SourceValueProperty, value); }
        }
    }
}
