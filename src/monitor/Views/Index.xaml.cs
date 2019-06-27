﻿using monitor.Data;
using System.Windows;
using System.Windows.Controls;

namespace monitor.Views
{
    /// <summary>
    /// Interaction logic for Index.xaml
    /// </summary>
    public partial class Index : Page
    {
        private UsuarioRepository _usuarioRepository;
        public Index()
        {
            InitializeComponent();
            Loaded += Index_Loaded;
        }

        private void Index_Loaded(object sender, RoutedEventArgs e)
        {
            _usuarioRepository = new UsuarioRepository();

        }
    }
}
