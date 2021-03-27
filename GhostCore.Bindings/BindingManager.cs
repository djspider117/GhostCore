using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.ObjectModel
{
    public static class BindingManager
    {
        private static List<PropertyBinding> _bindings = new List<PropertyBinding>();

        public static PropertyBinding BindProperties(INotifyPropertyChanged source, string sourceProperty, object target, string targetProperty, BindingMode mode = BindingMode.OneWay)
        {
            var binding = new PropertyBinding(sourceProperty, targetProperty, source, target, mode);
            binding.Disposing += binding_Disposing;
            _bindings.Add(binding);

            Debug.WriteLine($"Added binding: {binding}");

            return binding;
        }

        public static PropertyBinding GetBinding(INotifyPropertyChanged source, string sourceProperty)
        {
            var b = _bindings.FirstOrDefault(x => x.Source == source && x.SourcePropertyName == sourceProperty);
            return b;
        }

        private static void binding_Disposing(object sender, EventArgs e)
        {
            Debug.WriteLine($"Removed and disposed binding: {sender}");
            _bindings.Remove(sender as PropertyBinding);
        }
    }
}
