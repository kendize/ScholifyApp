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
        private IGenericRepository<User> userRepository;
        private IGenericRepository<Pupil> pupilRepository;
        private UserService userService;

        public LookUsers(IGenericRepository<User> userRepos, IGenericRepository<Pupil> pupilRepos)
        {
            this.pupilRepository = pupilRepos;
            this.userRepository = userRepos;
            this.userService = new UserService(new GenericRepository<User>(), new GenericRepository<Pupil>());
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
