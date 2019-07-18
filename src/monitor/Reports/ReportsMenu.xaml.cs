using monitor.Views.ReportsView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace monitor.Reports
{
    /// <summary>
    /// Interaction logic for ReportsMenu.xaml
    /// </summary>
    public partial class ReportsMenu : Window
    {
        public ReportsMenu()
        {
            InitializeComponent();
            Loaded += ReportsMenu_Loaded;
        }

        private void ReportsMenu_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMenus();
            lblTitle.Content = "Reprtes";
            menuItems.SelectionChanged += menuItems_SelectionChanged;
        }

        public void LoadMenus()
        {
            List<Item> items;
            using (StreamReader r = new StreamReader("Data\\reports.json"))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Item>>(json);
            }

            menuItems.ItemsSource = items;
            //menuItems.SelectedItem = items.FirstOrDefault();
        }


        private void menuItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Item itemSelected = (Item)menuItems.SelectedItem;

            switch (itemSelected.Title.ToLower())
            {

                case "por modelo":
                    PiezasPorModeloView piezasPorModeloPage = new PiezasPorModeloView();
                    mainPage.NavigationService.Navigate(piezasPorModeloPage);
                    break;
                case "salir":
                    Close();
                    break;
                default:
                    break;
            }

        }
    }
}
