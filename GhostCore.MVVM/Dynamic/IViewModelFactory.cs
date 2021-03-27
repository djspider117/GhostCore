using System.Text;

namespace GhostCore.MVVM.Dynamic
{
    public interface IViewModelFactory<TViewModel, TModel> where TViewModel : ViewModelBase<TModel>
    {
        TViewModel Create(TModel parameter);
    }
}
