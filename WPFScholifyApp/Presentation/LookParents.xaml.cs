using System;
using System.Collections.Generic;
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

namespace WPFScholifyApp.Presentation
{
    /// <summary>
    /// Interaction logic for LookParents.xaml
    /// </summary>
    public partial class LookParents : Window
    {

        private IGenericRepository<User> userRepository;
        private IGenericRepository<Parents> parentsRepository;
        private IGenericRepository<Pupil> pupilRepository;
        private IGenericRepository<ParentsPupil> parentsPupilRepository;
        private ParentsService parentsService;
        private AdminWindow adminWindow;
        public int ParentsId { get; set; }
        public User? currentParents { get; set; }
        public int currentPupilId { get; set; }



        public LookParents(IGenericRepository<User> userRepos, IGenericRepository<Parents> parentsRepos, IGenericRepository<Pupil> pupilRepos, AdminWindow adminWindow, IGenericRepository<ParentsPupil> parentsPupilRepository)
        {
            this.parentsPupilRepository = parentsPupilRepository;
            this.pupilRepository = pupilRepos;
            this.userRepository = userRepos;
            this.parentsRepository = parentsRepos;
            this.parentsService = new ParentsService(new GenericRepository<User>(), new GenericRepository<Pupil>(), new GenericRepository<Parents>());
            this.InitializeComponent();
            this.adminWindow = adminWindow;
        }

        private void SaveUser(object sender, RoutedEventArgs e)
        {
            var user = new User
            {
                Id = currentParents!.Id,
                Email = this.Email.Text,
                Password = this.Password.Text,
                FirstName = this.FirstName.Text,
                LastName = this.LastName.Text,
                MiddleName = this.MiddleName.Text,
                Gender = this.Gender.Text,
                Birthday = this.Birthday.SelectedDate!.Value.ToUniversalTime(),
                Address = this.Adress.Text,
                PhoneNumber = this.PhoneNumber.Text,
                Role = "батьки"
            };


            this.userRepository.Update(user);
            this.userRepository.Save();
            this.adminWindow.DeleteFromAdminPanels();
            this.adminWindow.ShowAllPuplis();
            this.adminWindow.ShowParentsForPupilId(this.currentPupilId);
            this.adminWindow.UpdateAdminPanels();
            this.Close();
        
    }

        private void Cancel(object sender, RoutedEventArgs e)
        {

        }
    }
}
