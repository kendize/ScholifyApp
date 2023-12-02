// <copyright file="CreateSubject.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
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
    /// Interaction logic for CreateSubject.xaml.
    /// </summary>
    public partial class CreateSubject : Window
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

        public int TeacherId { get; set; }
        public ObservableCollection<ComboBoxItem> cbItems { get; set; }

        public CreateSubject(AdminService adminService,
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
                            MainWindow mainWindow)
        {
            this.InitializeComponent();

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

            cbItems = new ObservableCollection<ComboBoxItem>();
            var classes = this.classService.GetAllClasses();
            foreach ( var c in classes )
            {
                cbItems.Add(new ComboBoxItem { Content = c.ClassName, Tag = c.Id });
            }
            this.cbItems = cbItems;
            this.ClassComboBox.ItemsSource = cbItems;
        }

        private void SaveSubject(object sender, RoutedEventArgs e)
        {
            string subjectName = this.SubjectName.Text;
            var teacher = new Teacher
            {
                Id = adminService.GetNewTeacherId(),
                UserId = this.TeacherId,
                SubjectId = adminService.GetNewSubjectId()
            };

            var subject = new Subject
            {
                Id = adminService.GetNewSubjectId(),
                SubjectName = subjectName,
                ClassId = (int)((ComboBoxItem)this.ClassComboBox.SelectedItem).Tag
        };

            this.subjectService.SaveSubject(subject, teacher);

            this.windowService.Show<AdminWindow>(window =>
            {
                window.RightPanel.Children.Clear();
                window.RightAction.Children.Clear();
                window.ShowAllSubjectsForTeacher(this.TeacherId);
            });
                
            this.Hide();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Save(object sender, RoutedEventArgs e)
        {

        }
    }
}
