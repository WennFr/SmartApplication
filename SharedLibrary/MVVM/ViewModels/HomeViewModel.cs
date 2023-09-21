using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ServiceApplication.MVVM.ViewModels;
using SharedLibrary.MVVM.Core;
using SharedLibrary.Services;

namespace SharedLibrary.MVVM.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        private readonly NavigationStore _navigationStore;
        private readonly DateTimeService _dateTimeService;

        public HomeViewModel(NavigationStore navigationStore, DateTimeService dateTimeService)
        {
            _navigationStore = navigationStore;
            _dateTimeService = dateTimeService;
            Task.Run(GetDateTime);
        }

        // Navigation
        public ICommand NavigateToSettingsCommand =>
            new RelayCommand(() => _navigationStore.CurrentViewModel = new SettingsViewModel(_navigationStore, _dateTimeService));




        private string? _currentTime = "00:00";
        public string? CurrentTime
        {
            get => _currentTime; set => SetValue(ref _currentTime, value);
        }


        private string? _currentDate;
        public string? CurrentDate { get => _currentDate; set => SetValue(ref _currentDate, value); }


        private void GetDateTime()
        {
            while (true)
            {
                CurrentTime = _dateTimeService.CurrentTime;
                CurrentDate = _dateTimeService.CurrentDate;
                
            }
        }


    }
}
