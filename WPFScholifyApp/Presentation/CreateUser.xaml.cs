// <copyright file="CreateUser.xaml.cs" company="PlaceholderCompany">
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
    /// Interaction logic for CreateUser.xaml.
    /// </summary>
    public partial class CreateUser : Window
    {
        private IGenericRepository<User> userRepository;
        private IGenericRepository<Pupil> pupilRepository;
        private UserService userService;

        public int ClassId { get; set; }

        public string TestFirstName { get; set; } = string.Empty;

        public CreateUser(IGenericRepository<User> userRepos, IGenericRepository<Pupil> pupilRepos)
        {
            this.pupilRepository = pupilRepos;
            this.userRepository = userRepos;
            this.userService = new UserService(new GenericRepository<User>(), new GenericRepository<Pupil>());
            this.InitializeComponent();
            this.Zoriana.Content = this.TestFirstName;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveUser(object sender, RoutedEventArgs e)
        {
            string email = this.Email.Text;
            string password = this.Password.Text;
            string firstName = this.FirstName.Text;
            string middleName = this.MiddleName.Text;
            string lastName = this.LastName.Text;
            string gender = this.Gender.Text;
            string adress = this.Adress.Text;
            string phoneNumber = this.PhoneNumber.Text;
            var role = ((ComboBoxItem)this.RoleComboBox.SelectedItem).Content == null ? ((ComboBoxItem)this.RoleComboBox.SelectedItem).Content : "учень";
            this.Close();

            var user = new User
            {
                Id = this.userRepository.GetAll().Select(x => x.Id).Max() + 1,
                Email = email,
                Password = password,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Gender = gender,
                Address = adress,
                PhoneNumber = phoneNumber,
                Role = role.ToString(),
            };

            var pupil = new Pupil
            {
                Id = this.userRepository.GetAll().Select(x => x.Id).Max() + 1,
                ClassId = this.ClassId,
            };

            this.userService.AddUser(user, pupil);
        }
    }
}
