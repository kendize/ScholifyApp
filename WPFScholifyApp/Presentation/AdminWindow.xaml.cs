// <copyright file="AdminWindow.xaml.cs" company="PlaceholderCompany">
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

    /// <summary>
    /// Interaction logic for AdminWindow.xaml.
    /// </summary>
    public partial class AdminWindow : Window
    {
        private AdminService adminService;
        private UserService userService;
        private User? testteacher;
        private IGenericRepository<User> userRepository;
        private IGenericRepository<Pupil> pupilRepository;
        private IGenericRepository<Subject> subjectRepository;
        private IGenericRepository<Teacher> teacherRepository;
        private IGenericRepository<Class> classRepository;
        private int selectedClassId;
        private int selectedTeacherId;

        // private int selectedPupilId;
        // private int selectedSubjectId;
        // private int selectedParentId;
        public AdminWindow()
        {
            this.userRepository = new GenericRepository<User>();
            this.subjectRepository = new GenericRepository<Subject>();
            this.pupilRepository = new GenericRepository<Pupil>();
            this.teacherRepository = new GenericRepository<Teacher>();
            this.classRepository = new GenericRepository<Class>();
            this.userService = new UserService(new GenericRepository<User>(), new GenericRepository<Pupil>());
            this.adminService = new AdminService(new GenericRepository<User>(), new GenericRepository<Class>(), new GenericRepository<Teacher>(), new GenericRepository<Pupil>(), new GenericRepository<Admin>(), new GenericRepository<Parents>(), new GenericRepository<Subject>());
            this.InitializeComponent();
        }

        // Метод який викликається при натисканні кнопки "Класи" на панелі Адміністратора
        public void ClassButton_Click(object sender, RoutedEventArgs e)
        {
            this.selectedClassId = 0;
            this.RightPanel.Children.Clear();
            this.LeftPanel.Children.Clear();
            this.LeftAction.Children.Clear();
            this.ShowAllClasses();
            this.LeftPanel.UpdateLayout();
            this.RightPanel.UpdateLayout();
            this.RightAction.UpdateLayout();
        }

        // Метод для виведення списку кнопок з усіма класами
        public void ShowAllClasses()
        {
            var classes = this.adminService.GetAllClasses();

            foreach (var c in classes)
            {
                var button = new Button { Content = c.ClassName, Height = 60, Width = 300, FontSize = 30, Tag = c.Id };
                button.Click += new RoutedEventHandler(this.SpecificClassButton_Click);
                this.LeftPanel.Children.Add(button);
            }

            // var createButton = new Button
            // {
            //    Content = "Створити Клас",
            //    VerticalAlignment = VerticalAlignment.Bottom,
            // };
            // createButton.Click += new RoutedEventHandler(this.AddClass);
            // this.LeftAction.Children.Add(createButton);
        }

        // Метод який викликається при натисканні кнопки обраного класу серед списку класів на панелі Адміністратора
        public void SpecificClassButton_Click(object sender, RoutedEventArgs e)
        {
            // Знайдемо ClassId з Tag кнопки, на яку ми натискали
            this.RightPanel.Children.Clear();
            var classButton = (Button)sender;
            this.selectedClassId = (int)classButton.Tag;

            // Очистимо праву панель, додамо кнопки з учнями та оновимо панель
            this.RightPanel.Children.Clear();
            this.ShowAllPupilsForClassId(this.selectedClassId);
            this.RightPanel.UpdateLayout();
        }

        // Метод для виведення списку кнопок з усіма учнями для обраного класу
        public void ShowAllPupilsForClassId(int classId)
        {
            this.RightAction.Children.Clear();
            this.RightPanel.Children.Clear();
            this.selectedClassId = classId;
            var pupils = this.adminService.GetAllPupilsForClass(classId);
            foreach (var p in pupils)
            {
                var pupilButton = new Button { Content = $"{p!.FirstName} {p!.LastName}", Height = 60, Width = 300, FontSize = 30, };
                var deleteButton = new Button { Content = $"Delete {p!.FirstName} {p!.LastName}", Height = 60, Width = 300, FontSize = 30, Tag = p.Id };
                deleteButton.Click += new RoutedEventHandler(this.DeletePupil);

                this.RightPanel.Children.Add(pupilButton);
                this.RightPanel.Children.Add(deleteButton);
            }

            // Після виведення всіх учнів для обраного класу додамо кнопку "Додати Учня"
            var createButton = new Button { Content = "Додати Учня", Height = 60, Width = 300, FontSize = 30, Tag = classId };
            createButton.Click += new RoutedEventHandler(this.AddPupil);
            this.RightAction.Children.Add(createButton);
            this.LeftPanel.UpdateLayout();
            this.RightAction.UpdateLayout();
        }

        // Метод який викликається при натисканні кнопки "Додати Учня"
        private void AddPupil(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            var createPanel = new CreateUser(this.userRepository, this.pupilRepository, this);
            createPanel.ClassId = this.selectedClassId;
            createPanel.Show();
        }

        // Метод який викликається при натисканні кнопки "Вчителі" на панелі Адміністратора
        public void TeachersButton_Click(object sender, RoutedEventArgs e)
        {
            this.RightPanel.Children.Clear();
            this.LeftPanel.Children.Clear();
            this.ShowAllTeachers();
            var createButton = new Button { Content = "Додати Вчителя", Height = 60, Width = 300, FontSize = 30, };
            createButton.Click += new RoutedEventHandler(this.AddTeacher);
            this.LeftPanel.Children.Add(createButton);
            this.LeftPanel.UpdateLayout();
        }

        // Метод для виведення списку кнопок з усіма вчителями
        public void ShowAllTeachers()
        {
            this.LeftAction.Children.Clear();
            this.LeftPanel.Children.Clear();
            this.RightPanel.Children.Clear();
            var teacher = this.adminService.GetAllTeacher();
            foreach (var t in teacher)
            {
                var button = new Button { Content = $"{t!.FirstName} {t!.LastName}", Height = 60, Width = 300, FontSize = 30, Tag = t.Id };
                button.Click += new RoutedEventHandler(this.SpecificTeacherButton_Click);
                this.LeftPanel.Children.Add(button);
                var lookButton = new Button { Content = "look", Height = 60, Width = 300, FontSize = 30, Tag = t.Id };
                lookButton.Click += new RoutedEventHandler(this.LookTeacher);
                this.LeftPanel.Children.Add(lookButton);
                var deleteButton = new Button { Content = $"Delete {t!.FirstName} {t!.LastName}", Height = 60, Width = 300, FontSize = 30, Tag = t.Id };
                deleteButton.Click += new RoutedEventHandler(this.DeleteTeacher);
            }

            var createButton = new Button { Content = "Додати Вчителя", Height = 60, Width = 300, FontSize = 30, };
            createButton.Click += new RoutedEventHandler(this.AddTeacher);
            this.LeftAction.Children.Add(createButton);
            this.LeftAction.UpdateLayout();
            this.RightPanel.UpdateLayout();
            this.LeftPanel.UpdateLayout();
        }

        // Метод який викликається при натисканні кнопки обраного вчителя серед списку вчителів на панелі Адміністратора
        public void SpecificTeacherButton_Click(object sender, RoutedEventArgs e)
        {
            var teacherButton = (Button)sender;
            this.selectedTeacherId = (int)teacherButton.Tag;
            this.ShowAllSubjectsForTeacher(this.selectedTeacherId);
        }

        public void ShowAllSubjectsForTeacher(int teacherId)
        {
            this.RightPanel.Children.Clear();
            this.RightAction.Children.Clear();
            this.testteacher = this.adminService.GetAllTeacher().FirstOrDefault(x => x.Id == teacherId);
            var subjects = this.adminService.GetAllSubjectsForTeacher(teacherId);
            foreach (var p in subjects)
            {
                var subjectButton = new Button { Content = $"{p!.SubjectName}", Height = 60, Width = 300, FontSize = 30, };
                var deleteButton = new Button { Content = $"Delete {p!.SubjectName}", Height = 60, Width = 300, FontSize = 30, Tag = p.Id };
                deleteButton.Click += new RoutedEventHandler(this.DeleteSubject);

                this.RightPanel.Children.Add(subjectButton);
                this.RightPanel.Children.Add(deleteButton);
            }

            var createButton = new Button { Content = "Додати предмет викладачу", Height = 60, Width = 300, FontSize = 30, Tag = teacherId };
            createButton.Click += new RoutedEventHandler(this.AddSubjectToTeacher);

            this.RightAction.Children.Add(createButton);
            this.RightAction.UpdateLayout();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddTeacher(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            var createPanel = new CreateTeacher(this.userRepository, this);
            createPanel.Show();
        }

        private void LookTeacher(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;
            var createPanel = new LookTeacher(this.userRepository);
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
            this.LeftPanel.UpdateLayout(); // воно не робе
        }

        private void LookUsers(object sender, RoutedEventArgs e)
        {
            var createButton = (Button)sender;

            var createPanel = new LookUsers(this.userRepository, this.pupilRepository);

            var pupils = this.adminService.GetAllPupils().FirstOrDefault(x => x.Id == (int)createButton.Tag);
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
            this.LeftPanel.UpdateLayout(); // воно не робе
        }

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

        private void DeleteTeacher(object sender, RoutedEventArgs e)
        {
            var deleteButton = (Button)sender;
            this.userService.DeletePupil((int)deleteButton.Tag);
        }

        private void AddSubjectToTeacher(object sender, RoutedEventArgs e)
        {
            var subjectButton = (Button)sender;
            var createPanel = new CreateSubject(this.teacherRepository, this.subjectRepository, this.userRepository, this);
            createPanel.TeacherId = this.selectedTeacherId;
            createPanel.Show();
            this.RightPanel.UpdateLayout();
            this.RightAction.UpdateLayout();
        }

        private void AddClass(object sender, RoutedEventArgs e)
        {
            var subjectButton = (Button)sender;
            var createPanel = new CreateClass(this.classRepository, this);
            createPanel.Show();
            this.LeftPanel.UpdateLayout(); // воно не робе
        }

        private void DeleteSubject(object sender, RoutedEventArgs e)
        {
            var deleteButton = (Button)sender;
            this.teacherRepository.Delete(this.teacherRepository.GetAll().FirstOrDefault(x => x.UserId == this.selectedTeacherId && x.SubjectId == (int)deleteButton.Tag) !.Id);
            this.teacherRepository.Save();
            this.subjectRepository.Delete((int)deleteButton.Tag);
            this.subjectRepository.Save();
            this.RightPanel.UpdateLayout();
            this.RightPanel.Children.Clear();
            this.RightAction.Children.Clear();
            this.ShowAllSubjectsForTeacher(this.selectedTeacherId);
        }

        private void ParentsButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
