using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace GhostCore.ObjectModel
{
    public enum BindingMode
    {
        //from source to target
        OneWay = 0x01,

        OneTime = 0x02,

        //from source to target AND from target to source
        TwoWay = 0x03,

        //from target to source
        OneWayToSource = 0x04,
        None = 0
    }

    public class PropertyBinding : IDisposable, IDisposing
    {
        #region Events

        #region Disposing

        public event EventHandler Disposing;

        protected void OnDisposing()
        {
            if (Disposing == null)
                return;

            Disposing(this, EventArgs.Empty);
        }

        #endregion

        #endregion

        #region Fields

        private INotifyPropertyChanged _source;
        private object _target;
        private bool _ignore;
        private bool _isActive;

        #endregion

        #region Properties

        public string SourcePropertyName { get; set; }
        public string TargetPropertyName { get; set; }
        public BindingMode Mode { get; private set; }

        public INotifyPropertyChanged Source
        {
            get { return _source; }
            set
            {
                if (_source != null)
                    _source.PropertyChanged -= source_PropertyChanged;
                _source = value;
                _source.PropertyChanged += source_PropertyChanged;
            }
        }

        public IValueConverter Converter { get; set; }
        public object ConverterParameter { get; set; }

        public object Target
        {
            get { return _target; }
            set
            {
                if (Mode == BindingMode.OneWayToSource || Mode == BindingMode.TwoWay)
                {
                    if (_target != null)
                    {
                        if (!_target.GetType().GetInterfaces().Contains(typeof(INotifyPropertyChanged)))
                            throw new InvalidOperationException("Cannot bind target to source because it doesn't implement INotifiyPropertyChanged");

                        (_target as INotifyPropertyChanged).PropertyChanged -= target_PropertyChanged;
                    }
                }
                _target = value;

                if (Mode == BindingMode.OneWayToSource || Mode == BindingMode.TwoWay)
                    (_target as INotifyPropertyChanged).PropertyChanged += target_PropertyChanged;
            }
        }

        #endregion

        #region Constructors and initialization

        internal PropertyBinding(BindingMode mode)
        {
            Mode = mode;
        }

        internal PropertyBinding(string sourcePropName, string targetPropName, INotifyPropertyChanged source, object target, BindingMode mode)
        {
            if (mode == BindingMode.OneWayToSource || mode == BindingMode.TwoWay)
            {
                if (!target.GetType().GetInterfaces().Contains(typeof(INotifyPropertyChanged)))
                    throw new InvalidOperationException("Cannot bind target to source because it doesn't implement INotifiyPropertyChanged");
            }

            Mode = mode;
            SourcePropertyName = sourcePropName;
            TargetPropertyName = targetPropName;
            Source = source;
            Target = target;

            Activate();
        }

        #endregion

        #region Handlers

        private void source_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_isActive)
                return;

            if (e.PropertyName == SourcePropertyName)
            {
                if (_ignore)
                {
                    _ignore = false;
                    return;
                }

                var stype = sender.GetType();
                var ttype = Target.GetType();
                var value = stype.GetProperty(SourcePropertyName).GetValue(sender);
                object convertedValue = value;
                if (Converter != null)
                {
                    convertedValue = Converter.Convert(value, ttype.GetProperty(TargetPropertyName).PropertyType, ConverterParameter, CultureInfo.CurrentCulture.ToString());
                }
                _ignore = true;

                try
                {
                    ttype.GetProperty(TargetPropertyName).SetValue(Target, convertedValue);
                }
                catch (Exception ex)
                {
                    string format = "[Binding Error] Source: {3}, Target: {4}, SourcePropertyName: {1}, TargetPropertyName: {2}, Exception: {0}";
                    Debug.WriteLine(string.Format(format, ex.Message, SourcePropertyName, TargetPropertyName, Source, Target));
                }

            }
        }

        private void target_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_isActive)
                return;

            if (e.PropertyName == TargetPropertyName)
            {
                if (_ignore)
                {
                    _ignore = false;
                    return;
                }

                var stype = Source.GetType();
                var ttype = Target.GetType();
                var value = ttype.GetProperty(TargetPropertyName)?.GetValue(sender);
                stype.GetProperty(SourcePropertyName)?.SetValue(Source, value);

                _ignore = true;
            }
        }

        #endregion

        public void Activate()
        {
            _isActive = true;
        }

        public void Deactivate()
        {
            _isActive = false;
        }

        public void TriggerSource(object sender)
        {
            source_PropertyChanged(sender, new PropertyChangedEventArgs(SourcePropertyName));
        }

        public void TriggerTarget(object sender)
        {
            target_PropertyChanged(sender, new PropertyChangedEventArgs(TargetPropertyName));
        }

        /// <summary>
        /// Deactivates the binding and clears its handlers, nulls its properties
        /// </summary>
        public void Dispose()
        {
            OnDisposing();
            Deactivate();

            if (_target != null)
                if (Mode == BindingMode.OneWayToSource || Mode == BindingMode.TwoWay)
                    (_target as INotifyPropertyChanged).PropertyChanged -= target_PropertyChanged;

            if (_source != null)
                _source.PropertyChanged -= source_PropertyChanged;

            _target = null;
            _source = null;
        }

        public override string ToString()
        {
            return string.Format("[Binding Source={0}, SourceProperty={1}, Target={2}, TargetProperty={3}, Mode={4}]", _source, SourcePropertyName, _target, TargetPropertyName, Mode);
        }
    }
}
