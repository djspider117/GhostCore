using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace GhostCore.UWP.TemplateSelectors
{

    public class DataTemplateEntry
    {
        public string For { get; set; }
        public DataTemplate Template { get; set; }
    }
    public class DataTemplateCollection : List<DataTemplate> { }
    public class DataTemplateEntryCollection : List<DataTemplateEntry> { }

    [ContentProperty(Name = nameof(Templates))]
    public class XamlTemplateSelector : DataTemplateSelector
    {
        protected DataTemplateEntryCollection _referencedTemplates;
        protected DataTemplateCollection _templates;
        protected Dictionary<string, DataTemplate> _dataTemplateMapping;

        public bool ThrowExceptionOnDuplicateTypes { get; set; } = true;
        public bool ThrowExceptionOnUnmappedType { get; set; } = false;

        public virtual DataTemplateCollection Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                InitializeTemplateMapping();
            }
        }

        public DataTemplateEntryCollection ReferencedTemplates
        {
            get { return _referencedTemplates; }
            set
            {
                _referencedTemplates = value;
                InitializeTemplateMapping();
            }
        }

        public DataTemplate NullTemplate { get; set; }

        public XamlTemplateSelector()
        {
            _templates = new DataTemplateCollection();
            _referencedTemplates = new DataTemplateEntryCollection();
        }

        protected virtual void InitializeTemplateMapping()
        {
            if (_templates == null)
            {
                _dataTemplateMapping?.Clear();
                _dataTemplateMapping = null;
                return;
            }

            _dataTemplateMapping = new Dictionary<string, DataTemplate>();
            foreach (var template in _templates)
            {
                var type = XamlTypeHelper.GetTargetType(template);
                if (_dataTemplateMapping.ContainsKey(type) && ThrowExceptionOnDuplicateTypes)
                    throw new InvalidOperationException("[AdvancedTemplateSelector] Unable to add data template. Duplicate type detected.");

                _dataTemplateMapping.Add(type, template);
            }


            foreach (var templateReference in _referencedTemplates)
            {
                if (_dataTemplateMapping.ContainsKey(templateReference.For))
                    continue;

                _dataTemplateMapping.Add(templateReference.For, templateReference.Template);
            }
        }
        protected virtual DataTemplate InternalSelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return NullTemplate;

            if (_dataTemplateMapping.TryGetValue(item.GetType().Name, out DataTemplate rv))
                return rv;

            return null;
        }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return base.SelectTemplateCore(item);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (_dataTemplateMapping == null)
                InitializeTemplateMapping();

            var template = InternalSelectTemplate(item, container);
            if (template != null)
                return template;

            if (ThrowExceptionOnUnmappedType)
                throw new InvalidOperationException("[AdvancedTemplateSelector] Unmapped type detected on SelectTemplateCore.");
            else
                return base.SelectTemplateCore(item, container);
        }

    }

    public class XamlTypeHelper : DependencyObject
    {
        public static readonly DependencyProperty TargetTypeProperty =
            DependencyProperty.RegisterAttached("TargetType", typeof(string), typeof(XamlTypeHelper), new PropertyMetadata(null));

        public static void SetTargetType(DataTemplate element, string value)
        {
            element.SetValue(TargetTypeProperty, value);
        }
        public static string GetTargetType(DataTemplate element)
        {
            return (string)element.GetValue(TargetTypeProperty);
        }
    }

}
