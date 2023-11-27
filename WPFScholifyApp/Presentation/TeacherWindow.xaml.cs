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
    using WPFScholifyApp.Presentation;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Interaction logic for TeacherWindow.xaml.
    /// </summary>
    public partial class TeacherWindow : Window
    {
        private AdminService adminService;
        private ParentsService parentsService;
        private TeacherService teacherService;
        private JournalService journalService;
        private GenericRepository<DayOfWeek> dayOfWeekRepository;
        private GenericRepository<Teacher> teacherRepository;
        private AdvertisementService advertisementService;
        private GenericRepository<DayBook> journalRepository;
        private GenericRepository<Class> classRepository;
        private GenericRepository<User> userRepository;
        private GenericRepository<DayBook> dayBookRepository;
        private GenericRepository<Subject> subjectRepository;
        private GenericRepository<Schedule> scheduleRepository;
        public int selectedClassId;
        public int selectedSubjectId;
        public int selectedPupilsId;
        private User CurrentUser;
        public List<DateTime> Days { get; set; }

        private GenericRepository<Parents> parentsRepository;

        public TeacherWindow(User currentUser, GenericRepository<DayOfWeek> dayOfWeekRepository, GenericRepository<Teacher> teacherRepository, JournalService journalService, GenericRepository<DayBook> journalRepository, GenericRepository<Class> classRepository, GenericRepository<User> userRepository, GenericRepository<DayBook> dayBookRepository, GenericRepository<Subject> subjectRepository, GenericRepository<Schedule> scheduleRepository, GenericRepository<Parents> parentsRepository)
        {
            this.dayBookRepository = dayBookRepository;
            this.journalService = journalService;
            this.journalRepository = journalRepository;
            this.dayOfWeekRepository = dayOfWeekRepository;
            this.teacherRepository = teacherRepository;
            this.classRepository = classRepository;
            this.userRepository = userRepository;
            this.subjectRepository = subjectRepository;
            this.scheduleRepository = scheduleRepository;
            this.parentsRepository = parentsRepository;
            this.parentsService = new ParentsService(new GenericRepository<User>(), new GenericRepository<Pupil>(), new GenericRepository<Parents>());
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
            this.adminService = new AdminService(
                new GenericRepository<User>(), 
                new GenericRepository<Class>(), 
                new GenericRepository<Teacher>(), 
                new GenericRepository<Pupil>(), 
                new GenericRepository<Admin>(), 
                new GenericRepository<Parents>(), 
                new GenericRepository<Subject>(), 
                new GenericRepository<Advertisement>());

            this.teacherService = new TeacherService( new GenericRepository<Advertisement>(), new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Schedule>());
            this.adminService = new AdminService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Advertisement>());
            this.advertisementService = new AdvertisementService(new GenericRepository<Advertisement>(), new GenericRepository<Class>(), new GenericRepository<Pupil>());
            this.InitializeComponent();
        }

        private void ParentsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.LeftPanel.Visibility = Visibility.Visible;
            this.RightPanel.Visibility = Visibility.Visible;
            this.RightAction.Visibility = Visibility.Visible;
            this.DeleteFromTeacherPanel();
            this.ShowAllPuplis();

        }

        public void ShowAllPuplis()
        {
            this.adminService = new AdminService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Advertisement>());
            var puplis = this.adminService.GetAllPupils();

            foreach (var p in puplis.OrderByDescending(x => x.LastName))
            {
                var teacherPanel = new StackPanel { Orientation = Orientation.Horizontal };
                var button = new Button { Content = $"{p!.LastName} {p!.FirstName}", Height = 60, Width = 300, FontSize = 30, Tag = p.Id };
                button.Click += new RoutedEventHandler(this.SpecificClassButton_ClickPupils);
                LeftPanel.Children.Add(button);
            }
            this.UpdateTeacherPanel();

        }
        public void SpecificClassButton_ClickPupils(object sender, RoutedEventArgs e)
        {
            var parentsButton = (Button)sender;
            this.ShowParentsForPupilId((int)parentsButton.Tag);
        }

        public void ShowParentsForPupilId(int pupilId)
        {
            this.DeleteFromTeacherPanel();
            this.ShowAllPuplis();
            this.selectedPupilsId = pupilId;
            this.parentsService = new ParentsService(new GenericRepository<User>(), new GenericRepository<Pupil>(), new GenericRepository<Parents>());

            var parents = this.parentsService.GetParentsForPupilId(pupilId);
            foreach (var f in parents)
            {
                var teacherPanel = new StackPanel { Orientation = Orientation.Horizontal };

                var button = new Button { Content = $" {f!.User!.LastName} {f!.User!.FirstName}", Height = 60, Width = 800, FontSize = 30, Tag = f.Id };
               button.Click += new RoutedEventHandler(this.LookParents);
                RightPanel.Children.Add(button);
                this.LeftPanel.Children.Add(teacherPanel);
            }

            this.UpdateTeacherPanel();
        }
        private void LookParents(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.SetSidePanelsVisible();

            this.DeleteFromTeacherPanel();

            var createButton = (Button)sender;
            var parentsId = (int)createButton.Tag;
            this.RightPanel.Children.Clear();
            this.RightAction.Children.Clear();


            ParentsService parentsService = new ParentsService(new GenericRepository<User>(), new GenericRepository<Pupil>(),new GenericRepository<Parents>() );
            var parents = this.adminService.GetAllParents().FirstOrDefault(x => x.Id == (int)createButton.Tag);
            if (parents != null)
            {
                TextBlock titleLabel = new TextBlock
                {
                    Text = "Приватна інформація",
                    FontSize = 50,
                    Foreground = new SolidColorBrush(Colors.DarkBlue),
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(100, 30, 0, 20),
                };
                this.RightPanel.Children.Add(titleLabel);
                TextBlock parentsInfo = new TextBlock
                {

                    Text = $"Ім'я:\t{parents.FirstName}\nПрізвище:\t {parents.LastName}\nПо батькові:\t{parents.MiddleName}\nСтать:\t{parents.Gender}" +
                        $"\nДата народження: {parents.Birthday:dd.MM.yyyy}\nАдреса:{parents.Address}\nТелефон:\t{parents.PhoneNumber}",
                    FontSize = 40,
                    Foreground = new SolidColorBrush(Colors.DarkBlue),
                    Margin = new Thickness(100, 70, 0, 40),
                };
                this.RightPanel.Children.Add(parentsInfo);
            }
            
            this.UpdateTeacherPanel();
            this.ShowAllPuplis();
         }

        public void ShowAllClassesParents()
        {
            this.DeleteFromTeacherPanel();
            this.RightAction.Children.Clear();

            var classes = this.adminService.GetAllClasses();

            foreach (var c in classes)
            {
                var button = new Button { Content = c.ClassName, Height = 60, Width = 350, FontSize = 30, Tag = c.Id };
                button.Click += new RoutedEventHandler(this.SpecificClassButton_Click1);
                this.LeftPanel.Children.Add(button);
            }
            this.UpdateTeacherPanel();

        }

        public void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Visible;
            this.SetSidePanelsHidden();
            ShowAllWeek();
            this.DeleteFromTeacherPanel();
        }

        public void AnnouncementsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.SetSidePanelsVisible();
            this.DeleteFromTeacherPanel();
            ShowAllClasses();
        }

        public void SpecificClassButton_Advertisement_Click(object sender, RoutedEventArgs e)
        {
            // Знайдемо ClassId з Tag кнопки, на яку ми натискали
            var classButton = (Button)sender;
            this.selectedClassId = (int)classButton.Tag;

            // Додамо кнопки з учнями
            this.ShowAllAdvertisementsForClassId(this.selectedClassId);
        }

        public void SpecificClassButton_Journal_Click(object sender, RoutedEventArgs e)
        {
            var classButton = (Button)sender;
            this.selectedSubjectId = (int)classButton.Tag;

            // Додамо кнопки з учнями
            this.ShowJournalForSubjectId(this.selectedSubjectId);
        }

        public void ShowAllPupilsForClassId(int classId)
        {
            this.DeleteFromTeacherPanel();
            //this.ShowAllClasses();
            ShowAllClassesParents();

            this.selectedClassId = classId;
            var pupils = this.adminService.GetAllPupilsForClass(classId);

            foreach (var p in pupils)
            {
                var pupilButton = new Button { Content = $"{p!.FirstName} {p!.LastName}", Height = 60, Width = 300, FontSize = 30, Tag = p.Id };
                pupilButton.Click += new RoutedEventHandler(this.LookPupils);
                this.RightPanel.Children.Add(pupilButton);
            }
            this.UpdateTeacherPanel();
        }

        public void SpecificClassButton_Click1(object sender, RoutedEventArgs e)
        {
            // Знайдемо ClassId з Tag кнопки, на яку ми натискали
            var classButton = (Button)sender;
            this.selectedClassId = (int)classButton.Tag;
            // Додамо кнопки з учнями
            this.ShowAllPupilsForClassId(this.selectedClassId);
        }
        public void ShowAllClasses(bool IsAdvertisement = false, bool IsJournal = false)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.SetSidePanelsVisible();

            this.DeleteFromTeacherPanel();

            var classes = this.adminService.GetAllClasses();

            foreach (var c in classes)
            {
                var button = new Button { Content = c.ClassName, Height = 60, Width = 350, FontSize = 30, Tag = c.Id };

                if (IsAdvertisement)
                {
                    button.Click += new RoutedEventHandler(this.SpecificClassButton_Advertisement_Click);
                }

                if (IsJournal)
                {
                    button.Click += new RoutedEventHandler(this.SpecificClassButton_Journal_Click);
                }
                this.LeftPanel.Children.Add(button);
            }

            this.UpdateTeacherPanel();
            
        }

        public void ShowAllSubjectsForTeacher()
        {
            this.Schedule.Visibility = Visibility.Hidden; 
            this.SetSidePanelsVisible();
            this.DeleteFromTeacherPanel();

            var subjects = this.subjectRepository.GetAllq()
                .Include(x => x.Teachers)
                .Include(x => x.Class)
                .Where(x => x.Teachers!.Select(y => y.Id).Contains(this.CurrentUser.Id));

            foreach (var s in subjects)
            {
                var button = new Button { Content = $"{s.SubjectName} {s.Class.ClassName}", Height = 60, Width = 350, FontSize = 30, Tag = s.Id };
                button.Click += new RoutedEventHandler(this.SpecificClassButton_Journal_Click);
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

        public void ShowJournalForSubjectId(int id)
        {
            this.DeleteFromTeacherPanel();
            this.ShowAllSubjectsForTeacher();
            var a = this.userRepository.GetAllq()
                .Include(x => x.Pupil)
                .ThenInclude(x => x.Class);

            var group = this.userRepository.GetAllq()
                .Include(x => x.Pupil)
                .ThenInclude(x => x.Class)
                .ThenInclude(x => x.Subjects)
                .Where(x => x.Pupil.Class.Subjects!.Select(y => y.Id).Contains(id)).ToList();
            var classId = this.classRepository.GetAllq().Include(x => x.Subjects).Where(x => x.Subjects!.Select(y => y.Id).Contains(id)).FirstOrDefault().Id;


            var grades = this.journalService.GetDayBooks(classId);
            var dates = this.scheduleRepository.GetAllq().AsNoTracking().Include(x => x.DayOfWeek).Where(x => x.SubjectId == id).Select(x => x.DayOfWeek).Distinct().ToList();


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
                    label.Content = "Учень";
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
                var user = group[i];

                var userLabel = new Label();
                userLabel.FontSize = 25;
                userLabel.Content = $"{user.LastName} {user.FirstName}";
                Grid.SetColumn(userLabel, 0);
                Grid.SetRow(userLabel, i + 1);
                grid.Children.Add(userLabel);

                for (int j = 0; j < dates.Count; j++)
                {
                    var date = dates[j];
                    var dayBook = grades.FirstOrDefault(x => x.Pupil.User.Id == user.Id && x.Schedule.DayOfWeek.Day.Equals(date.Day));

                    if (dayBook != null)
                    {
                        var gradeButton = new Button();
                        gradeButton.FontSize = 25;
                        gradeButton.Width = 110;
                        gradeButton.Height = 45;
                        gradeButton.Content = dayBook.Grade.ToString();
                        gradeButton.Tag = dayBook.Id;

                        gradeButton.Click += new RoutedEventHandler(this.DeleteGrade);
                        Grid.SetColumn(gradeButton, j + 1);
                        Grid.SetRow(gradeButton, i + 1);
                        grid.Children.Add(gradeButton);
                    }

                    else 
                    {
                        var gradeInput = new TextBox();
                        gradeInput.FontSize = 25;
                        gradeInput.Width = 110;
                        gradeInput.Height = 45;

                        gradeInput.VerticalAlignment = VerticalAlignment.Center;
                        gradeInput.HorizontalAlignment = HorizontalAlignment.Center;
                        gradeInput.Text = "";
                        gradeInput.Tag = new
                        {
                            DayOfWeek = date,
                            DayBook = new DayBook
                            {
                                Id = (this.dayBookRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1,
                                PupilId = (this.adminService.GetAllPupils().FirstOrDefault(x => x.Id == user.Id)?.Id ?? 0),
                                Grade = int.TryParse(gradeInput.Text, out int result)  == true  ? result : 0,
                                Attendance = !int.TryParse(gradeInput.Text, out int result2) == true ? gradeInput.Text : string.Empty,
                            }
                        };

                        gradeInput.KeyDown += this.AddGrade;
                        Grid.SetColumn(gradeInput, j + 1);
                        Grid.SetRow(gradeInput, i + 1);
                        grid.Children.Add(gradeInput);
                    }

                    
                }
            }
            grid.Margin = new Thickness(20, 20, 50, 50);
            // Add the Grid to your UI
            RightPanel.Children.Add(grid);

            // After displaying all students for the selected class, add the "Add Announcement" button
            this.UpdateTeacherPanel();
        }

        public void AddGrade(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var gradeInput = (TextBox)sender;
                var tag = (dynamic)gradeInput.Tag;
                var dayOfWeek = ((dynamic)gradeInput.Tag).DayOfWeek;
                var dayBook = ((dynamic)gradeInput.Tag).DayBook;
                dayBook.Grade = int.TryParse(gradeInput.Text, out int result) == true ? result : 0;
                dayBook.Attendance = !int.TryParse(gradeInput.Text, out int result2) == true ? gradeInput.Text : string.Empty;
                this.journalService.AddGrade(dayOfWeek, dayBook, this.selectedSubjectId);
                this.DeleteFromTeacherPanel();
                this.ShowJournalForSubjectId(this.selectedSubjectId);
                this.UpdateTeacherPanel();
            }
        }

        public void DeleteGrade(object sender, RoutedEventArgs e)
        {
            var gradeInput = (Button)sender;
            var dayBookId = (int)gradeInput.Tag;
            this.journalService.DeleteGrade(dayBookId);
            this.DeleteFromTeacherPanel();
            this.ShowJournalForSubjectId(this.selectedSubjectId);
            this.UpdateTeacherPanel();
        }

        private void LookAvertisement(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            this.RightPanel.Children.Clear();
            this.RightAction.Children.Clear();

            AdvertisementService advertisementService = new AdvertisementService(new GenericRepository<Advertisement>(), new GenericRepository<Class>(), new GenericRepository<Pupil>());
            var advertisement = this.advertisementService.GetAllAdvertisementsForClassId(this.selectedClassId).FirstOrDefault(x => x.Id == (int)createButton.Tag);
            if (advertisement != null)
            {
                TextBlock advertisementInfo = new TextBlock
                {

                    Text = $"Тема:\t {advertisement.Name}\n\n Вміст:\t {advertisement.Description}",
                    FontSize = 40,
                    Foreground = new SolidColorBrush(Colors.DarkBlue),
                    Margin = new Thickness(100, 90, 0, 20),
                };
                this.RightPanel.Children.Add(advertisementInfo);
            }

            this.UpdateTeacherPanel();
        }
        private void LookPupils(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            this.RightPanel.Children.Clear();
            this.RightAction.Children.Clear();

            AdvertisementService advertisementService = new AdvertisementService(new GenericRepository<Advertisement>(), new GenericRepository<Class>(), new GenericRepository<Pupil>());
            var advertisement = this.advertisementService.GetAllAdvertisementsForClassId(this.selectedClassId).FirstOrDefault(x => x.Id == (int)createButton.Tag);
            if (advertisement != null)
            {
                TextBlock advertisementInfo = new TextBlock
                {

                    Text = $"Тема:\t {advertisement.Name}\n\n Вміст:\t {advertisement.Description}",
                    FontSize = 40,
                    Foreground = new SolidColorBrush(Colors.DarkBlue),
                    Margin = new Thickness(100, 90, 0, 20),
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
            var createPanel = new CreateAdvertisements(new GenericRepository<Teacher>(), new GenericRepository<Advertisement>(), new GenericRepository<User>(), this, new GenericRepository<Class>(), new GenericRepository<Pupil>());
            createPanel.ClassId = this.selectedClassId;
            createPanel.Show();
        }
        private void PrivateInfoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.SetSidePanelsVisible();

            this.DeleteFromTeacherPanel();
            DeleteFromTeacherPanel();

            TextBlock titleLabel = new TextBlock
            {
                Text = "Приватна інформація",
                FontSize = 50,
                Foreground = new SolidColorBrush(Colors.DarkBlue),
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(100, 30, 0, 20),
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
                    Text = $"Ім'я:\t{teacher.FirstName}\nПрізвище:\t {teacher.LastName}\nПо батькові:\t {teacher.MiddleName}\nСтать:\t {teacher.Gender}" +
                        $"\nДата народження: {teacher.Birthday:dd.MM.yyyy}\nАдреса:\t {teacher.Address}\nТелефон:\t {teacher.PhoneNumber}",
                    FontSize = 40,
                    Foreground = new SolidColorBrush(Colors.DarkBlue),
                    Margin = new Thickness(100, 90, 0, 20),
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
            this.Schedule.Visibility = Visibility.Hidden;
            this.SetSidePanelsVisible();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }

        private void ClassJournalButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.SetSidePanelsVisible();

            this.ShowAllSubjectsForTeacher();

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

        public void SetSidePanelsVisible()
        {
            this.RightPanel.Visibility = Visibility.Visible;
            this.LeftPanel.Visibility = Visibility.Visible;
            this.RightAction.Visibility = Visibility.Visible;
        }

        public void SetSidePanelsHidden()
        {
            this.RightPanel.Visibility = Visibility.Hidden;
            this.LeftPanel.Visibility = Visibility.Hidden;
            this.RightAction.Visibility = Visibility.Hidden;
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
