using Microsoft.Azure.Devices.Shared;
using SharedLibrary.Handlers.Services;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharedLibrary.MVVM.Models;
using Microsoft.Azure.Devices;
using SharedLibrary.MVVM.Core;

namespace Control_Panel
{
    public partial class MainWindow : Window
    {
        public MainWindow(IotHubManager iotHub, NavigationStore navigationStore)
        {
            InitializeComponent();
            DataContext = navigationStore;

        }

        
        //private async void StartButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        Button? button = sender as Button;

        //        if (button != null)
        //        {
        //            Twin? twin = button.DataContext as Twin;

        //            if (twin != null)
        //            {
        //                string deviceId = twin.DeviceId;


        //                if (!string.IsNullOrEmpty(deviceId))
        //                    await _iotHub.SendMethodAsync(new MethodDataRequest
        //                    {
        //                        DeviceId = deviceId,
        //                        MethodName = "start"
        //                    });
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }


        //}

        //private async void StopButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        Button? button = sender as Button;

        //        if (button != null)
        //        {
        //            Twin? twin = button.DataContext as Twin;

        //            if (twin != null)
        //            {
        //                string deviceId = twin.DeviceId;


        //                if (!string.IsNullOrEmpty(deviceId))
        //                    await _iotHub.SendMethodAsync(new MethodDataRequest
        //                    {
        //                        DeviceId = deviceId,
        //                        MethodName = "stop"
        //                    });

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }
        //}

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }




    }
}
