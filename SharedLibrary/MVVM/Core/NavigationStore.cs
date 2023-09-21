using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.MVVM.Core
{
    public class NavigationStore : ObservableObject
    {
        private ObservableObject? _currentViewModel;

        public ObservableObject? CurrentViewModel
        {
            get => _currentViewModel; 
            set => SetValue(ref _currentViewModel, value);
        }

        public void NavigateTo(ObservableObject? viewModel)
        {
            CurrentViewModel = viewModel;
        }
    }
}
