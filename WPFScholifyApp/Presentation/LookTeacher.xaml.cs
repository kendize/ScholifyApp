// <copyright file="LookTeacher.xaml.cs" company="PlaceholderCompany">
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
    /// Interaction logic for LookTeacher.xaml.
    /// </summary>
    public partial class LookTeacher : Window
    {
        private IGenericRepository<User> userRepository;
        private IGenericRepository<Teacher> teacherRepository;
        private TeacherService teacherService;
        public AdminWindow AdminWindow { get; set; }
        public User? currentUser { get; set; }
        public int currentClassId { get; set; }
        public LookTeacher(IGenericRepository<User> userRepos, IGenericRepository<Teacher> teacherRepos, AdminWindow adminWindow)
        {
            this.teacherRepository = teacherRepos;
            this.userRepository = userRepos;
            this.teacherService = new TeacherService(new GenericRepository<Advertisement>(), new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Schedule>());
            this.AdminWindow = adminWindow;
            this.InitializeComponent();
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
                Role = "вчитель"
            };


            this.userRepository.Update(user);
            this.userRepository.Save();

            this.AdminWindow.DeleteFromAdminPanels();
            this.AdminWindow.ShowAllTeachers();
            //this.AdminWindow.ShowAllClasses();
            //this.AdminWindow.ShowAllPupilsForClassId(currentClassId);

            this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
