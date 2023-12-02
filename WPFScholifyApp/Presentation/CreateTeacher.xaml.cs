// <copyright file="CreateTeacher.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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
    using DayOfWeek = DAL.DBClasses.DayOfWeek;

    /// <summary>
    /// Interaction logic for CreateTecher.xaml.
    /// </summary>
    public partial class CreateTeacher : Window
    {
        private AdminService adminService;
        private AdvertisementService advertisementService;
        private JournalService journalService;
        private ParentsService parentsService;
        private PupilService pupilService;
        private UserService userService;
        private ScheduleService scheduleService;
        private TeacherService teacherService;
        private WindowService windowService;
        private MainWindow mainWindow;

        public CreateTeacher(AdminService adminService,
                            AdvertisementService advertisementService,
                            JournalService journalService,
                            ParentsService parentsService,
                            PupilService pupilService,
                            UserService userService,
                            ScheduleService scheduleService,
                            TeacherService teacherService,
                            WindowService windowService,
                            MainWindow mainWindow
                            )
        {

            this.adminService = adminService;
            this.advertisementService = advertisementService;
            this.journalService = journalService;
            this.parentsService = parentsService;
            this.pupilService = pupilService;
            this.userService = userService;
            this.scheduleService = scheduleService;
            this.teacherService = teacherService;
            this.windowService = windowService;
            this.mainWindow = mainWindow;
            this.InitializeComponent();
        }

        private void SaveUser(object sender, RoutedEventArgs e)
        {
            string email = this.Email.Text;
            string password = this.Password.Text;
            string firstName = this.FirstName.Text;
            string middleName = this.MiddleName.Text;
            string lastName = this.LastName.Text;
            string gender = this.Gender.Text;
            DateTime birthday = this.Birthday.DisplayDate.ToUniversalTime();
            string adress = this.Adress.Text;
            string phoneNumber = this.PhoneNumber.Text;


            var user = new User
            {
                Id = this.adminService.GetNewUserId(),
                Email = email,
                Password = password,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Gender = gender,
                Birthday = birthday,
                Address = adress,
                PhoneNumber = phoneNumber,
                Role = "вчитель",
            };


            this.teacherService.AddTeacher(user);
            this.Hide();

            this.windowService.Show<AdminWindow>(window =>
            {
                window.DeleteFromAdminPanels();
                window.ShowAllTeachers();
                window.UpdateAdminPanels();
            });
                
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
