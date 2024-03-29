﻿using monitor.Views;
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
using monitor.Views.EstacionesView;
using System.ComponentModel;

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
            Item itemSelected = (Item)menuItems.SelectedItem;
            lblTitle.Content = itemSelected.Title;

            switch (itemSelected.Title)
            {
                case "Inicio":
                    ModelsView StartModePage = new ModelsView();
                    mainPage.NavigationService.Navigate(StartModePage);
                    break;
                case "Estaciones":
                    IndexEstacion EstacionesPage = new IndexEstacion();
                    mainPage.NavigationService.Navigate(EstacionesPage);
                    break;
                case "Reportes":
                    Reports.ReportsMenu reportPage = new Reports.ReportsMenu();
                    reportPage.Show();
                    WindowState = WindowState.Minimized;
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

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result =
                 MessageBox.Show("Al salir se cerraran todas las estaciones. ¿Continuar?", "Cerrar", MessageBoxButton.YesNo,MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                // If user doesn't want to close, cancel closure
                e.Cancel = true;
            }

            foreach (var estacion in App.estacionesWindows)
            {
                estacion.Close();
            }            
        }
    }
    public class Item
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public int Nivel { get; set; }
        public string Id { get; set; }
    }
}
