// <copyright file="CreateSubject.xaml.cs" company="PlaceholderCompany">
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
    using WPFScholifyApp.BLL;
    using WPFScholifyApp.DAL.ClassRepository;
    using WPFScholifyApp.DAL.DBClasses;

    /// <summary>
    /// Interaction logic for CreateSubject.xaml.
    /// </summary>
    public partial class CreateSubject : Window
    {
        private IGenericRepository<Teacher> teacherRepository;
        private IGenericRepository<Subject> subjectRepository;
        private AdminWindow adminWindow;

        public int TeacherId { get; set; }

        public CreateSubject(IGenericRepository<Teacher> teacherRepos, IGenericRepository<Subject> subjectRepos, IGenericRepository<User> userRepository, AdminWindow adminWindow)
        {
            this.teacherRepository = teacherRepos;
            this.subjectRepository = subjectRepos;
            this.adminWindow = adminWindow;
            this.InitializeComponent();
        }

        private void SaveSubject(object sender, RoutedEventArgs e)
        {
            string subjectName = this.SubjectName.Text;
            var teacher = new Teacher
            {
                Id = (this.teacherRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1,
                UserId = this.TeacherId,
                SubjectId = (this.subjectRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0) + 1,
            };

            var subject = new Subject
            {
                Id = this.subjectRepository.GetAll().Select(x => x.Id).Max() + 1,
                SubjectName = subjectName,
            };

            this.subjectRepository.Insert(subject);
            this.subjectRepository.Save();
            this.teacherRepository.Insert(teacher);
            this.teacherRepository.Save();
            this.adminWindow.RightPanel.Children.Clear();
            this.adminWindow.RightAction.Children.Clear();
            this.adminWindow.ShowAllSubjectsForTeacher(this.TeacherId);
            this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
