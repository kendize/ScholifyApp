using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private IGenericRepository<Schedule> scheduleRepository;
        private IGenericRepository<DayOfWeek> dayOfWeekRepository;
        private IGenericRepository<LessonTime> lessonTimeRepository;
        private IGenericRepository<Teacher> teacherRepository;
        private IGenericRepository<Subject> subjectRepository;
        private AdminWindow adminWindow;
        public Class? Class;

        public ObservableCollection<ComboBoxItem> cbItems { get; set; }
        public ObservableCollection<ComboBoxItem> cbItems2 { get; set; }

        public CreateSchedule(Class clas, IGenericRepository<Subject> subjectRepository, IGenericRepository<Schedule> scheduleRepository, IGenericRepository<DayOfWeek> dayOfWeekRepository, IGenericRepository<LessonTime> lessonTimeRepository, IGenericRepository<Teacher> teacherRepository, AdminWindow adminWindow)
        {
            this.adminWindow = adminWindow;
            InitializeComponent();
            this.scheduleRepository = scheduleRepository;
            this.dayOfWeekRepository = dayOfWeekRepository;
            this.lessonTimeRepository = lessonTimeRepository;
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
            cbItems = new ObservableCollection<ComboBoxItem>();

            this.ClassLabel.Content = clas.ClassName;
            this.Class = clas;
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
            this.adminWindow = adminWindow;

            var subjects = this.subjectRepository.GetAll().Where(x => x.ClassId == clas.Id);
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
            var lessonTime = this.lessonTimeRepository.GetAll().FirstOrDefault(x => x.Id == timeId);
            var subjectId =  ((ComboBoxItem)this.SubjectComboBox.SelectedItem).Tag != null ? (int)((ComboBoxItem)this.SubjectComboBox.SelectedItem).Tag : 0;
            var dayOfWeek = this.dayOfWeekRepository.GetAll().FirstOrDefault(x => x.Date.AddDays(1).Date.Equals(this.Date.SelectedDate!.Value.Date));

            if (dayOfWeek == null) {
                var newDayOfWeek = new DayOfWeek
                {
                    Id = (this.dayOfWeekRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1,
                    Date = this.Date.SelectedDate!.Value.ToUniversalTime(),
                    Day = this.Date.SelectedDate!.Value.ToUniversalTime().ToString("d"),
                };

                this.dayOfWeekRepository.Insert(newDayOfWeek);
                this.dayOfWeekRepository.Save();
                dayOfWeek = new DayOfWeek { Id = newDayOfWeek.Id };
            }

            var teacher = this.teacherRepository.GetAll().FirstOrDefault(x => x.SubjectId == subjectId);

            var newSchedule = new Schedule()
            {
                TeacherId = teacher!.Id,
                ClassId = this.Class!.Id,
                DayOfWeekId = dayOfWeek!.Id,
                LessonTimeId = lessonTime!.Id,
                SubjectId = subjectId
            };

            this.scheduleRepository.Insert(newSchedule);
            this.scheduleRepository.Save();

            this.Close();

            this.adminWindow.ShowAllWeek(this.Class!.Id);
        }
    }
}
