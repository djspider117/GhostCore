namespace GhostCore.MVVM.Dynamic
{
    public class ReflectionViewModelFactory<TViewModel, TModel> : ReflectionViewModelFactory<TViewModel, TModel, ViewModelForAttribute> where TViewModel : ViewModelBase<TModel>
    {
    }

}
