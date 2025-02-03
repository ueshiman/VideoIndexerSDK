using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Extensions.DependencyInjection;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VideoIndexPoc
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.d
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private Windows.ListWindow? _listWindow = null;
        public MainWindow(Windows.ListWindow listWindow)
        {
            this.InitializeComponent();
            _listWindow = listWindow;

        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            _listWindow ??= App.AppHost.Services.GetRequiredService<Windows.ListWindow>();
            _listWindow._myMainWindow = this;
            //myButton.Content = "Clicked";
            _listWindow?.Activate();
        }

        public void CloseListWindow()
        {
            _listWindow?.Close();
            _listWindow = null;
        }
    }
}
