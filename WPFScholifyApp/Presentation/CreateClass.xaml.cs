// <copyright file="CreateClass.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.Presentation
{
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

    /// <summary>
    /// Interaction logic for CreateClass.xaml.
    /// </summary>
    public partial class CreateClass : Window
    {
        private IGenericRepository<Class> classRepository;
        private AdminWindow adminWindow;

        public CreateClass(IGenericRepository<Class> classRepos, AdminWindow adminWindow)
        {
            this.classRepository = classRepos;
            this.InitializeComponent();
            this.adminWindow = adminWindow;
        }

        private void SaveSubject(object sender, RoutedEventArgs e)
        {
            string className = this.ClassName.Text;
            this.Close();
            var clases = new Class
            {
                Id = this.classRepository.GetAll().Select(x => x.Id).Max() + 1,
                ClassName = className,
            };

            this.adminWindow.LeftPanel.Children.Clear();
            this.classRepository.Insert(clases);
            this.classRepository.Save();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
