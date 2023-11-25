// <copyright file="TeacherWindow.xaml.cs" company="PlaceholderCompany">
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
    /// Interaction logic for TeacherWindow.xaml.
    /// </summary>
    public partial class TeacherWindow : Window
    {
        private AdminService adminService;
        private TeacherService teacherService;
        private AdvertisementService advertisementService;
        public int selectedClassId;

        public TeacherWindow()
        {
            this.adminService = new AdminService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Advertisement>());
            this.teacherService = new TeacherService(new GenericRepository<User>(), new GenericRepository<Advertisement>(), new GenericRepository<Class>());
            this.advertisementService = new AdvertisementService(new GenericRepository<Advertisement>(), new GenericRepository<Class>());
            this.InitializeComponent();
        }

        private void ParentsButton_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteFromTeacherPanel();
        }

        public void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteFromTeacherPanel();
        }

        public void AnnouncementsButton_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteFromTeacherPanel();
            ShowAllClasses();
        }

        public void SpecificClassButton_Click(object sender, RoutedEventArgs e)
        {
            // Знайдемо ClassId з Tag кнопки, на яку ми натискали
            var classButton = (Button)sender;
            this.selectedClassId = (int)classButton.Tag;

            // Додамо кнопки з учнями
            this.ShowAllAdvertisementsForClassId(this.selectedClassId);
        }

        public void ShowAllClasses()
        {
            this.DeleteFromTeacherPanel();
            this.RightAction.Children.Clear();

            var classes = this.adminService.GetAllClasses();

            foreach (var c in classes)
            {
                var button = new Button { Content = c.ClassName, Height = 60, Width = 350, FontSize = 30, Tag = c.Id };
                button.Click += new RoutedEventHandler(this.SpecificClassButton_Click);
                this.LeftPanel.Children.Add(button);
            }

            this.UpdateTeacherPanel();
            
        }

        // виводить інфу по оголошеннях
        public void ShowAllAdvertisementsForClassId(int Id)
        {
            this.DeleteFromTeacherPanel();
            this.ShowAllClasses();
            var advertisements = this.teacherService.GetAllAdvertisementsForClassId(Id);

            foreach (var ad in advertisements)
            {
                var lookButton = new Button { Content = $"Переглянути Оголошення '{ad!.Name}'", Height = 80, Width = 700, FontSize = 30, Tag = ad.Id };
                lookButton.Click += new RoutedEventHandler(this.LookAvertisement);
                this.RightPanel.Children.Add(lookButton);
                var deleteButton = new Button { Content = $" Видалити Оголошення '{ad!.Name}'", Height = 80, Width = 700, FontSize = 30, Tag = ad.Id };
                deleteButton.Click += new RoutedEventHandler(this.DeleteAdvertisements); this.LeftPanel.UpdateLayout(); ;
                this.RightPanel.Children.Add(deleteButton);
                this.LeftPanel.UpdateLayout();

            }

            // Після виведення всіх учнів для обраного класу додамо кнопку "Додати Оголошення"
            var createButton = new Button { Content = "Додати Оголошення", Height = 80, Width = 700, FontSize = 30, Tag = Id };
            createButton.Click += new RoutedEventHandler(this.AddAdvertisements);
            this.RightAction.Children.Add(createButton);
            this.UpdateTeacherPanel();
        }
        private void LookAvertisement(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            this.RightPanel.Children.Clear();
            this.RightAction.Children.Clear();

            AdvertisementService advertisementService = new AdvertisementService(new GenericRepository<Advertisement>(), new GenericRepository<Class>());
            var advertisement = this.advertisementService.GetAllAdvertisementsForClassId(this.selectedClassId).FirstOrDefault(x => x.Id == (int)createButton.Tag);
            if (advertisement != null)
            {
                TextBlock advertisementInfo = new TextBlock
                {

                    Text = $"Тема:\t {advertisement.Name}\n\n Вміст:\t {advertisement.Description}",
                    FontSize = 40,
                    Foreground = new SolidColorBrush(Colors.DarkBlue),
                    Margin = new Thickness(180, 90, 0, 10),
                };
                this.RightPanel.Children.Add(advertisementInfo);
            }

            this.UpdateTeacherPanel();
        }
         
        private void DeleteAdvertisements(object sender, RoutedEventArgs e)
        {
            DeleteFromTeacherPanel();
            ShowAllClasses();
            var deleteButton = (Button)sender;
            this.advertisementService.DeletedAvertisementl((int)deleteButton.Tag);
            ShowAllAdvertisementsForClassId(selectedClassId);
            UpdateTeacherPanel();
        }

        private void AddAdvertisements(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            var createPanel = new CreateAdvertisements(new GenericRepository<Teacher>(), new GenericRepository<Advertisement>(), new GenericRepository<User>(), this, new GenericRepository<Class>());
            createPanel.ClassId = this.selectedClassId;
            createPanel.Show();
        }
        private void PrivateInfoButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteFromTeacherPanel();

            TextBlock titleLabel = new TextBlock
            {
                Text = "Приватна інформація",
                FontSize = 50,
                Foreground = new SolidColorBrush(Colors.DarkBlue),
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(180, 30, 0, 10),
            };
            this.RightPanel.Children.Add(titleLabel);

            UserService userService = new UserService(new GenericRepository<User>(), new GenericRepository<Pupil>());
            string name = this.FirstNameTextBlock.Text;
            string surname = this.LastNameTextBlock.Text;
            User teacher = userService.GetInfoByNameSurname(name, surname);

            if (teacher != null)
            {
                TextBlock studentInfo = new TextBlock
                {
                    Text = $"Ім'я:\t\t {teacher.FirstName}\n\nПрізвище:\t {teacher.LastName}\n\nПо батькові:\t {teacher.MiddleName}\n\nСтать:\t\t {teacher.Gender}" +
                        $"\n\nДата народження: {teacher.Birthday:dd.MM.yyyy}\n\nАдреса:\t\t {teacher.Address}\n\nТелефон:\t {teacher.PhoneNumber}",
                    FontSize = 40,
                    Foreground = new SolidColorBrush(Colors.DarkBlue),
                    Margin = new Thickness(180, 90, 0, 10),
                };
                this.RightPanel.Children.Add(studentInfo);
            }

            this.UpdateTeacherPanel();
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

        private void ClassJournalButton_Click(object sender, RoutedEventArgs e)
        {
        }

        public void DeleteFromTeacherPanel()
        {
            this.RightPanel.Children.Clear();
            this.LeftPanel.Children.Clear();
            this.RightAction.Children.Clear();
        }

        public void UpdateTeacherPanel()
        {
            this.RightPanel.UpdateLayout();
            this.LeftPanel.UpdateLayout();
            this.RightAction.UpdateLayout();
        }
    }
}
