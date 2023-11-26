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
using WPFScholifyApp.DAL.ClassRepository;
using WPFScholifyApp.DAL.DBClasses;

namespace WPFScholifyApp.Presentation
{
    /// <summary>
    /// Interaction logic for LookClass.xaml
    /// </summary>
    public partial class LookClass : Window
    {
        public AdminWindow AdminWindow { get; set; }
        public int currentClass { get; set; }

        private IGenericRepository<Class> classRepository;

        public LookClass(IGenericRepository<Class> classRepository, AdminWindow adminWindow)
        {
            this.classRepository = classRepository;
            this.AdminWindow = adminWindow;
            InitializeComponent();
        }

        private void SaveClass(object sender, RoutedEventArgs e)
        {
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {

        }
    }
}
