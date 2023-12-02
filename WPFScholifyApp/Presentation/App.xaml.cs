// <copyright file="App.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp
{
    using System;
    using System.Windows;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using WPFScholifyApp.BLL;
    using WPFScholifyApp.DAL.ClassRepository;
    using WPFScholifyApp.DAL.DBClasses;
    using WPFScholifyApp.Presentation;
    using DayOfWeek = DAL.DBClasses.DayOfWeek;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<PupilWindow>();
                services.AddSingleton<AdminWindow>();
                services.AddSingleton<ParentsWindow>();
                services.AddSingleton<TeacherWindow>();

                services.AddTransient<LookUsers>();
                services.AddTransient<CreateAdvertisements>();
                services.AddTransient<CreateClass>();
                services.AddTransient<CreateParents>();
                services.AddTransient<CreateSchedule>();
                services.AddTransient<CreateSubject>();
                services.AddTransient<CreateTeacher>();
                services.AddTransient<CreateUser>();

                services.AddScoped<GenericRepository<Admin>>();
                services.AddScoped<GenericRepository<Advertisement>>();
                services.AddScoped<GenericRepository<Class>>();
                services.AddScoped<GenericRepository<DayBook>>();
                services.AddScoped<GenericRepository<DayOfWeek>>();
                services.AddScoped<GenericRepository<LessonTime>>();
                services.AddScoped<GenericRepository<Parents>>();
                services.AddScoped<GenericRepository<ParentsPupil>>();
                services.AddScoped<GenericRepository<Pupil>>();
                services.AddScoped<GenericRepository<Schedule>>();
                services.AddScoped<GenericRepository<Subject>>();
                services.AddScoped<GenericRepository<Teacher>>();
                services.AddScoped<GenericRepository<User>>();

                services.AddSingleton<AdminService>();
                services.AddSingleton<AdvertisementService>();
                services.AddSingleton<ClassService>();
                services.AddSingleton<DayBookService>();
                services.AddSingleton<DayOfWeekService>();
                services.AddSingleton<JournalService>();
                services.AddSingleton<LessonTimeService>();

                services.AddSingleton<ParentsService>();
                services.AddSingleton<PupilService>();
                services.AddSingleton<ScheduleService>();
                services.AddSingleton<SubjectService>();
                services.AddSingleton<TeacherService>();
                services.AddSingleton<UserService>();
                services.AddSingleton<WindowService>();
            })
            .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }

        [STAThread]
        public static void Main()
        {
            var app = new App();
            MainWindow mainWindow = App.AppHost.Services.GetRequiredService<MainWindow>();
            app.Run(mainWindow);
        }
    }
}
