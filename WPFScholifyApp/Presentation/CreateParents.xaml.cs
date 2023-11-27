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
using WPFScholifyApp.BLL;
using WPFScholifyApp.DAL.ClassRepository;
using WPFScholifyApp.DAL.DBClasses;

namespace WPFScholifyApp.Presentation
{
    /// <summary>
    /// Interaction logic for CreateParents.xaml
    /// </summary>
    public partial class CreateParents : Window
    {
        private IGenericRepository<User> userRepository;
        private IGenericRepository<Parents> parentsRepository;
        private IGenericRepository<Pupil> pupilRepository;
        private IGenericRepository<ParentsPupil> parentsPupilRepository;
        private ParentsService parentsService;
        private AdminWindow adminWindow;
        public int PupilsId { get; set; }
        public CreateParents(IGenericRepository<User> userRepos, IGenericRepository<Parents> parentsRepos,IGenericRepository<Pupil> pupilRepos, AdminWindow adminWindow, IGenericRepository<ParentsPupil> parentsPupilRepository)
            {
            this.parentsPupilRepository = parentsPupilRepository;
            this.pupilRepository = pupilRepos;
            this.userRepository = userRepos;
            this.parentsRepository = parentsRepos;
            this.parentsService = new ParentsService(new GenericRepository<User>(), new GenericRepository<Pupil>(), new GenericRepository<Parents>());
            this.InitializeComponent();
            this.adminWindow = adminWindow;
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
                Id = this.userRepository.GetAll().Select(x => x.Id).Max() + 1,
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

            var pupil = this.pupilRepository.GetAll().FirstOrDefault(x => x.Id == PupilsId);

            this.userRepository.Insert(user);
            this.userRepository.Save();
            user = this.userRepository.GetAll().FirstOrDefault(x => x.Id == user.Id);

            var parents = new Parents
            {
                Id = (this.parentsRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1,
                UserId = user!.Id
            };


            this.parentsRepository.Insert(parents);
            this.parentsRepository.Save();

            var ParentsPupil = new ParentsPupil
            {
                pupilId = pupil!.Id,
                parentId = parents.Id
            };

            this.parentsPupilRepository.Insert(ParentsPupil);
            this.parentsPupilRepository.Save();


            // this.parentsService.AddUser(user, parents);
            this.Close();
            this.adminWindow.DeleteFromAdminPanels();

            this.adminWindow.ShowAllPuplis();
            this.adminWindow.ShowParentsForPupilId(this.PupilsId);

            this.adminWindow.UpdateAdminPanels();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
