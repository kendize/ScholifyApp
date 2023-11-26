// <copyright file="TeacherWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
    using DayOfWeek = DAL.DBClasses.DayOfWeek;

    /// <summary>
    /// Interaction logic for TeacherWindow.xaml.
    /// </summary>
    public partial class TeacherWindow : Window
    {
        private AdminService adminService;
        private TeacherService teacherService;
        private IGenericRepository<DayOfWeek> dayOfWeekRepository;
        private IGenericRepository<Teacher> teacherRepository;

        public int selectedClassId;
        private User CurrentUser;
        public List<DateTime> Days { get; set; }

        public TeacherWindow(User currentUser, IGenericRepository<DayOfWeek> dayOfWeekRepository, IGenericRepository<Teacher> teacherRepository)
        {
            this.dayOfWeekRepository = dayOfWeekRepository;
            this.teacherRepository = teacherRepository;
            this.InitializeComponent();
            CultureInfo culture = new CultureInfo("uk-UA"); // Adjust culture as needed

            // Get the current date
            DateTime currentDate = DateTime.Now;

            // Calculate the start date of the current week (Monday)
            DateTimeFormatInfo dfi = culture.DateTimeFormat;
            System.DayOfWeek firstDayOfWeek = dfi.FirstDayOfWeek;
            int daysToSubtract = (int)currentDate.DayOfWeek - (int)firstDayOfWeek;
            if (daysToSubtract < 0)
            {
                daysToSubtract += 7;
            }

            DateTime startDate = currentDate.AddDays(-daysToSubtract);

            // Create a list to store the days of the week
            List<DateTime> daysOfWeek = new List<DateTime>();

            // Add each day of the week to the list
            for (int i = 0; i < 7; i++)
            {
                daysOfWeek.Add(startDate.AddDays(i));
            }
            this.Days = daysOfWeek;
            this.CurrentUser = currentUser;
            this.adminService = new AdminService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>());
            this.teacherService = new TeacherService( new GenericRepository<Advertisement>(), new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Schedule>());
            this.InitializeComponent();
        }

        private void ParentsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.InfoPanel.Visibility = Visibility.Visible;
        }

        public void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Visible;
            this.InfoPanel.Visibility = Visibility.Hidden;
            ShowAllWeek();
        }

        public void AnnouncementsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.InfoPanel.Visibility = Visibility.Visible;
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
            this.Schedule.Visibility = Visibility.Hidden;
            this.InfoPanel.Visibility = Visibility.Visible;
            this.DeleteFromTeacherPanel();

            var classes = this.adminService.GetAllClasses();

            foreach (var c in classes)
            {
                var button = new Button { Content = c.ClassName, Height = 60, Width = 300, FontSize = 30, Tag = c.Id };
                button.Click += new RoutedEventHandler(this.SpecificClassButton_Click);
                this.InfoPanel.Children.Add(button);
            }

            this.UpdateTeacherPanel();
        }

        public void ShowAllAdvertisementsForClassId(int Id)
        {
            this.DeleteFromTeacherPanel();

            var advertisements = this.teacherService.GetAllAdvertisementsForClassId(Id);

            foreach (var ad in advertisements)
            {
                var textbox = new TextBox { Text = ad.Description, FontSize = 30 };
                this.InfoPanel.Children.Add(textbox);
            }
            this.UpdateTeacherPanel();

        }

        private void PrivateInfoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.InfoPanel.Visibility = Visibility.Visible;
            this.DeleteFromTeacherPanel();

            TextBlock titleLabel = new TextBlock
            {
                Text = "Приватна інформація",
                FontSize = 50,
                Foreground = new SolidColorBrush(Colors.DarkBlue),
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(410, 30, 0, 10),
            };
            this.InfoPanel.Children.Add(titleLabel);

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
                    Margin = new Thickness(430, 90, 0, 10),
                };
                this.InfoPanel.Children.Add(studentInfo);
            }

            this.UpdateTeacherPanel();
        }

        private void ChatButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.InfoPanel.Visibility = Visibility.Visible;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }

        private void ClassJournalButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.InfoPanel.Visibility = Visibility.Visible;
        }

        public void ShowAllWeek()
        {
            ClearDays();
            var result = new List<Schedule>();
            var dayOfWeeks = this.dayOfWeekRepository.GetAll();
            var teacherIds = this.teacherRepository.GetAll().Select(t => t.Id).ToList();
            for (int i = 0; i <= 6; i++)
            {

                var dayOfWeek = dayOfWeeks.FirstOrDefault(x => x.Date.AddDays(1).Date.Equals(this.Days[i].Date));
                var dayOfWeekId = dayOfWeek != null ? dayOfWeek!.Id : 0;
                var schedulesForDay = new List<Schedule> ();
                foreach(var t in teacherIds)
                {
                    schedulesForDay.AddRange(this.teacherService.GetAllSchedules(t, dayOfWeekId).ToList());
                }

                result.AddRange(schedulesForDay);

                if (i == 0)
                {
                    Label label1 = new Label();
                    var date = this.Days[i].Date.Date.ToString("d");


                    label1.Content = $"Понеділок {date}";
                    label1.FontSize = 24;
                    label1.HorizontalAlignment = HorizontalAlignment.Center;
                    this.Monday.Children.Add(label1);
                    foreach (var schedule in schedulesForDay.OrderBy(x => x.LessonTime!.StartTime))
                    {
                        Label label2 = new Label();
                        label2.Content = $"{schedule.LessonTime!.Start} {schedule.Subject!.SubjectName}";
                        label2.FontSize = 24;
                        this.Monday.Children.Add(label2);
                        this.Monday.UpdateLayout();

                    }
                }
                if (i == 1)
                {
                    Label label1 = new Label();
                    var date = this.Days[i].Date.Date.ToString("d");


                    label1.Content = $"Вівторок {date}";
                    label1.FontSize = 24;
                    label1.HorizontalAlignment = HorizontalAlignment.Center;
                    this.Tuesday.Children.Add(label1);
                    foreach (var schedule in schedulesForDay.OrderBy(x => x.LessonTime!.StartTime))

                    {
                        Label label2 = new Label();
                        label2.Content = $"{schedule.LessonTime!.Start} {schedule.Subject!.SubjectName}";
                        label2.FontSize = 24;
                        this.Tuesday.Children.Add(label2);
                        this.Tuesday.UpdateLayout();
                    }
                }
                if (i == 2)
                {
                    Label label1 = new Label();
                    var date = this.Days[i].Date.Date.ToString("d");



                    label1.Content = $"Середа {date}";
                    label1.FontSize = 24;
                    label1.HorizontalAlignment = HorizontalAlignment.Center;
                    this.Wednesday.Children.Add(label1);
                    foreach (var schedule in schedulesForDay.OrderBy(x => x.LessonTime!.StartTime))

                    {
                        Label label2 = new Label();
                        label2.Content = $"{schedule.LessonTime!.Start} {schedule.Subject!.SubjectName}";
                        label2.FontSize = 24;
                        this.Wednesday.Children.Add(label2);
                        this.Wednesday.UpdateLayout();
                    }
                }
                if (i == 3)
                {
                    Label label1 = new Label();
                    var date = this.Days[i].Date.Date.ToString("d");



                    label1.Content = $"Четвер {date}";
                    label1.FontSize = 24;
                    label1.HorizontalAlignment = HorizontalAlignment.Center;
                    this.Thursday.Children.Add(label1);
                    foreach (var schedule in schedulesForDay.OrderBy(x => x.LessonTime!.StartTime))

                    {
                        Label label2 = new Label();
                        label2.Content = $"{schedule.LessonTime!.Start} {schedule.Subject!.SubjectName}";
                        label2.FontSize = 24;
                        this.Thursday.Children.Add(label2);
                        this.Thursday.UpdateLayout();
                    }
                }
                if (i == 4)
                {
                    Label label1 = new Label();
                    var date = this.Days[i].Date.Date.ToString("d");


                    label1.Content = $"П'ятниця {date}";
                    label1.FontSize = 24;
                    label1.HorizontalAlignment = HorizontalAlignment.Center;
                    this.Friday.Children.Add(label1);
                    foreach (var schedule in schedulesForDay.OrderBy(x => x.LessonTime!.StartTime))

                    {
                        Label label2 = new Label();
                        label2.Content = $"{schedule.LessonTime!.Start} {schedule.Subject!.SubjectName}";
                        label2.FontSize = 24;
                        this.Friday.Children.Add(label2);
                        this.Friday.UpdateLayout();
                    }
                }

            }
            UpdateDays();
            return;

        }

        public void ChangeDate(object sender, RoutedEventArgs e)
        {
            var changeDateButton = (Button)sender;
            int daysToAdd = int.Parse((string)changeDateButton.Tag);
            CultureInfo culture = new CultureInfo("uk-UA"); // Adjust culture as needed

            // Get the current date
            DateTime currentDate = this.Days[0];

            // Calculate the start date of the current week (Monday)
            DateTimeFormatInfo dfi = culture.DateTimeFormat;
            System.DayOfWeek firstDayOfWeek = dfi.FirstDayOfWeek;
            int daysToSubtract = (int)currentDate.DayOfWeek - (int)firstDayOfWeek;
            if (daysToSubtract < 0)
            {
                daysToSubtract += 7;
            }

            DateTime startDate = currentDate.AddDays(-daysToSubtract + daysToAdd);

            // Create a list to store the days of the week
            List<DateTime> daysOfWeek = new List<DateTime>();

            // Add each day of the week to the list
            for (int i = 0; i < 7; i++)
            {
                daysOfWeek.Add(startDate.AddDays(i));
            }
            this.Days = daysOfWeek;

            this.ShowAllWeek();
        }

        public void DeleteFromTeacherPanel()
        {
            this.InfoPanel.Children.Clear();
        }

        public void UpdateTeacherPanel()
        {
            this.InfoPanel.UpdateLayout();
        }

        public void ClearDays()
        {
            this.Monday.Children.Clear();
            this.Tuesday.Children.Clear();
            this.Wednesday.Children.Clear();
            this.Thursday.Children.Clear();
            this.Friday.Children.Clear();
        }
        public void UpdateDays()
        {
            this.Monday.UpdateLayout();
            this.Tuesday.UpdateLayout();
            this.Wednesday.UpdateLayout();
            this.Thursday.UpdateLayout();
            this.Friday.UpdateLayout();
        }
    }
}
