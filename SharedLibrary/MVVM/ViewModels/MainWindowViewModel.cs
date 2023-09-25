using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.MVVM.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;

        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            CurrentViewModel = _serviceProvider.GetRequiredService<HomeViewModel>();
        }


        [ObservableProperty]
        private ObservableObject? _currentViewModel;

    }
}
