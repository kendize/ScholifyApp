﻿// <copyright file="CreateTeacher.xaml.cs" company="PlaceholderCompany">
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
    /// Interaction logic for CreateTecher.xaml.
    /// </summary>
    public partial class CreateTeacher : Window
    {
        private IGenericRepository<User> userRepository;
        private TeacherService teacherService;

        public CreateTeacher(IGenericRepository<User> userRepos)
        {
            this.userRepository = userRepos;
            this.teacherService = new TeacherService(new GenericRepository<User>());
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
            var role = ((ComboBoxItem)this.RoleComboBox.SelectedItem).Content == null ? ((ComboBoxItem)this.RoleComboBox.SelectedItem).Content : "вчитель";
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
                Birthday = birthday,
                Address = adress,
                PhoneNumber = phoneNumber,
                Role = role.ToString(),
            };

            this.teacherService.AddTeacher(user);
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}