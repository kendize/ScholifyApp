// <copyright file="ParentsWindow.xaml.cs" company="PlaceholderCompany">
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
    using WPFScholifyApp.BLL;
    using WPFScholifyApp.DAL.ClassRepository;
    using WPFScholifyApp.DAL.DBClasses;

    /// <summary>
    /// Interaction logic for ParentsWindow.xaml.
    /// </summary>
    public partial class ParentsWindow : Window
    {
        public ParentsWindow()
        {
            this.InitializeComponent();
        }

        private void JournalButton_Click(object sender, RoutedEventArgs e)
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

        private void PrivateInfoButton_Click(object sender, RoutedEventArgs e)
        {
            this.InfoPanel.Children.Clear();

            TextBlock titleLabel = new TextBlock
            {
                Text = "Приватна інформація",
                FontSize = 50,
                Foreground = new SolidColorBrush(Colors.DarkBlue),
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(410, 30, 0, 10),
            };
            this.InfoPanel.Children.Add(titleLabel);

            UserService userService = new UserService(new GenericRepository<User>());
            string name = this.FirstNameTextBlock.Text;
            string surname = this.LastNameTextBlock.Text;
            User parents = userService.GetInfoByNameSurname(name, surname);

            if (parents != null)
            {
                TextBlock studentInfo = new TextBlock
                {
                    Text = $"Ім'я:\t\t {parents.FirstName}\n\nПрізвище:\t {parents.LastName}\n\nПо батькові:\t {parents.MiddleName}\n\nСтать:\t\t {parents.Gender}" +
                    $"\n\nДата народження: {parents.Birthday:dd.MM.yyyy}\n\nАдреса:\t\t {parents.Address}\n\nТелефон:\t {parents.PhoneNumber}",
                    FontSize = 40,
                    Foreground = new SolidColorBrush(Colors.DarkBlue),
                    Margin = new Thickness(430, 90, 0, 10),
                };
                this.InfoPanel.Children.Add(studentInfo);
            }
        }
    }
}
