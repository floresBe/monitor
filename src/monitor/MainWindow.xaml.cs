using monitor.Views;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Deployment.Application;
using System;
using System.Windows.Controls;
using monitor.Fingerprint.Views;
using monitor.Fingerprint.Views.UsuariosView;
using System.Linq; 
using monitor.Views.HomeView;
using monitor.Views.UsuariosView;
using monitor.Views.ModelosView;

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
            lblVersion.Content = $"{version}";
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

            menuItems.ItemsSource = items.Where(w => w.Nivel >= App.usuario.TipoEmpleado);
            menuItems.SelectedItem = items.FirstOrDefault();
        }

        private void MenuItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Item itemSelected = (Item) menuItems.SelectedItem;
            lblTitle.Content = itemSelected.Title;

            switch (itemSelected.Title)
            {
                case "Inicio":
                    if (App.isRunning)
                    { 
                        mainPage.NavigationService.Navigate(App.stopModelPage);
                        break;
                    }

                    StartModel page = new StartModel();
                    mainPage.NavigationService.Navigate(page);
                    break;
                case "Estaciones":
                    //RegisterUser page = new RegisterUser();
                    //mainPage.NavigationService.Navigate(page);
                    break;
                case "Reportes":
                    //RegisterUser page = new RegisterUser();
                    //mainPage.NavigationService.Navigate(page);
                    break;
                case "Modelos": 
                    IndexModel Modelpage = new IndexModel();
                    mainPage.NavigationService.Navigate(Modelpage);
                    break;
                case "Usuarios":
                    IndexUser pageUsers = new IndexUser();
                    mainPage.NavigationService.Navigate(pageUsers);
                    break;
                case "Salir":
                    Login pageLogin = new Login();
                    Application.Current.MainWindow = pageLogin;
                    Close();
                    pageLogin.Show(); 
                    break;
                default:
                    break;
            }

            
        }
    }
    public class Item
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public int Nivel { get; set; }
    }
}
