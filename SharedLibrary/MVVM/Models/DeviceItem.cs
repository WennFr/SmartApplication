using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharedLibrary.MVVM.Models
{
    public class DeviceItem : INotifyPropertyChanged
    {

        public DeviceItem()
        {
        }


        public string? DeviceId { get; set; }
        public string? DeviceType { get; set; }
        public string? Placement { get; set; }
        private bool _isActive;

        public bool IsActive 
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Icon => SetIcon();


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string SetIcon()
        {
            switch (DeviceType?.ToLower())
            {
                case "lamp":
                    return "\uf0eb";

                case "fan":
                    return "\ue004";

                default:
                    return "\uf2db";
            }

        }


    }
}
