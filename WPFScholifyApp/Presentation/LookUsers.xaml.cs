// <copyright file="LookUsers.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.Presentation
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
    /// Interaction logic for LookUsers.xaml.
    /// </summary>
    public partial class LookUsers : Window
    {
        public bool ShowAllTeachers { get; set; } = false;
        public bool ShowAllClasses { get; set; } = false;
        private IGenericRepository<User> userRepository;
        private IGenericRepository<Pupil> pupilRepository;
        private UserService userService;
        public AdminWindow AdminWindow { get; set; }

        public User? currentUser { get; set; }

        public int currentClassId { get; set; }

        public int currentPupilId { get; set; }

        public int parentId { get; set; }
        public LookUsers(IGenericRepository<User> userRepos, IGenericRepository<Pupil> pupilRepos, AdminWindow adminWindow, IGenericRepository<Parents> parentsRepository, IGenericRepository<ParentsPupil> parentPupilRepository)
        {
            this.pupilRepository = pupilRepos;
            this.userRepository = userRepos;
            this.userService = new UserService(userRepos, pupilRepos, parentsRepository, parentPupilRepository);
            this.InitializeComponent();
            AdminWindow = adminWindow;
        }

        private void SaveUser(object sender, RoutedEventArgs e)
        {
            var user = new User
            {
                Id = currentUser!.Id,
                Email = this.Email.Text,
                Password = this.Password.Text,
                FirstName = this.FirstName.Text,
                LastName = this.LastName.Text,
                MiddleName = this.MiddleName.Text,
                Gender = this.Gender.Text,
                Birthday = this.Birthday.SelectedDate!.Value.ToUniversalTime(),
                Address = this.Adress.Text,
                PhoneNumber = this.PhoneNumber.Text,
                Role = currentUser.Role
            };

            this.userRepository.Update(user);
            this.userRepository.Save();

            if (currentUser.Role == "учень")
            {

                this.AdminWindow.DeleteFromAdminPanels();
                if (this.ShowAllTeachers)
                {
                    this.AdminWindow.ShowAllTeachers();
                }

                if (this.ShowAllClasses)
                {
                    this.AdminWindow.ShowAllClasses();
                    this.AdminWindow.ShowAllPupilsForClassId(this.currentClassId);
                }

                this.AdminWindow.UpdateAdminPanels();
            }

            if (currentUser.Role == "батьки")
            {
                this.AdminWindow.DeleteFromAdminPanels();
                this.AdminWindow.ShowAllPuplis();
                this.AdminWindow.ShowParentsForPupilId(this.currentPupilId);
                this.AdminWindow.UpdateAdminPanels();
            }   
            
            if (currentUser.Role == "вчитель")
            {
                this.AdminWindow.DeleteFromAdminPanels();
                this.AdminWindow.ShowAllTeachers();

            }

            this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
