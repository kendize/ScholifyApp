// <copyright file="ParentsWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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
    /// Interaction logic for ParentsWindow.xaml.
    /// </summary>
    public partial class ParentsWindow : Window
    {
        private AdminService adminService;
        private AdvertisementService advertisementService;
        private ClassService classService;
        private JournalService journalService;
        private ParentsService parentsService;
        private PupilService pupilService;
        private UserService userService;
        private ScheduleService scheduleService;
        private SubjectService subjectService;
        private TeacherService teacherService;
        private WindowService windowService;

        private MainWindow mainWindow;
        private AdminWindow adminWindow;


        public User? _authenticatedUser { get; set; }
        private readonly MainWindow _mainWindow;
        public List<DateTime> Days { get; set; }
        public List<Pupil> pupils { get; set; }

        public ParentsWindow(AdminService adminService,
                            AdvertisementService advertisementService,
                            ClassService classService,
                            JournalService journalService,
                            ParentsService parentsService,
                            PupilService pupilService,
                            UserService userService,
                            ScheduleService scheduleService,
                            SubjectService subjectService,
                            TeacherService teacherService,
                            WindowService windowService,

                            MainWindow mainWindow,
                            AdminWindow adminWindow
                            )
        {

            this.adminService = adminService;
            this.advertisementService = advertisementService;
            this.classService = classService;
            this.journalService = journalService;
            this.parentsService = parentsService;
            this.pupilService = pupilService;
            this.userService = userService;
            this.scheduleService = scheduleService;
            this.subjectService = subjectService;
            this.teacherService = teacherService;
            this.windowService = windowService;

            this.mainWindow = mainWindow;
            this.adminWindow = adminWindow;
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
            this.pupilService = pupilService;

            this.pupils = this.pupilService.GetAllPupils();

            _mainWindow = mainWindow;
            this.Closing += new CancelEventHandler(this.Window_Closing!);
            this.InitializeComponent();
        }

        public void Window_Closing(object sender, CancelEventArgs e)
        {
            this.Hide();
            this.adminWindow.Show();
        }

        private void JournalButton_Click(object sender, RoutedEventArgs e)
        {
            this.InfoPanel.Children.Clear();
            var listOfUsers = this.pupils.Select(x => x.UserId).ToList();
            foreach (var u in listOfUsers)
            {
                this.ShowJournalForUserId(u);
            }
            this.InfoPanel.UpdateLayout();

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Show();

            this.Hide();

        }

        private void PrivateInfoButton_Click(object sender, RoutedEventArgs e)
        {
            this.InfoPanel.Children.Clear();

            TextBlock titleLabel = new TextBlock
            {
                Text = "Приватна інформація",
                FontSize = 50,
                Foreground = new SolidColorBrush(Colors.DarkBlue),
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(410, 30, 0, 10),
            };
            this.InfoPanel.Children.Add(titleLabel);

            string name = this.FirstNameTextBlock.Text;
            string surname = this.LastNameTextBlock.Text;
            User parents = userService.GetInfoByNameSurname(name, surname);

            if (parents != null)
            {
                TextBlock studentInfo = new TextBlock
                {
                    Text = $"Ім'я:\t\t {parents.FirstName}\n\nПрізвище:\t {parents.LastName}\n\nПо батькові:\t {parents.MiddleName}\n\nСтать:\t\t {parents.Gender}" +
                    $"\n\nДата народження: {parents.Birthday:dd.MM.yyyy}\n\nАдреса:\t\t {parents.Address}\n\nТелефон:\t {parents.PhoneNumber}",
                    FontSize = 40,
                    Foreground = new SolidColorBrush(Colors.DarkBlue),
                    Margin = new Thickness(430, 90, 0, 10),
                };
                this.InfoPanel.Children.Add(studentInfo);
            }

            
            
        }

        public void ShowJournalForUserId(int id)
        {
            //this.ShowAllSubjectsForTeacher();
            var classId = this.classService.GetClassByUserId(id).Id;
            var group = this.subjectService.GetSubjectsByClassId(classId);

            var grades = this.journalService.GetDayBooksForUserId(id);
            var dates = this.scheduleService.GetDatesByClassId(classId);


            dates = dates.OrderBy(x => x.Date).ToList();
            // Create Grid for table
            var grid = new Grid();

            // Define the rows in the grid
            for (int i = 0; i <= group.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            // Create columns for user names and dates
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto }); // User column

            foreach (var date in dates)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto }); // Date columns
            }

            // Add headers
            for (int i = 0; i <= dates.Count; i++)
            {
                var label = new Label();
                label.FontSize = 20;
                label.HorizontalAlignment = HorizontalAlignment.Center;

                if (i == 0)
                {
                    label.Content = "Предмет";
                }
                else
                {
                    label.Content = dates[i - 1].Date.ToString("d");
                }

                Grid.SetColumn(label, i);
                Grid.SetRow(label, 0);
                grid.Children.Add(label);
            }

            // Populate data
            for (int i = 0; i < group.Count; i++)
            {
                var subject = group[i];

                var subjectLabel = new Label();
                subjectLabel.FontSize = 25;
                subjectLabel.Content = $"{subject.SubjectName}";
                Grid.SetColumn(subjectLabel, 0);
                Grid.SetRow(subjectLabel, i + 1);
                grid.Children.Add(subjectLabel);

                for (int j = 0; j < dates.Count; j++)
                {
                    var date = dates[j];
                    var dayBook = grades.FirstOrDefault(x => x.Schedule.SubjectId == subject.Id && x.Schedule.DayOfWeek.Day.Equals(date.Day));

                    if (dayBook != null)
                    {
                        var gradeButton = new Button();
                        gradeButton.FontSize = 25;
                        gradeButton.Width = 110;
                        gradeButton.Height = 45;
                        gradeButton.Content = dayBook.Grade.ToString();
                        gradeButton.Tag = dayBook.Id;

                        Grid.SetColumn(gradeButton, j + 1);
                        Grid.SetRow(gradeButton, i + 1);
                        grid.Children.Add(gradeButton);
                    }

                    else
                    {
                        var gradeInput = new Button();
                        gradeInput.FontSize = 25;
                        gradeInput.Width = 110;
                        gradeInput.Height = 45;

                        gradeInput.HorizontalAlignment = HorizontalAlignment.Center;
                        gradeInput.VerticalAlignment = VerticalAlignment.Center;
                        gradeInput.VerticalAlignment = VerticalAlignment.Center;
                        gradeInput.HorizontalAlignment = HorizontalAlignment.Center;
                        gradeInput.Content = "-";

                        Grid.SetColumn(gradeInput, j + 1);
                        Grid.SetRow(gradeInput, i + 1);
                        grid.Children.Add(gradeInput);
                    }


                }
            }
            grid.Margin = new Thickness(80, 80, 50, 50);
            // Add the Grid to your UI
            InfoPanel.Children.Add(grid);

            // After displaying all students for the selected class, add the "Add Announcement" button

        }
    }
}
