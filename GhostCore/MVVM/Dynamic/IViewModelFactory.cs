using GhostCore.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace GhostCore.MVVM.Dynamic
{

    public interface IViewModelFactory<TViewModel, TModel> where TViewModel : ViewModelBase<TModel>
    {
        TViewModel Create(TModel parameter);
    }

    public class ReflectionViewModelFactory<TViewModel, TModel, TAttribute> : IViewModelFactory<TViewModel, TModel>
        where TViewModel : ViewModelBase<TModel>
        where TAttribute : ViewModelForAttribute
    {
        private Dictionary<Type, Func<TModel, TViewModel>> _typeFactory;
        private volatile bool _isInitialized;

        public void Initialize()
        {
            _isInitialized = true;
            _typeFactory = new Dictionary<Type, Func<TModel, TViewModel>>();

            var typeAttributeMapping = AssemblyReflectionParser.GetTypesForAttributes<TAttribute>();

            foreach (var pair in typeAttributeMapping)
            {
                foreach (var attribute in pair.Attributes)
                {
                    var ctor = pair.Type.GetConstructor(new Type[] { attribute.TargetType });
                    _typeFactory.Add(attribute.TargetType, (model) => (TViewModel)ctor.Invoke(new object[] { model }));
                }
            }
        }

        public TViewModel Create(TModel model)
        {
            if (!_isInitialized)
                Initialize();

            var actionType = model.GetType();
            return _typeFactory[actionType](model);
        }
    }

    public class ReflectionViewModelFactory<TViewModel, TModel> : ReflectionViewModelFactory<TViewModel, TModel, ViewModelForAttribute> where TViewModel : ViewModelBase<TModel>
    {
    }

   [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class ViewModelForAttribute : Attribute
    {
        public Type TargetType { get; set; }

        public ViewModelForAttribute(Type targetType)
        {
            TargetType = targetType;
        }

    }

}
