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
        private TeacherService teacherService;

        public LookTeacher(IGenericRepository<User> userRepos)
        {
            this.userRepository = userRepos;
            this.teacherService = new TeacherService(new GenericRepository<User>());
            this.InitializeComponent();
        }

        private void SaveUser(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
