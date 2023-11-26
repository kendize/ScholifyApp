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
            var role = ((ComboBoxItem)this.RoleComboBox.SelectedItem).Content;
            if (role != null)
            {
                string selectedRole = role.ToString() !;
                UserService userService = new UserService(new GenericRepository<User>(), new GenericRepository<Pupil>());
                User authenticatedUser = userService.Authenticate(email, password, selectedRole);
                User authenticatedEmail = userService.AuthenticateEmail(email);
                User authenticatedPassword = userService.AuthenticatePassword(password);

                if (authenticatedUser != null)
                {
                    if (selectedRole == "учень")
                    {
                        PupilWindow pupilWindow = new PupilWindow(authenticatedUser, new GenericRepository<Pupil>());
                        pupilWindow.FirstNameTextBlock.Text = userService.Authenticate(email, password, selectedRole).FirstName;
                        pupilWindow.LastNameTextBlock.Text = userService.Authenticate(email, password, selectedRole).LastName;
                        pupilWindow.Show();
                    }
                    else if (selectedRole == "вчитель")
                    {
                        TeacherWindow teacherWindow = new TeacherWindow();
                        teacherWindow.FirstNameTextBlock.Text = userService.Authenticate(email, password, selectedRole).FirstName;
                        teacherWindow.LastNameTextBlock.Text = userService.Authenticate(email, password, selectedRole).LastName;
                        teacherWindow.Show();
                    }
                    else if (selectedRole == "батьки")
                    {
                        ParentsWindow parentsWindow = new ParentsWindow();
                        parentsWindow.FirstNameTextBlock.Text = userService.Authenticate(email, password, selectedRole).FirstName;
                        parentsWindow.LastNameTextBlock.Text = userService.Authenticate(email, password, selectedRole).LastName;
                        parentsWindow.Show();
                    }
                    else
                    {
                        AdminWindow adminWindow = new AdminWindow(authenticatedUser);
                        adminWindow.FirstNameTextBlock.Text = userService.Authenticate(email, password, selectedRole).FirstName;
                        adminWindow.LastNameTextBlock.Text = userService.Authenticate(email, password, selectedRole).LastName;
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
            else
            {
                throw new InvalidOperationException("Role is null");
            }
        }
    }
}
