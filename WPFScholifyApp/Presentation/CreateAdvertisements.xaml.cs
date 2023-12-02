using System;
using System.Collections.Generic;
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
    /// Interaction logic for CreateAdvertisements.xaml
    /// </summary>
    public partial class CreateAdvertisements : Window
    {
        private AdminService adminService;
        private AdvertisementService advertisementService;
        private WindowService windowService;
        private MainWindow mainWindow;

        public int ClassId { get; set; }

        public CreateAdvertisements(
                            AdminService adminService,
                            AdvertisementService advertisementService,
                            WindowService windowService,
                            MainWindow mainWindow

                            )
        {

            this.adminService = adminService;
            this.advertisementService = advertisementService;
            this.windowService = windowService;
            this.mainWindow = mainWindow;
            this.InitializeComponent();
        }


        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            string name = this.Name1.Text;
            string description = this.Description.Text;
            var advertisement = new Advertisement
            {
                Id = this.adminService.GetNewAdvertisementId(),
                Name = name,
                Description = description,
                ClassId = this.ClassId
            };

            this.advertisementService.AddAdvertisement(advertisement);
            this.Hide();
            this.windowService.Show<TeacherWindow>(window =>
            {
                window.DeleteFromTeacherPanel();
                window.ShowAllClasses();
                window.ShowAllAdvertisementsForClassId(ClassId);
                window.UpdateTeacherPanel();
            });
            
        }

    }
}
