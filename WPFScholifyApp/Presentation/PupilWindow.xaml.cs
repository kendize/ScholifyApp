// <copyright file="PupilWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp
{
    using Microsoft.EntityFrameworkCore;
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
        private IGenericRepository<Pupil> pupilRepository;
        public User CurrentUser { get; set; }
        
        public PupilWindow(User currentUser, IGenericRepository<Pupil> pupilRepository)
        {
            this.adminService = new AdminService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Advertisement>());
            this.userService = new UserService(new GenericRepository<User>(), new GenericRepository<Pupil>());
            this.advertisementService = new AdvertisementService(new GenericRepository<Advertisement>(), new GenericRepository<Class>(), new GenericRepository<Pupil>());
            this.InitializeComponent();
            this.CurrentUser = currentUser;
            this.pupilRepository = pupilRepository;
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
                Margin = new Thickness(180, 30, 0, 10),
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
                    Margin = new Thickness(180, 90, 0, 10),
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
            var classId = this.pupilRepository.GetAllq().Include(x => x.Class).FirstOrDefault(x => x.Id == CurrentUser.Id)!.ClassId;
            AdvertisementService advertisementService = new AdvertisementService(new GenericRepository<Advertisement>(), new GenericRepository<Class>(), new GenericRepository<Pupil>());
            var advertisements = this.advertisementService.GetAdvertisementsForClassId(classId);
            foreach (var advertisement in advertisements)
            {
                if (advertisement != null)
                {
                    string advertisementText = $"Тема:\t {advertisement.Name}\n\n Вміст:\t {advertisement.Description}";
                    List<string> lines = new List<string>();
                    int charactersPerLine = 110;
                    for (int i = 0; i < advertisementText.Length; i += charactersPerLine)
                    {
                        int length = Math.Min(charactersPerLine, advertisementText.Length - i);
                        lines.Add(advertisementText.Substring(i, length));
                    }

                    string wrappedText = string.Join(Environment.NewLine, lines);
                    TextBlock advertisementInfo = new TextBlock
                    {
                        Text = wrappedText,
                        FontSize = 30,
                        Foreground = new SolidColorBrush(Colors.DarkBlue),
                        Margin = new Thickness(90, 80, 0, 5),
                    };
                    this.Panel.Children.Add(advertisementInfo);
                }
            }

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

