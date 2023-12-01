// <copyright file="AdminWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
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
        private PupilService pupilService;
        private AdminService adminService;
        private ParentsService parentsService;
        private UserService userService;
        private ScheduleService scheduleService;
        private GenericRepository<User> userRepository;
        private GenericRepository<Pupil> pupilRepository;
        private GenericRepository<Subject> subjectRepository;
        private GenericRepository<Teacher> teacherRepository;
        private GenericRepository<Class> classRepository;
        private GenericRepository<Schedule> scheduleRepository;
        private GenericRepository<DayOfWeek> dayOfWeekRepository;
        private GenericRepository<LessonTime> lessonTimeRepository;
        private GenericRepository<Parents> parentsRepository;
        private TeacherService teacherService;
        private AdvertisementService advertisementService;
        private int selectedClassId;
        private int selectedTeacherId;
        private int selectedSubjectId;
        private int selectedPupilsId;

        private IGenericRepository<Advertisement> advertisementsRepository;
        private User? CurrentUser { get; set; }
        public List<DateTime> Days { get; set; }

        // private int selectedPupilId;
        // private int selectedParentId;
        public AdminWindow(User CurrentUser)
        {

            this.pupilService = new PupilService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Schedule>());
            this.scheduleRepository = new GenericRepository<Schedule>();
            this.dayOfWeekRepository = new GenericRepository<DayOfWeek>();
            this.lessonTimeRepository = new GenericRepository<LessonTime>();
            this.CurrentUser = CurrentUser;
            this.parentsRepository = new GenericRepository<Parents>();
            this.userRepository = new GenericRepository<User>();
            this.subjectRepository = new GenericRepository<Subject>();
            this.pupilRepository = new GenericRepository<Pupil>();
            this.teacherRepository = new GenericRepository<Teacher>();
            this.classRepository = new GenericRepository<Class>();
            this.advertisementsRepository = new GenericRepository<Advertisement>();
            this.userService = new UserService(new GenericRepository<User>(), new GenericRepository<Pupil>(), new GenericRepository<Parents>(), new GenericRepository<ParentsPupil>());
            this.parentsService = new ParentsService(new GenericRepository<User>(), new GenericRepository<Pupil>(), new GenericRepository<Parents>());
            this.scheduleService = new ScheduleService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Schedule>(), new GenericRepository<Subject>());
            this.adminService = new AdminService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Advertisement>());
            this.InitializeComponent();
            this.teacherService = new TeacherService(new GenericRepository<Advertisement>(), new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Schedule>());
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
            this.CurrentUser = CurrentUser;
            this.adminService = new AdminService(
                new GenericRepository<User>(),
                new GenericRepository<Class>(),
                new GenericRepository<Teacher>(),
                new GenericRepository<Pupil>(),
                new GenericRepository<Admin>(),
                new GenericRepository<Parents>(),
                new GenericRepository<Subject>(),
                new GenericRepository<Advertisement>());

            this.advertisementService = new AdvertisementService(new GenericRepository<Advertisement>(), new GenericRepository<Class>(), new GenericRepository<Pupil>());
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
            this.adminService = new AdminService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Advertisement>());
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
            this.adminService = new AdminService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Advertisement>());
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

            var subjects = this.subjectRepository.GetAllq()
                .Include(x => x.Class).Where(x => x.ClassId == classId).ToList();

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
            this.parentsService = new ParentsService(new GenericRepository<User>(), new GenericRepository<Pupil>(), new GenericRepository<Parents>());

            var parents = this.parentsService.GetParentsForPupilId(pupilId);
            foreach (var f in parents)
            {
                //var teacherPanel = new StackPanel { Orientation = Orientation.Horizontal };

                var button = new Button { Content = $" {f!.User!.LastName} {f!.User!.FirstName}", Height = 60, Width = 500, FontSize = 30, Tag = f.UserId };
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
            var createPanel = new LookUsers(new GenericRepository<User>(), new GenericRepository<Pupil>(), this, new GenericRepository<Parents>(), new GenericRepository<ParentsPupil>());

            //var createPanel = new LookParents(new GenericRepository<User>(), new GenericRepository<Parents>(), new GenericRepository<Pupil>(), this, new GenericRepository<ParentsPupil>());
            //  createPanel.ShowAllClasses = true;

            createPanel.currentPupilId = this.selectedPupilsId;
            var parent = this.adminService.GetAllParents().FirstOrDefault(x => x.Id == (int)createButton.Tag);
            createPanel.currentUser = parent;
            createPanel.Email.Text = parent!.Email!.ToString();
            createPanel.Password.Text = parent!.Password!.ToString();
            createPanel.FirstName.Text = parent!.FirstName!.ToString();
            createPanel.LastName.Text = parent!.LastName!.ToString();
            createPanel.MiddleName.Text = parent!.MiddleName!.ToString();
            createPanel.Gender.Text = parent!.Gender!.ToString();
            createPanel.Birthday.Text = parent!.Birthday!.ToString();
            createPanel.Adress.Text = parent!.Address!.ToString();
            createPanel.PhoneNumber.Text = parent!.PhoneNumber!.ToString();

            createPanel.Show();
            this.LeftPanel.UpdateLayout();

        }
        private void AddParents(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            var createPanel = new CreateParents(this.userRepository, this.parentsRepository, this.pupilRepository, this, new GenericRepository<ParentsPupil>());
            createPanel.PupilsId = this.selectedPupilsId;
            createPanel.Show();
        }
        
        
        public void DeleteClass(object sender, RoutedEventArgs e)
        {
            var deleteButton = (Button)sender;
            this.selectedClassId = (int)deleteButton.Tag;
            this.classRepository.Delete((int)deleteButton.Tag);
            this.classRepository.Save();
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
            this.userRepository = new GenericRepository<User>(); // Re-initialize the repository
            this.pupilRepository = new GenericRepository<Pupil>();
            this.adminService = new AdminService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>(), new GenericRepository<Advertisement>());
        }

      
        // Метод для виведення списку кнопок з усіма предметами для обраного викладача
        public void ShowAllSubjectsForTeacher(int teacherId)
        {
            this.DeleteFromAdminPanels();
            this.ShowAllTeachers();

            this.selectedTeacherId = teacherId;
            // this.adminService = new AdminService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>());
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
            var classId = this.subjectRepository.GetAll().FirstOrDefault(x => x.Id == subjectId).ClassId;
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
            var createPanel = new CreateClass(this.classRepository, this);
            createPanel.Show();
            this.LeftPanel.UpdateLayout(); // воно не робе
        }

        // Метод який викликається при натисканні кнопки "Додати Учня"
        private void AddPupil(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            var createPanel = new CreateUser(this.userRepository, this.pupilRepository, this);
            createPanel.ClassId = this.selectedClassId;
            createPanel.Show();
        }

        // Метод який викликається при натисканні кнопки "Додати Вчителя"
        private void AddTeacher(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            var createPanel = new CreateTeacher(this.userRepository, this);
            createPanel.Show();
        }
        
        // Метод який викликається при натисканні кнопки "Додати Предмет"
        private void AddSubjectToTeacher(object sender, RoutedEventArgs e)
        {
            var subjectButton = (Button)sender;
            var createPanel = new CreateSubject(this.teacherRepository, this.subjectRepository, this.userRepository, this, this.classRepository);
            createPanel.TeacherId = this.selectedTeacherId;
            createPanel.Show();
            this.RightPanel.UpdateLayout();
            this.RightAction.UpdateLayout();
        }

        // Метод який викликається при натисканні кнопки "Додати Розклад"
        private void СreateSchedule(object sender, RoutedEventArgs e)
        {
            //var createButton = (Button)sender;
            var createWindow = new CreateSchedule(
                this.subjectRepository.GetAllq().Include(x => x.Class).FirstOrDefault(x => x.ClassId == this.selectedClassId)!.Class!,
                new GenericRepository<Subject>(),
                this.scheduleRepository,
                this.dayOfWeekRepository,
                this.lessonTimeRepository,
                this.teacherRepository,
                this);

            createWindow.Show();
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
            this.teacherRepository.Delete(this.teacherRepository.GetAll().FirstOrDefault(x => x.UserId == this.selectedTeacherId && x.SubjectId == (int)deleteButton.Tag)!.Id);
            this.teacherRepository.Save();
            this.subjectRepository.Delete((int)deleteButton.Tag);
            this.subjectRepository.Save();
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
            this.scheduleRepository.Delete((int)deleteButton.Tag);
            this.scheduleRepository.Save();
            this.DeleteFromAdminPanels();

            var subjectId = this.subjectRepository.GetAll().FirstOrDefault(x => x.Id == (int)deleteButton.Tag).Id;

            var classId = this.subjectRepository.GetAll().FirstOrDefault(x => x.Id == subjectId).ClassId;
            ShowAllSubjects(classId);
            this.ShowAllSchedulesForSubject(this.selectedSubjectId);
            this.UpdateAdminPanels();
        }

        // Методи оновлення дисплею
        private void LookTeacher(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            var createPanel = new LookUsers(new GenericRepository<User>(), new GenericRepository<Pupil>(), this, new GenericRepository<Parents>(), new GenericRepository<ParentsPupil>());

            //var createPanel = new LookTeacher(new GenericRepository<User>(), new GenericRepository<Teacher>(), this);
            createPanel.currentClassId = this.selectedClassId;

            createPanel.currentUser = this.adminService.GetAllTeacher().FirstOrDefault(x => x.Id == (int)createButton.Tag);
            var teacher = this.adminService.GetAllTeacher().FirstOrDefault(x => x.Id == (int)createButton.Tag);
            createPanel.Email.Text = teacher!.Email?.ToString();
            createPanel.Password.Text = teacher!.Password?.ToString();
            createPanel.FirstName.Text = teacher!.FirstName?.ToString();
            createPanel.LastName.Text = teacher!.LastName?.ToString();
            createPanel.MiddleName.Text = teacher!.MiddleName?.ToString();
            createPanel.Gender.Text = teacher!.Gender?.ToString();
            createPanel.Birthday.Text = teacher!.Birthday?.ToString();
            createPanel.Adress.Text = teacher!.Address?.ToString();
            createPanel.PhoneNumber.Text = teacher!.PhoneNumber?.ToString();

            createPanel.Show();
            this.LeftPanel.UpdateLayout(); 
        }

        private void LookUsers(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;

            var createPanel = new LookUsers(new GenericRepository<User>(), new GenericRepository<Pupil>(), this, new GenericRepository<Parents>(), new GenericRepository<ParentsPupil>());
            createPanel.ShowAllClasses = true;
            createPanel.currentClassId = this.selectedClassId;
            var pupils = this.adminService.GetAllPupils().FirstOrDefault(x => x.Id == (int)createButton.Tag);
            createPanel.currentUser = pupils;
            createPanel.Email.Text = pupils!.Email!.ToString();
            createPanel.Password.Text = pupils!.Password!.ToString();
            createPanel.FirstName.Text = pupils!.FirstName!.ToString();
            createPanel.LastName.Text = pupils!.LastName!.ToString();
            createPanel.MiddleName.Text = pupils!.MiddleName!.ToString();
            createPanel.Gender.Text = pupils!.Gender!.ToString();
            createPanel.Birthday.Text = pupils!.Birthday!.ToString();
            createPanel.Adress.Text = pupils!.Address!.ToString();
            createPanel.PhoneNumber.Text = pupils!.PhoneNumber!.ToString();

            createPanel.Show();
            this.LeftPanel.UpdateLayout(); 
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
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
