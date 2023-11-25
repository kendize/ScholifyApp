// <copyright file="PupilWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;
    using WPFScholifyApp.BLL;
    using WPFScholifyApp.DAL.ClassRepository;
    using WPFScholifyApp.DAL.DBClasses;

    /// <summary>
    /// Interaction logic for PupilWindow.xaml.
    /// </summary>
    public partial class PupilWindow : Window
    {
        private AdminService adminService;
        private UserService userService;
        private AdvertisementService advertisementService;
        public int selectedClassId;
        
        public PupilWindow()
        {
            this.adminService = new AdminService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Advertisement>());
            this.userService = new UserService(new GenericRepository<User>(), new GenericRepository<Pupil>());
            this.advertisementService = new AdvertisementService(new GenericRepository<Advertisement>(), new GenericRepository<Class>());
            this.InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }

        private void PrivateInfoButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteFromPupilsPanel();

            TextBlock titleLabel = new TextBlock
            {
                Text = "Приватна інформація",
                FontSize = 50,
                Foreground = new SolidColorBrush(Colors.DarkBlue),
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(430, 30, 0, 10),
            };
            this.Panel.Children.Add(titleLabel);

            UserService userService = new UserService(new GenericRepository<User>(), new GenericRepository<Pupil>());
            string name = this.FirstNameTextBlock.Text;
            string surname = this.LastNameTextBlock.Text;
            User pupil = userService.GetInfoByNameSurname(name, surname);

            if (pupil != null)
            {
                TextBlock studentInfo = new TextBlock
                {
                    Text = $"Ім'я:\t\t {pupil.FirstName}\n\nПрізвище:\t {pupil.LastName}\n\nПо батькові:\t {pupil.MiddleName}\n\nСтать:\t\t {pupil.Gender}" +
                    $"\n\nДата народження: {pupil.Birthday:dd.MM.yyyy}\n\nАдреса:\t\t {pupil.Address}\n\nТелефон:\t {pupil.PhoneNumber}",
                    FontSize = 40,
                    Foreground = new SolidColorBrush(Colors.DarkBlue),
                    Margin = new Thickness(430, 90, 0, 10),
                };
                this.Panel.Children.Add(studentInfo);
            }
        }

        private void JournalButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteFromPupilsPanel();
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteFromPupilsPanel();
        }

        private void AnnouncementsButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteFromPupilsPanel();
            ////AdvertisementService advertisementService = new AdvertisementService(new GenericRepository<Advertisement>(), new GenericRepository<Class>());
            ////var advertisement = this.advertisementService.GetAllAdvertisementsForClassId(this.selectedClassId).FirstOrDefault(x => x.Id == (int).Tag);
            ////if (advertisement != null)
            ////{
            ////    TextBlock advertisementInfo = new TextBlock
            ////    {

            ////        Text = $"Тема:\t {advertisement.Name}\n\n Вміст:\t {advertisement.Description}",
            ////        FontSize = 40,
            ////        Foreground = new SolidColorBrush(Colors.DarkBlue),
            ////        Margin = new Thickness(180, 90, 0, 10),
            ////    };
            ////    this.Panel.Children.Add(advertisementInfo);
            ////}

            UpdatePupilsPanel();

        }

        public void DeleteFromPupilsPanel()
        {
            this.Panel.Children.Clear();
        }

        public void UpdatePupilsPanel()
        {
            this.Panel.UpdateLayout();
        }
    }
}

