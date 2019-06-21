using monitor.Views;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Deployment.Application;
using System;
using System.Windows.Controls;
using monitor.Fingerprint.Views;

namespace monitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //RegisterUser page = new RegisterUser();
            //mainPage.NavigationService.Navigate(page);
            LoadMenus();
            LoadCurrentVersion();
        }

        private void LoadCurrentVersion()
        {
            string version = string.Empty;
            try
            {
                // Get deployment version.
                version = $"v{ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()}";
            }
            catch (InvalidDeploymentException)
            {
                // You cannot read publish version when app isn't installed 
                // (e.g. during debug)
                version = "debug";
            }
            Title = $"{Title} - {version}";
            lblVersion.Content = $"Monitor - {version}";
            lblDate.Content = DateTime.Now.ToLongDateString();
        }
        public void LoadMenus()
        {
            List<Item> items;
            using (StreamReader r = new StreamReader("Data\\data.json"))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Item>>(json);
            }
            foreach (var item in items)
            {
                
            }
        }

    }
    public class Item
    {
        public string title;
        public string icon;
    }
}
