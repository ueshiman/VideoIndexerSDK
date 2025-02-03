using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Extensions.Logging;
using VideoIndexPoc2.Windows;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VideoIndexPoc2
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly ILogger<MainWindow> _logger;
        private Windows.ListWindow? _listWindow = null;
        public MainWindow(ILogger<MainWindow> logger, ListWindow listWindow)
        {
            this.InitializeComponent();
            _listWindow = listWindow;
            _logger = logger;
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _listWindow ??= App.AppHost.Services.GetRequiredService<ListWindow>();
                _listWindow.MyMainWindow = this;
                //myButton.Content = "Clicked";
                _listWindow?.Activate();
            }
            catch (Exception exception)
            {
                //Console.WriteLine(exception);
                _logger.LogError(exception, "Error occurred in myButton_Click");
                /*throw*/;
            }

        }

        public void CloseListWindow()
        {
            _listWindow?.Close();
            _listWindow = null;
        }
    }
}
