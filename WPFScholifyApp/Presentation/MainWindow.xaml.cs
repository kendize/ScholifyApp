// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using WPFScholifyApp.BLL;
    using WPFScholifyApp.DAL.ClassRepository;
    using WPFScholifyApp.DAL.DBClasses;
    using DayOfWeek = DAL.DBClasses.DayOfWeek;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string email = this.EmailTextBox.Text;
            string password = this.PasswordBox.Password;
                UserService userService = new UserService(new GenericRepository<User>(), new GenericRepository<Pupil>());
                User authenticatedUser = userService.Authenticate(email, password);
                User authenticatedEmail = userService.AuthenticateEmail(email);
                User authenticatedPassword = userService.AuthenticatePassword(password);

                if (authenticatedUser != null)
                {
                    if (authenticatedUser.Role == "учень")
                    {
                        PupilWindow pupilWindow = new PupilWindow(
                            authenticatedUser, 
                            new PupilService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Schedule>()), 
                            new GenericRepository<DayOfWeek>(), 
                            new GenericRepository<Pupil>(), 
                            new AdminService(
                                new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>() , new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Advertisement>()),
                            new GenericRepository<Class>(), new JournalService(new GenericRepository<DayBook>(), new GenericRepository<Subject>(), new GenericRepository<Class>(), new GenericRepository<DayOfWeek>(), new ScheduleService(new GenericRepository<User> (), new GenericRepository<Class>(), new GenericRepository<Schedule>(), new GenericRepository<Subject>())), new GenericRepository<DayBook>(), new GenericRepository<Schedule>(), new GenericRepository<Subject>());
                        pupilWindow.FirstNameTextBlock.Text = userService.Authenticate(email, password).FirstName;
                        pupilWindow.LastNameTextBlock.Text = userService.Authenticate(email, password).LastName;
                        pupilWindow.Show();
                    }
                    else if (authenticatedUser.Role == "вчитель")
                    {
                        TeacherWindow teacherWindow = new TeacherWindow(authenticatedUser, 
                            new GenericRepository<DayOfWeek>(), 
                            new GenericRepository<Teacher>(), 
                            new JournalService(
                                new GenericRepository <DayBook> (),
                                new  GenericRepository < Subject > (), 
                                new GenericRepository < Class > (), 
                                new GenericRepository<DayOfWeek>(), 
                                new ScheduleService(
                                    new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Schedule>(),  new GenericRepository<Subject>())), 
                            new GenericRepository<DayBook>(),
                                new GenericRepository<Class>(),
                                new GenericRepository<User>(),
                                new GenericRepository<DayBook>(),
                                new GenericRepository<Subject>(),
                                new GenericRepository<Schedule>());

                        teacherWindow.FirstNameTextBlock.Text = userService.Authenticate(email, password).FirstName;
                        teacherWindow.LastNameTextBlock.Text = userService.Authenticate(email, password).LastName;
                        teacherWindow.Show();
                    }
                    else if (authenticatedUser.Role == "батьки")
                    {
                        ParentsWindow parentsWindow = new ParentsWindow();
                        parentsWindow.FirstNameTextBlock.Text = userService.Authenticate(email, password).FirstName;
                        parentsWindow.LastNameTextBlock.Text = userService.Authenticate(email, password).LastName;
                        parentsWindow.Show();
                    }
                    else
                    {
                        AdminWindow adminWindow = new AdminWindow(authenticatedUser);
                        adminWindow.FirstNameTextBlock.Text = userService.Authenticate(email, password).FirstName;
                        adminWindow.LastNameTextBlock.Text = userService.Authenticate(email, password).LastName;
                        adminWindow.Show();
                    }

                    this.Close();
                }
                else
                {
                    string errorMessage = "Помилка автентифікації. Перевірте введені дані.";

                    if (authenticatedEmail == null)
                    {
                        errorMessage = "Неправильно введенна електрона пошта.";
                    }
                    else if (authenticatedPassword == null)
                    {
                        errorMessage = "Неправильно введений пароль.";
                    }
                    else
                    {
                        errorMessage = "Неправильно вибрана роль.";
                    }

                    MessageBox.Show(errorMessage, "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }
    }
}
