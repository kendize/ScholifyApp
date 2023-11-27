// <copyright file="PupilWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using WPFScholifyApp.BLL;
    using WPFScholifyApp.DAL.ClassRepository;
    using WPFScholifyApp.DAL.DBClasses;
    using DayOfWeek = DAL.DBClasses.DayOfWeek;

    /// <summary>
    /// Interaction logic for PupilWindow.xaml.
    /// </summary>
    public partial class PupilWindow : Window
    {
        public List<DateTime> Days { get; set; }

        private IGenericRepository<Pupil> pupilRepository;
        private IGenericRepository<DayOfWeek> dayOfWeekRepository;
        private IGenericRepository<Class> classRepository;
        private IGenericRepository<DayBook> dayBookRepository;
        private IGenericRepository<Schedule> scheduleRepository;
        private IGenericRepository<Subject> subjectRepository;
        private PupilService pupilService;
        private User CurrentUser;
        private AdminService adminService;
        private UserService userService;
        private AdvertisementService advertisementService;
        private JournalService journalService;

        public PupilWindow(User currentUser, PupilService pupilService, GenericRepository<DayOfWeek> dayOfWeekRepository, GenericRepository<Pupil> pupilRepository, AdminService adminService, GenericRepository<Class> classRepository, JournalService journalService, GenericRepository<DayBook> dayBookRepository, GenericRepository<Schedule> scheduleRepository, GenericRepository<Subject> subjectRepository)

        {
            this.subjectRepository = subjectRepository;
            this.scheduleRepository = scheduleRepository;
            this.journalService = journalService;
            this.dayBookRepository = dayBookRepository;
            this.CurrentUser = currentUser;
            this.pupilService = new PupilService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Schedule>());
            this.adminService = adminService;
            this.userService = new UserService(new GenericRepository<User>(), new GenericRepository<Pupil>(), new GenericRepository<Parents>(), new GenericRepository<ParentsPupil>());
            this.dayOfWeekRepository = dayOfWeekRepository;
            this.advertisementService = new AdvertisementService(new GenericRepository<Advertisement>(), new GenericRepository<Class>(), new GenericRepository<Pupil>());
            this.pupilRepository = pupilRepository;
            this.classRepository = classRepository;

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
            this.pupilService = pupilService;
            this.CurrentUser = currentUser;
            this.dayOfWeekRepository = dayOfWeekRepository;
            this.CurrentUser = currentUser;
            this.pupilRepository = pupilRepository;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }

        private void PrivateInfoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.Panel.Visibility = Visibility.Visible;
            this.Panel.Children.Clear();
            DeleteFromPupilsPanel();

            TextBlock titleLabel = new TextBlock
            {
                Text = "Приватна інформація",
                FontSize = 50,
                Foreground = new SolidColorBrush(Colors.DarkBlue),
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(180, 30, 0, 10),
            };
            this.Panel.Children.Add(titleLabel);

            UserService userService = new UserService(new GenericRepository<User>(), new GenericRepository<Pupil>(), new GenericRepository<Parents>(), new GenericRepository<ParentsPupil>());
            string name = this.FirstNameTextBlock.Text;
            string surname = this.LastNameTextBlock.Text;
            User pupil = userService.GetInfoByNameSurname(name, surname);

            if (pupil != null)
            {
                TextBlock studentInfo = new TextBlock
                {
                    Text = $"Ім'я:\t\t {pupil.FirstName}\n\nПрізвище:\t {pupil.LastName}\n\nПо батькові:\t {pupil.MiddleName}\n\nСтать:\t\t {pupil.Gender}" +
                    $"\n\nДата народження: {pupil.Birthday:dd.MM.yyyy}\n\nАдреса:\t\t {pupil.Address}\n\nТелефон:\t {pupil.PhoneNumber}",
                    FontSize = 40,
                    Foreground = new SolidColorBrush(Colors.DarkBlue),
                    Margin = new Thickness(180, 90, 0, 10),
                };
                this.Panel.Children.Add(studentInfo);
            }
        }

        private void JournalButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.Panel.Visibility = Visibility.Visible;
            this.ShowJournalForUserId(CurrentUser.Id);
            this.UpdateDays();
        }
        public void ShowJournalForUserId(int id)
        {
            this.DeleteFromPupilsPanel();
            //this.ShowAllSubjectsForTeacher();
            var group2 = this.classRepository.GetAllq().Include(x => x.Pupils).Where(x => x.Pupils!.Select(y => y.Id).Contains(id)).ToList(); //   .Select(x => x.Pupils).ToList();
            var classId = this.classRepository.GetAllq().Include(x => x.Pupils).Where(x => x.Pupils!.Select(y => y.Id).Contains(id)).FirstOrDefault().Id;
            var group = this.subjectRepository.GetAll().Where(x => x.ClassId == classId).ToList();


            var grades = this.journalService.GetDayBooksForUserId(id);
            var dates = this.scheduleRepository.GetAllq().AsNoTracking().Include(x => x.Class).Where(x => x.ClassId == classId).Select(x => x.DayOfWeek).Distinct().ToList();


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
            Panel.Children.Add(grid);

            // After displaying all students for the selected class, add the "Add Announcement" button
            this.UpdatePupilsPanel();
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Visible;
            this.Panel.Visibility = Visibility.Hidden;
            this.ShowAllWeek();

            //DeleteFromPupilsPanel();
        }

        public void ShowAllWeek()
        {
            ClearDays();
            var classId = this.pupilService.GetAddPupils().FirstOrDefault(x => x.Id == this.CurrentUser.Id)!.ClassId;
            var result = new List<Schedule>();
            var dayOfWeeks = this.dayOfWeekRepository.GetAll();
            for (int i = 0; i <= 6; i++)
            {
                
                var dayOfWeek = dayOfWeeks.FirstOrDefault(x => x.Date.AddDays(1).Date.Equals(this.Days[i].Date));
                var dayOfWeekId = dayOfWeek != null ? dayOfWeek!.Id : 0;
                var schedulesForDay = this.pupilService.GetAllSchedules(classId, dayOfWeekId).ToList();
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
                if (i ==1)
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
                if (i ==2)
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

        
        private void AnnouncementsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.Panel.Visibility = Visibility.Visible;
            DeleteFromPupilsPanel();
            var classId = this.pupilRepository.GetAllq().Include(x => x.Class).FirstOrDefault(x => x.Id == CurrentUser.Id)!.ClassId;
            AdvertisementService advertisementService = new AdvertisementService(new GenericRepository<Advertisement>(), new GenericRepository<Class>(), new GenericRepository<Pupil>());
            var advertisements = this.advertisementService.GetAdvertisementsForClassId(classId);
            foreach (var advertisement in advertisements)
            {
                if (advertisement != null)
                {
                    string advertisementText = $"Тема:\t {advertisement.Name}\n\n Вміст:\t {advertisement.Description}";
                    List<string> lines = new List<string>();
                    int charactersPerLine = 110;
                    for (int i = 0; i < advertisementText.Length; i += charactersPerLine)
                    {
                        int length = Math.Min(charactersPerLine, advertisementText.Length - i);
                        lines.Add(advertisementText.Substring(i, length));
                    }

                    string wrappedText = string.Join(Environment.NewLine, lines);
                    TextBlock advertisementInfo = new TextBlock
                    {
                        Text = wrappedText,
                        FontSize = 30,
                        Foreground = new SolidColorBrush(Colors.DarkBlue),
                        Margin = new Thickness(90, 80, 0, 5),
                    };
                    this.Panel.Children.Add(advertisementInfo);
                }
            }

            UpdatePupilsPanel();
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

        public void DeleteFromPupilsPanel()
        {
            this.Panel.Children.Clear();
        }

        public void UpdatePupilsPanel()
        {
            this.Panel.UpdateLayout();
        }
    }
}

