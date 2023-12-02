namespace WPFScholifyApp.Presentation
{
    using System;
    using System.Windows;
    using WPFScholifyApp.BLL;
    using WPFScholifyApp.DAL.ClassRepository;
    using WPFScholifyApp.DAL.DBClasses;
    using DayOfWeek = DAL.DBClasses.DayOfWeek;

    public partial class LookUsers : Window
    {
        private UserService userService;
        private WindowService windowService;
        private MainWindow mainWindow;

        public bool ShowAllTeachers { get; set; } = false;
        public bool ShowAllClasses { get; set; } = false;
        public User? currentUser { get; set; }
        public int currentClassId { get; set; }
        public int currentPupilId { get; set; }
        public int parentId { get; set; }

        public LookUsers(
            UserService userService,
            WindowService windowService,
            MainWindow mainWindow)
        {
            this.userService = userService;
            this.windowService = windowService;
            this.mainWindow = mainWindow;
            this.InitializeComponent();
        }

        private void SaveUser(object sender, RoutedEventArgs e)
        {
            this.windowService.Show<AdminWindow>(window =>
            {
                var user = new User
                {
                    Id = currentUser!.Id,
                    Email = Email.Text,
                    Password = Password.Text,
                    FirstName = FirstName.Text,
                    LastName = LastName.Text,
                    MiddleName = MiddleName.Text,
                    Gender = Gender.Text,
                    Birthday = Birthday.SelectedDate!.Value.ToUniversalTime(),
                    Address = Adress.Text,
                    PhoneNumber = PhoneNumber.Text,
                    Role = currentUser.Role
                };

                this.userService.SaveUser(user);

                window.DeleteFromAdminPanels();

                if (currentUser.Role == "учень")
                {
                    if (ShowAllTeachers)
                        window.ShowAllTeachers();

                    if (ShowAllClasses)
                    {
                        window.ShowAllPupilsForClassId(currentClassId);
                    }
                }

                if (currentUser.Role == "батьки")
                {
                    window.ShowAllPuplis();
                    window.ShowParentsForPupilId(currentPupilId);
                }

                if (currentUser.Role == "вчитель")
                {
                    window.ShowAllTeachers();
                }

                window.UpdateAdminPanels();
            });

            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
