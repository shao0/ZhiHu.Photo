using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ZhiHu.Photo.Configurations.Models;
using ZhiHu.Photo.DataAccess;
using ZhiHu.Photo.DataAccess.Contacts;
using ZhiHu.Photo.Extensions;
using ZhiHu.Photo.Services;
using ZhiHu.Photo.Services.Contacts;
using ZhiHu.Photo.ViewModels;
using ZhiHu.Photo.Views;

namespace ZhiHu.Photo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceProvider = ConfigureServices(new ServiceCollection())
                .AutoMapper()
                .AutoInjection(suffixNames: new[] { "Service", "DataAccess" })
                .AutoConfiguration(new[] { "ZhiHu.Photo.Configurations.Models" })
                .BuildServiceProvider();
            var window = ServiceProvider.GetService<MainWindow>();
            window.DataContext = ServiceProvider.GetService<ShellViewModel>();
            Current.MainWindow = window;
            window.ShowDialog();
            base.OnStartup(e);
        }

        private IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<ShellView>();
            services.AddSingleton<ShellViewModel>();
            return services;
        }
    }
}
