// <copyright file="AdminWindow.xaml.cs" company="PlaceholderCompany">
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
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using WPFScholifyApp.BLL;
    using WPFScholifyApp.DAL.ClassRepository;
    using WPFScholifyApp.DAL.DBClasses;
    using WPFScholifyApp.Presentation;

    /// <summary>
    /// Interaction logic for AdminWindow.xaml.
    /// </summary>
    public partial class AdminWindow : Window
    {
        private AdminService adminService;
        private UserService userService;
        private Class? testclass;
        private IGenericRepository<User> userRepository;
        private IGenericRepository<Pupil> pupilRepository;

        public AdminWindow()
        {
            this.userRepository = new GenericRepository<User>();
            this.pupilRepository = new GenericRepository<Pupil>();
            this.userService = new UserService(new GenericRepository<User>(), new GenericRepository<Pupil>());
            this.adminService = new AdminService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>());
            this.InitializeComponent();
        }

        private void ClassButton_Click(object sender, RoutedEventArgs e)
        {
            this.InfoPanel.Children.Clear();
            var classes = this.adminService.GetAllClasses();
            foreach (var c in classes)
            {
                var button = new Button { Content = c.ClassName, Height = 60, Width = 300, FontSize = 30, Tag = c.Id };
                button.Click += new RoutedEventHandler(this.SpecificClassButton_Click);
                this.InfoPanel.Children.Add(button);
            }
        }

        private void TeachersButton_Click(object sender, RoutedEventArgs e)
        {
            var teacher = this.adminService.GetAllTeacher();
            foreach (var c in teacher)
            {
                var button = new Button { Content = c.Teacher, Height = 60, Width = 300, FontSize = 30, };
                button.Click += new RoutedEventHandler(this.SpecificClassButton_Click);
                this.InfoPanel.Children.Add(button);
            }
        }

        public void SpecificClassButton_Click(object sender, RoutedEventArgs e)
        {
            this.PupilsPanel.Children.Clear();
            var classButton = (Button)sender;
            this.testclass = this.adminService.GetAllClasses().FirstOrDefault(x => x.Id == (int)classButton.Tag);
            var pupils = this.adminService.GetAllPupilsForClass((int)classButton.Tag);
            foreach (var p in pupils)
            {
                var pupilButton = new Button { Content = $"{p!.FirstName} {p!.LastName}", Height = 60, Width = 300, FontSize = 30, };
                var deleteButton = new Button { Content = $"Delete {p!.FirstName} {p!.LastName}", Height = 60, Width = 300, FontSize = 30, Tag = p.Id };
                deleteButton.Click += new RoutedEventHandler(this.DeleteUser);

                this.PupilsPanel.Children.Add(pupilButton);
                this.PupilsPanel.Children.Add(deleteButton);
            }

            var createButton = new Button { Content = "Додати Учня", Tag = (int)classButton.Tag, Height = 60, Width = 300, FontSize = 30, };

            createButton.Click += new RoutedEventHandler(this.AddUser);
            this.PupilsPanel.Children.Add(createButton);
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            var createPanel = new CreateUser(this.userRepository, this.pupilRepository);
            createPanel.ClassId = (int)createButton.Tag;
            createPanel.Show();
            this.PupilsPanel.UpdateLayout();
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            var deleteButton = (Button)sender;
            this.userService.DeleteUser((int)deleteButton.Tag);
            this.PupilsPanel.UpdateLayout();
        }

        private void ParentsButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
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
    }
}
