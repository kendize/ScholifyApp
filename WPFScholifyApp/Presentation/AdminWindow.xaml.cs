// <copyright file="AdminWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp
{
    using System;
    using System.Collections.Generic;
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

    /// <summary>
    /// Interaction logic for AdminWindow.xaml.
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            this.InitializeComponent();
        }

        private void ClassButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TeachersButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ParentsButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ChatButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }
    }
}
