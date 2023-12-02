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
using WPFScholifyApp.Enums;
using DayOfWeek = WPFScholifyApp.DAL.DBClasses.DayOfWeek;

namespace WPFScholifyApp.Presentation
{
    /// <summary>
    /// Interaction logic for CreateSchedule.xaml
    /// </summary>
    public partial class CreateSchedule : Window
    {
        public Class? clas { get; set; }
        private AdminService adminService;
        private AdvertisementService advertisementService;
        private ClassService classService;
        private DayOfWeekService dayOfWeekService;
        private JournalService journalService;
        private LessonTimeService lessonTimeService;
        private ParentsService parentsService;
        private PupilService pupilService;
        private UserService userService;
        private ScheduleService scheduleService;
        private SubjectService subjectService;
        private TeacherService teacherService;
        private WindowService windowService;
        private MainWindow mainWindow;

        public ObservableCollection<ComboBoxItem> cbItems { get; set; }
        public ObservableCollection<ComboBoxItem> cbItems2 { get; set; }

        public CreateSchedule(AdminService adminService,
                            AdvertisementService advertisementService,
                            ClassService classService,
                            DayOfWeekService dayOfWeekService,
                            JournalService journalService,
                            LessonTimeService lessonTimeService,
                            ParentsService parentsService,
                            PupilService pupilService,
                            UserService userService,
                            ScheduleService scheduleService,
                            SubjectService subjectService,
                            TeacherService teacherService,
                            WindowService windowService,
                            MainWindow mainWindow
                            )
        {
            this.InitializeComponent();

            this.adminService = adminService;
            this.advertisementService = advertisementService;
            this.classService = classService;
            this.dayOfWeekService = dayOfWeekService;
            this.journalService = journalService;
            this.lessonTimeService = lessonTimeService;
            this.parentsService = parentsService;
            this.pupilService = pupilService;
            this.userService = userService;
            this.scheduleService = scheduleService;
            this.subjectService = subjectService;
            this.teacherService = teacherService;
            this.windowService = windowService;
            this.mainWindow = mainWindow;
            cbItems = new ObservableCollection<ComboBoxItem>();

            this.ClassLabel.Content = clas != null ? clas!.ClassName : "";
            cbItems.Add(new ComboBoxItem { Content = $"{Times.t8_30.ToString("HH:mm")} - {Times.t9_15.ToString("HH:mm")}", Tag = 1 });
            cbItems.Add(new ComboBoxItem { Content = $"{Times.t9_30.ToString("HH:mm")} - {Times.t10_15.ToString("HH:mm")}", Tag = 2 });
            cbItems.Add(new ComboBoxItem { Content = $"{Times.t10_30.ToString("HH:mm")} - {Times.t11_15.ToString("HH:mm")}", Tag = 3 });
            cbItems.Add(new ComboBoxItem { Content = $"{Times.t11_35.ToString("HH:mm")} - {Times.t12_20.ToString("HH:mm")}", Tag = 4 });
            cbItems.Add(new ComboBoxItem { Content = $"{Times.t12_40.ToString("HH:mm")} - {Times.t13_25.ToString("HH:mm")}", Tag = 5 });
            cbItems.Add(new ComboBoxItem { Content = $"{Times.t13_35.ToString("HH:mm")} - {Times.t14_20.ToString("HH:mm")}", Tag = 6 });
            cbItems.Add(new ComboBoxItem { Content = $"{Times.t14_35.ToString("HH:mm")} - {Times.t15_20.ToString("HH:mm")}", Tag = 7 });
            cbItems.Add(new ComboBoxItem { Content = $"{Times.t15_30.ToString("HH:mm")} - {Times.t16_15.ToString("HH:mm")}", Tag = 8 });
            this.cbItems = cbItems;
            this.TimeComboBox.ItemsSource = cbItems;
            if (clas == null)
            {
                // Assuming you have some default criteria for selecting a class from the repository
                this.clas = this.classService.GetAllClasses().FirstOrDefault() ?? new Class();
            }

            var subjects = this.subjectService.GetSubjectsByClassId(clas.Id);// this.subjectRepository.GetAll().Where(x => x.ClassId == clas.Id);
            cbItems2 = new ObservableCollection<ComboBoxItem>();
            foreach ( var subject in subjects)
            {
                cbItems2.Add(new ComboBoxItem { Content = subject.SubjectName, Tag = subject.Id });
            }
            this.cbItems2 = cbItems2;
            this.SubjectComboBox.ItemsSource = cbItems2;
        }

        private void ClassComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var timeId = ((ComboBoxItem)this.TimeComboBox.SelectedItem).Tag != null ? (int)((ComboBoxItem)this.TimeComboBox.SelectedItem).Tag : 0;
            var lessonTime = this.lessonTimeService.GetLessonTimeById(timeId);
            var subjectId =  ((ComboBoxItem)this.SubjectComboBox.SelectedItem).Tag != null ? (int)((ComboBoxItem)this.SubjectComboBox.SelectedItem).Tag : 0;
            var dayOfWeek = this.dayOfWeekService.GetAll().FirstOrDefault(x => x.Date.AddDays(1).Date.Equals(this.Date.SelectedDate!.Value.Date));

            if (dayOfWeek == null) {
                var newDayOfWeek = new DayOfWeek
                {
                    Id = this.adminService.GetNewDayOfWeekId(), // (this.dayOfWeekRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1,
                    Date = this.Date.SelectedDate!.Value.ToUniversalTime(),
                    Day = this.Date.SelectedDate!.Value.ToUniversalTime().ToString("d"),
                };

                this.dayOfWeekService.Save(newDayOfWeek);
                dayOfWeek = new DayOfWeek { Id = newDayOfWeek.Id };
            }

            var teacher = this.teacherService.GetTeacherBySubjectId(subjectId); // this.teacherRepository.GetAll().FirstOrDefault(x => x.SubjectId == subjectId);

            var newSchedule = new Schedule()
            {
                TeacherId = teacher!.Id,
                ClassId = this.clas!.Id,
                DayOfWeekId = dayOfWeek!.Id,
                LessonTimeId = lessonTime!.Id,
                SubjectId = subjectId
            };

            this.scheduleService.Save(newSchedule);

            this.Hide();

            this.windowService.Show<AdminWindow>(window =>
            {
                window.ShowAllWeek(this.clas!.Id);
            } );
        }
    }
}
