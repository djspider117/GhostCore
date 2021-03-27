using GhostCore.Utility;
using System;
using System.Collections.Generic;

namespace GhostCore.MVVM.Dynamic
{
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

}
