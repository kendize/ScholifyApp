// <copyright file="AdminWindow.xaml.cs" company="PlaceholderCompany">
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
    using DayOfWeek = DAL.DBClasses.DayOfWeek;

    /// <summary>
    /// Interaction logic for AdminWindow.xaml.
    /// </summary>
    public partial class AdminWindow : Window
    {
        public User? _authenticatedUser { get; set; }
        private AdminService adminService;
        private AdvertisementService advertisementService;
        private ClassService classService;
        private DayOfWeekService dayOfWeekService;
        private JournalService journalService;
        private ParentsService parentsService;
        private PupilService pupilService;
        private UserService userService;
        private ScheduleService scheduleService;
        private SubjectService subjectService;
        private TeacherService teacherService;
        private WindowService windowService;

        private MainWindow mainWindow;
        private LookUsers lookUsers;
        private CreateParents createParents;
        private CreateClass createClass;
        private CreateUser createUser;
        private CreateTeacher createTeacher;
        private CreateSubject createSubject;
        private CreateSchedule createSchedule1;
        private int selectedClassId;
        private int selectedTeacherId;
        private int selectedSubjectId;
        private int selectedPupilsId;

        public List<DateTime> Days { get; set; }

        // private int selectedPupilId;
        // private int selectedParentId;
        public AdminWindow(
                            AdminService adminService,
                            AdvertisementService advertisementService,
                            ClassService classService,
                            DayOfWeekService dayOfWeekService,
                            JournalService journalService,
                            ParentsService parentsService,
                            PupilService pupilService,
                            UserService userService,
                            ScheduleService scheduleService,
                            SubjectService subjectService,
                            TeacherService teacherService,
                            WindowService windowService,

                            MainWindow mainWindow,
                            LookUsers lookUsers,
                            CreateParents createParents,
                            CreateClass createClass,
                            CreateUser createUser,
                            CreateTeacher createTeacher,
                            CreateSubject createSubject,
                            CreateSchedule createSchedule1
                            )
        {

            this.adminService = adminService;
            this.advertisementService = advertisementService;
            this.classService = classService;
            this.dayOfWeekService = dayOfWeekService;
            this.journalService = journalService;
            this.parentsService = parentsService;
            this.userService = userService;
            this.scheduleService = scheduleService;
            this.subjectService = subjectService;
            this.teacherService = teacherService;
            this.windowService = windowService;
            this.pupilService = pupilService;

            this.mainWindow = mainWindow;
            this.lookUsers = lookUsers;
            this.createParents = createParents;
            this.createClass = createClass;
            this.createUser = createUser;
            this.createTeacher = createTeacher;
            this.createSubject = createSubject;
            this.createSchedule1 = createSchedule1;

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
            this.Closing += new CancelEventHandler(this.Window_Closing!);
            this.InitializeComponent();
        }

        public void Window_Closing(object sender, CancelEventArgs e)
        {
            this.Hide();
            this.mainWindow.Show();
        }
        // Метод який викликається при натисканні кнопки "Класи" на панелі Адміністратора
        public void ClassButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.SetSidePanelsVisible();
            this.selectedClassId = 0;
            this.ShowAllClasses();
        }

        // Метод який викликається при натисканні кнопки "Вчителі" на панелі Адміністратора
        public void TeachersButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.SetSidePanelsVisible();
            this.selectedTeacherId = 0;
            this.ShowAllTeachers();
        }

        // Метод який викликається при натисканні кнопки "Батьки" на панелі Адміністратора
        private void ParentsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.SetSidePanelsVisible();
            this.ShowAllPuplis();
        }

        // Метод який викликається при натисканні кнопки "Розклад" на панелі Адміністратора
        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Hidden;
            this.SetSidePanelsVisible();
            
            ShowAllClasses(true);
        }




        // ----------------------------------------------------------------------------------
        // Розклад
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

            this.ShowAllWeek(this.selectedClassId);
        }

        public void ShowAllWeek(int classId)
        {
            ClearDays();
            var result = new List<Schedule>();
            var dayOfWeeks = this.dayOfWeekService.GetAll();
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


        //

        public void ShowAllPuplis()
        {
            this.DeleteFromAdminPanels();
            var puplis = this.adminService.GetAllPupils();

            foreach (var p in puplis.OrderByDescending(x => x.LastName))
            {
                var teacherPanel = new StackPanel { Orientation = Orientation.Horizontal };
                var button = new Button { Content = $"{p!.LastName} {p!.FirstName}", Height = 60, Width = 290, FontSize = 30, Tag = p.Id };
                button.Click += new RoutedEventHandler(this.SpecificClassButton_ClickPupils);
                LeftPanel.Children.Add(button);
            }
            this.UpdateAdminPanels();
        }

        // Метод для виведення списку кнопок з усіма класами для предметів
        public void ShowAllClasses(bool isSchedule = false)
        {
            this.selectedSubjectId = 0;
            this.DeleteFromAdminPanels();

            var classes = this.adminService.GetAllClasses();

            foreach (var c in classes)
            {
                var teacherPanel = new StackPanel { Orientation = Orientation.Horizontal };

                var button = new Button { Content = $"{c!.ClassName}", Height = 60, Width = 290, FontSize = 30, Tag = c.Id };
                if (isSchedule)
                {
                    button.Click += new RoutedEventHandler(this.SpecificClassButton_Schedule_Click);
                }
                else
                {
                    button.Click += new RoutedEventHandler(this.SpecificClassButton_Click);
                }

                teacherPanel.Children.Add(button);

                if (!isSchedule)
                {
                    var deleteButton = new Button { Content = $"Del", Height = 60, Width = 70, FontSize = 30, Tag = c.Id, Margin = new Thickness(10, 0, 0, 0) };
                    deleteButton.Click += new RoutedEventHandler(DeleteClass);
                    teacherPanel.Children.Add(deleteButton);
                }


                this.LeftPanel.Children.Add(teacherPanel);
            }

            if (!isSchedule)
            {
                var createButton = new Button
                {
                    Content = "Створити Клас",
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Height = 60,
                    Width = 300,
                    FontSize = 30,
                };
                createButton.Click += new RoutedEventHandler(this.AddClass);
                this.LeftAction.Children.Add(createButton);
            }


            this.UpdateAdminPanels();
        }
        // Метод для виведення списку кнопок з усіма вчителями
        public void ShowAllTeachers()
        {
            this.DeleteFromAdminPanels();
            var teacher = this.adminService.GetAllTeacher();
            foreach (var t in teacher)
            {
                var teacherPanel = new StackPanel { Orientation = Orientation.Horizontal };

                var button = new Button { Content = $"{t!.FirstName} {t!.LastName}", Height = 60, Width = 290, FontSize = 30, Tag = t.Id };
                button.Click += new RoutedEventHandler(this.SpecificTeacherButton_Click);
                teacherPanel.Children.Add(button);

                var lookButton = new Button { Content = "U", Height = 60, Width = 30, FontSize = 30, Tag = t.Id, Margin = new Thickness(10, 0, 0, 0) };
                lookButton.Click += new RoutedEventHandler(this.LookTeacher);
                teacherPanel.Children.Add(lookButton);

                var deleteButton = new Button { Content = $"Del", Height = 60, Width = 60, FontSize = 30, Tag = t.Id, Margin = new Thickness(2, 0, 0, 0) };
                deleteButton.Click += new RoutedEventHandler(this.DeleteTeacher);
                teacherPanel.Children.Add(deleteButton);

                this.LeftPanel.Children.Add(teacherPanel);
            }

            var createButton = new Button { Content = "Додати Вчителя", Height = 60, Width = 300, FontSize = 30, };
            createButton.Click += new RoutedEventHandler(this.AddTeacher);
            this.LeftAction.Children.Add(createButton);

            this.UpdateAdminPanels();
        }
        // Метод для виведення списку кнопок з усіма Предметами
        public void ShowAllSubjects(int classId)
        {

            this.DeleteFromAdminPanels();

            var subjects = this.subjectService.GetSubjectsByClassId(classId);

            foreach (var s in subjects)
            {
                var button = new Button { Content = $"{s.SubjectName} {s.Class!.ClassName}", Height = 60, Width = 400, FontSize = 30, Tag = s.Id };
                button.Click += new RoutedEventHandler(this.SpecificSubjectButton_Click);
                this.LeftPanel.Children.Add(button);
            }

            this.UpdateAdminPanels();
        }

        //----------------------------

        public void SpecificClassButton_ClickPupils(object sender, RoutedEventArgs e)
        {
            var parentsButton = (Button)sender;
            this.ShowParentsForPupilId((int)parentsButton.Tag);
          }

        public void ShowParentsForPupilId(int pupilId)
        {
            this.DeleteFromAdminPanels();
            this.ShowAllPuplis();
            this.selectedPupilsId = pupilId;

            var parents = this.parentsService.GetParentsForPupilId(pupilId);
            foreach (var f in parents)
            {
                //var teacherPanel = new StackPanel { Orientation = Orientation.Horizontal };

                var button = new Button { Content = $" {f!.LastName} {f!.FirstName}", Height = 60, Width = 500, FontSize = 30, Tag = f.Id };
                button.Click += new RoutedEventHandler(this.LookParents);
                RightPanel.Children.Add(button);

                var deleteButton = new Button { Content = $"Видалити", Height = 60, Width = 500, FontSize = 30, Tag = f.Id, Margin = new Thickness(0, 0, 0, 0) };
                deleteButton.Click += new RoutedEventHandler(this.DeleteParents);
                RightPanel.Children.Add(deleteButton);

                //this.LeftPanel.Children.Add(teacherPanel);
            }

            var createButton = new Button { Content = "Додати Батьків", Height = 60, Width = 300, FontSize = 30, };
            createButton.Click += new RoutedEventHandler(this.AddParents);
            this.RightAction.Children.Add(createButton);

            this.UpdateAdminPanels();
        }

        private void DeleteParents(object sender, RoutedEventArgs e)
        {
            var deleteButton = (Button)sender;
            this.userService.DeleteParent((int)deleteButton.Tag);
            this.DeleteFromAdminPanels();
            this.ShowAllPuplis();
            this.ShowParentsForPupilId(this.selectedPupilsId);
            this.UpdateAdminPanels();
        }

        private void LookParents(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            var parentsId = (int)createButton.Tag;

            this.windowService.Show<LookUsers>(window =>
            {
                window.currentPupilId = this.selectedPupilsId;
                var parent = this.adminService.GetAllParents().FirstOrDefault(x => x.Id == (int)createButton.Tag);

                if (parent != null)
                {
                    window.currentUser = parent;
                    window.Email.Text = parent.Email?.ToString() ?? string.Empty;
                    window.Password.Text = parent.Password?.ToString() ?? string.Empty;
                    window.FirstName.Text = parent.FirstName?.ToString() ?? string.Empty;
                    window.LastName.Text = parent.LastName?.ToString() ?? string.Empty;
                    window.MiddleName.Text = parent.MiddleName?.ToString() ?? string.Empty;
                    window.Gender.Text = parent.Gender?.ToString() ?? string.Empty;
                    window.Birthday.Text = parent.Birthday?.ToString() ?? string.Empty;
                    window.Adress.Text = parent.Address?.ToString() ?? string.Empty;
                    window.PhoneNumber.Text = parent.PhoneNumber?.ToString() ?? string.Empty;
                }

                window.Show();
            });

            this.LeftPanel.UpdateLayout();

        }
        private void AddParents(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            var createPanel = this.createParents;
            createPanel.PupilsId = this.selectedPupilsId;
            createPanel.Show();
        }
        
        
        public void DeleteClass(object sender, RoutedEventArgs e)
        {
            var deleteButton = (Button)sender;
            this.selectedClassId = (int)deleteButton.Tag;
            this.classService.Delete((int)deleteButton.Tag);
            this.UpdateAdminPanels();
            this.DeleteFromAdminPanels();
            this.ShowAllClasses();

        }
        
        public void SpecificClassButton_Schedule_Click(object sender, RoutedEventArgs e)
        {
            this.Schedule.Visibility = Visibility.Visible;
            this.SetSidePanelsHidden();
            var button =(Button)sender;
            var tag = (int)button.Tag;
            this.selectedClassId = tag;
            this.ShowAllWeek(this.selectedClassId);
            //this.ShowAllSubjects(tag);
        }

        

        //----


        // Метод який викликається при натисканні кнопки обраного класу серед списку класів на панелі Адміністратора (Перегляд Учнів)
        public void SpecificClassButton_Click(object sender, RoutedEventArgs e)
        {
            // Знайдемо ClassId з Tag кнопки, на яку ми натискали
            var classButton = (Button)sender;
            this.selectedClassId = (int)classButton.Tag;

            // Додамо кнопки з учнями
            this.ShowAllPupilsForClassId(this.selectedClassId);
        }

        // Метод який викликається при натисканні кнопки обраного вчителя серед списку вчителів на панелі Адміністратора
        public void SpecificTeacherButton_Click(object sender, RoutedEventArgs e)
        {
            var teacherButton = (Button)sender;
            this.selectedTeacherId = (int)teacherButton.Tag;
            this.ShowAllSubjectsForTeacher(this.selectedTeacherId);
        }


        // Метод який викликається при натисканні кнопки обраного предмету серед списку предметів на панелі Адміністратора
        public void SpecificSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            var subjectButton = (Button)sender;
            this.selectedSubjectId = (int)subjectButton.Tag;
            ShowAllSchedulesForSubject(this.selectedSubjectId);
        }

        // Метод для виведення списку кнопок з усіма учнями для обраного класу
        public void ShowAllPupilsForClassId(int classId)
        {
            this.DeleteFromAdminPanels();
            this.ShowAllClasses();
            this.selectedClassId = classId;
            var pupils = this.adminService.GetAllPupilsForClass(classId);

            foreach (var p in pupils)
            {
                var pupilButton = new Button { Content = $"{p!.FirstName} {p!.LastName}", Height = 60, Width = 700, FontSize = 30, Tag = p.Id };
                pupilButton.Click += new RoutedEventHandler(this.LookUsers);
                var deleteButton = new Button { Content = $"Delete {p!.FirstName} {p!.LastName}", Height = 60, Width = 700, FontSize = 30, Tag = p.Id };
                deleteButton.Click += new RoutedEventHandler(this.DeletePupil);
                this.RightPanel.Children.Add(pupilButton);
                this.RightPanel.Children.Add(deleteButton);
            }

            // Після виведення всіх учнів для обраного класу додамо кнопку "Додати Учня"
            var createButton = new Button { Content = "Додати Учня", Height = 60, Width = 700, FontSize = 30, Tag = classId };
            createButton.Click += new RoutedEventHandler(this.AddPupil);
            this.RightAction.Children.Add(createButton);
            this.UpdateAdminPanels();
        }

      
        // Метод для виведення списку кнопок з усіма предметами для обраного викладача
        public void ShowAllSubjectsForTeacher(int teacherId)
        {
            this.DeleteFromAdminPanels();
            this.ShowAllTeachers();

            this.selectedTeacherId = teacherId;
            var subjects = this.adminService.GetAllSubjectsForTeacher(teacherId);
            foreach (var p in subjects)
            {
                var subjectButton = new Button { Content = $"{p!.SubjectName}", Height = 60, Width = 300, FontSize = 30, };
                var deleteButton = new Button { Content = $"Delete {p!.SubjectName}", Height = 60, Width = 300, FontSize = 30, Tag = p.Id };
                deleteButton.Click += new RoutedEventHandler(this.DeleteSubject);

                this.RightPanel.Children.Add(subjectButton);
                this.RightPanel.Children.Add(deleteButton);
            }

            var createButton = new Button { Content = "Додати предмет викладачу", Height = 60, Width = 700, FontSize = 30, Tag = teacherId };
            createButton.Click += new RoutedEventHandler(this.AddSubjectToTeacher);

            this.RightAction.Children.Add(createButton);

            this.UpdateAdminPanels();
        }

        // Метод для виведення списку розкладу з усіма предметами для обраного предмета
        public void ShowAllSchedulesForSubject(int subjectId)
        {
            DeleteFromAdminPanels();
            var classId = this.classService.GetClassBySubjectId(subjectId).Id;
            ShowAllSubjects(classId);
            var schedules = this.scheduleService.GetAllSchedulesForSubjectId(subjectId);
            foreach (var schedule in schedules)
            {
                var button = new Button { Content = $"{schedule.Subject!.SubjectName} {schedule.Class!.ClassName} {schedule.DayOfWeek!.Date.AddDays(1).ToString("d")} {schedule.LessonTime!.StartTime.ToString("HH:mm")} - {schedule.LessonTime!.EndTime.ToString("HH:mm")}", Height = 60, Width = 700, FontSize = 30 };
                var deleteButton = new Button { Content = "Видалити", Height = 60, Width = 700, FontSize = 3, Tag = schedule.Id };
                deleteButton.Click += new RoutedEventHandler(this.DeleteSchedule);
                this.RightPanel.Children.Add(button);
                this.RightPanel.Children.Add(deleteButton);
            }

            var createButton = new Button { Content = "Додати предмет в розклад", Height = 60, Width = 700, FontSize = 30, Tag = classId };
            createButton.Click += new RoutedEventHandler(this.СreateSchedule);
            this.RightAction.Children.Add(createButton);
            UpdateAdminPanels();
        }



        // Метод який викликається при натисканні кнопки "Додати Клас"
        private void AddClass(object sender, RoutedEventArgs e)
        {
            var subjectButton = (Button)sender;
            var createPanel = this.createClass;
            createPanel.Show();
            this.LeftPanel.UpdateLayout(); // воно не робе
        }

        // Метод який викликається при натисканні кнопки "Додати Учня"
        private void AddPupil(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            var createPanel = this.createUser;
            createPanel.ClassId = this.selectedClassId;
            createPanel.Show();
        }

        // Метод який викликається при натисканні кнопки "Додати Вчителя"
        private void AddTeacher(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            var createPanel = this.createTeacher;
            createPanel.Show();
        }
        
        // Метод який викликається при натисканні кнопки "Додати Предмет"
        private void AddSubjectToTeacher(object sender, RoutedEventArgs e)
        {
            var subjectButton = (Button)sender;
            var createPanel = this.createSubject;
            createPanel.TeacherId = this.selectedTeacherId;
            createPanel.Show();
            this.RightPanel.UpdateLayout();
            this.RightAction.UpdateLayout();
        }

        // Метод який викликається при натисканні кнопки "Додати Розклад"
        private void СreateSchedule(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            this.windowService.Show<CreateSchedule>(window =>
            {
                window.InitializeComponent();
                object tag = createButton.Tag;
                if (tag != null && int.TryParse(tag.ToString(), out int classId))
                {
                window.clas = this.classService.GetAllClasses().FirstOrDefault(x => x.Id == classId); //) this.subjectRepository.GetAllq().Include(x => x.Class).FirstOrDefault(x => x.ClassId == classId)?.Class;
                }
            });
        }

        // Метод який викликається при натисканні кнопки "Видалити Учня"
        private void DeletePupil(object sender, RoutedEventArgs e)
        {
            var deleteButton = (Button)sender;
            this.userService.DeletePupil((int)deleteButton.Tag);
            this.RightPanel.Children.Clear();
            this.LeftPanel.Children.Clear();
            this.ShowAllClasses();
            this.ShowAllPupilsForClassId(this.selectedClassId);
            this.RightPanel.UpdateLayout();
            this.LeftPanel.UpdateLayout();
        }

        // Метод який викликається при натисканні кнопки "Видалити Вчителя"
        private void DeleteTeacher(object sender, RoutedEventArgs e)
        {
            var deleteButton = (Button)sender;
            this.userService.DeletePupil((int)deleteButton.Tag);
            this.DeleteFromAdminPanels();
            this.ShowAllTeachers();
        }
        
        // Метод який викликається при натисканні кнопки "Видалити Предмет"
        private void DeleteSubject(object sender, RoutedEventArgs e)
        {
            var deleteButton = (Button)sender;
            this.teacherService.Delete(this.teacherService.GetTeacherBySubjectId((int)deleteButton.Tag)!.Id);
            this.subjectService.Delete((int)deleteButton.Tag);
            this.RightPanel.UpdateLayout();
            this.RightPanel.Children.Clear();
            this.RightAction.Children.Clear();
            this.ShowAllSubjectsForTeacher(this.selectedTeacherId);
        }

        // Метод який викликається при натисканні кнопки "Видалити Розклад"
        public void DeleteSchedule(object sender, RoutedEventArgs e)
        {
            var deleteButton = (Button)sender;
            this.DeleteFromAdminPanels();
            this.scheduleService.Delete((int)deleteButton.Tag);
            this.DeleteFromAdminPanels();

            //var subjectId = this.subjectRepository.GetAll().FirstOrDefault(x => x.Id == (int)deleteButton.Tag).Id;

            //var classId = this.subjectRepository.GetAll().FirstOrDefault(x => x.Id == subjectId).ClassId;
            var subjectId = this.subjectService.GetSubjectById((int)deleteButton.Tag).Id;

            var classId = this.classService.GetClassBySubjectId(subjectId).Id;
            ShowAllSubjects(classId);
            this.ShowAllSchedulesForSubject(this.selectedSubjectId);
            this.UpdateAdminPanels();
        }

        // Методи оновлення дисплею
        private void LookTeacher(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            this.windowService.Show<LookUsers>(window =>
            {
                window.currentClassId = this.selectedClassId;

                window.currentUser = this.adminService.GetAllTeacher().FirstOrDefault(x => x.Id == (int)createButton.Tag);
                var teacher = this.adminService.GetAllTeacher().FirstOrDefault(x => x.Id == (int)createButton.Tag);
                window.Email.Text = teacher!.Email?.ToString();
                window.Password.Text = teacher!.Password?.ToString();
                window.FirstName.Text = teacher!.FirstName?.ToString();
                window.LastName.Text = teacher!.LastName?.ToString();
                window.MiddleName.Text = teacher!.MiddleName?.ToString();
                window.Gender.Text = teacher!.Gender?.ToString();
                window.Birthday.Text = teacher!.Birthday?.ToString();
                window.Adress.Text = teacher!.Address?.ToString();
                window.PhoneNumber.Text = teacher!.PhoneNumber?.ToString();
                window.InitializeComponent();
            });

            //var createPanel = new LookTeacher(new GenericRepository<User>(), new GenericRepository<Teacher>(), this);

            this.LeftPanel.UpdateLayout(); 
        }

        private void LookUsers(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            this.windowService.Show<LookUsers>(window =>
            {
                window.ShowAllClasses = true;
                window.currentClassId = this.selectedClassId;
                var pupils = this.adminService.GetAllPupils().FirstOrDefault(x => x.Id == (int)createButton.Tag);

                if (pupils != null)
                {
                    window.currentUser = pupils;
                    window.Email.Text = pupils.Email?.ToString() ?? string.Empty;
                    window.Password.Text = pupils.Password?.ToString() ?? string.Empty;
                    window.FirstName.Text = pupils.FirstName?.ToString() ?? string.Empty;
                    window.LastName.Text = pupils.LastName?.ToString() ?? string.Empty;
                    window.MiddleName.Text = pupils.MiddleName?.ToString() ?? string.Empty;
                    window.Gender.Text = pupils.Gender?.ToString() ?? string.Empty;
                    window.Birthday.Text = pupils.Birthday?.ToString() ?? string.Empty;
                    window.Adress.Text = pupils.Address?.ToString() ?? string.Empty;
                    window.PhoneNumber.Text = pupils.PhoneNumber?.ToString() ?? string.Empty;
                }
            });

            this.LeftPanel.UpdateLayout();
        }


        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.windowService.Show<MainWindow>(window =>
            {
                window.InitializeComponent();
                window.Show();
            });
            this.Hide();

        }


        // Методи для оновлення вмісту вікна
        public void DeleteFromAdminPanels()
        {
            this.RightPanel.Children.Clear();
            this.LeftPanel.Children.Clear();
            this.RightAction.Children.Clear();
            this.LeftAction.Children.Clear();
        }

        public void UpdateAdminPanels()
        {
            this.RightPanel.UpdateLayout();
            this.LeftPanel.UpdateLayout();
            this.RightAction.UpdateLayout();
            this.LeftAction.UpdateLayout();
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
