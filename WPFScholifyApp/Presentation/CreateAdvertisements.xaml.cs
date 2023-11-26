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
    /// Interaction logic for CreateAdvertisements.xaml
    /// </summary>
    public partial class CreateAdvertisements : Window
    {
        private IGenericRepository<Teacher> teacherRepository;
        private IGenericRepository<Class> classRepository;
        private IGenericRepository<Advertisement> advertisementRepository;
        private IGenericRepository<Pupil> pupilRepository;
        private TeacherWindow teacherWindow;
        private AdvertisementService advertisementService;
        public int ClassId { get; set; }

        public CreateAdvertisements(IGenericRepository<Teacher> teacherRepos, IGenericRepository<Advertisement> advertisementRepos, IGenericRepository<User> userRepository, TeacherWindow teacherWindow, IGenericRepository<Class>  classRepository, IGenericRepository<Pupil> pupilRepository)
        {
            this.teacherRepository = teacherRepos;
            this.classRepository = classRepository;
            this.advertisementRepository = advertisementRepos;
            this.teacherWindow = teacherWindow;
            this.pupilRepository = pupilRepository;
            this.advertisementService = new AdvertisementService(advertisementRepos, classRepository, pupilRepository);
            this.InitializeComponent();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            string name = this.Name1.Text;
            string description = this.Description.Text;
            var advertisement = new Advertisement
            {
                Id = (this.advertisementRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1,
                Name = name,
                Description = description,
                ClassId = this.ClassId
            };

            this.advertisementService.AddAdvertisement(advertisement);
            this.Close();

            this.teacherWindow.DeleteFromTeacherPanel();
            this.teacherWindow.ShowAllClasses();
            this.teacherWindow.ShowAllAdvertisementsForClassId(ClassId);
            this.teacherWindow.UpdateTeacherPanel();
        }

    }
}
