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
using DayOfWeek = WPFScholifyApp.DAL.DBClasses.DayOfWeek;

namespace WPFScholifyApp.Presentation
{
    /// <summary>
    /// Interaction logic for CreateParents.xaml
    /// </summary>
    public partial class CreateParents : Window
    {
        private AdminService adminService;
        private AdvertisementService advertisementService;
        private JournalService journalService;
        private ParentsService parentsService;
        private PupilService pupilService;
        private UserService userService;
        private ScheduleService scheduleService;
        private TeacherService teacherService;
        private WindowService windowService;
        private MainWindow mainWindow;

        public int PupilsId { get; set; }
        public CreateParents(AdminService adminService,
                            AdvertisementService advertisementService,
                            JournalService journalService,
                            ParentsService parentsService,
                            PupilService pupilService,
                            UserService userService,
                            ScheduleService scheduleService,
                            TeacherService teacherService,
                            WindowService windowService,
                            MainWindow mainWindow
                            )
        {

            this.adminService = adminService;
            this.advertisementService = advertisementService;
            this.journalService = journalService;
            this.parentsService = parentsService;
            this.pupilService = pupilService;
            this.userService = userService;
            this.scheduleService = scheduleService;
            this.teacherService = teacherService;
            this.windowService = windowService;
            this.mainWindow = mainWindow;
            this.InitializeComponent();
        }

        private void SaveParents(object sender, RoutedEventArgs e)
        {
            string email = this.Email.Text;
            string password = this.Password.Text;
            string firstName = this.FirstName.Text;
            string middleName = this.MiddleName.Text;
            string lastName = this.LastName.Text;
            string gender = this.Gender.Text;
            DateTime birthday = this.Birthday.DisplayDate.ToUniversalTime();
            string adress = this.Adress.Text;
            string phoneNumber = this.PhoneNumber.Text;

            var user = new User
            {
                Id = this.adminService.GetNewUserId(),
                Email = email,
                Password = password,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Gender = gender,
                Birthday = birthday,
                Address = adress,
                PhoneNumber = phoneNumber,
                Role = "батьки"
            };

            var pupil = this.pupilService.GetAllPupils().FirstOrDefault(x => x.UserId == PupilsId);

            user = this.userService.GetUserById(user.Id);

            var parents = new Parents
            {
                Id = this.adminService.GetNewParentId(),
                UserId = user!.Id
            };

            this.userService.AddUser(user, parents);

            this.windowService.Show<AdminWindow>(window =>
            {
                window.DeleteFromAdminPanels();

                window.ShowAllPuplis();
                window.ShowParentsForPupilId(this.PupilsId);

                window.UpdateAdminPanels();
            });
                this.Hide();
            
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
