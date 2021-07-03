using Academy.Base.Servieces;
using Academy.Management.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using SimpleInjector;
using System;
using System.Collections.Generic; 
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Academy.Management
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Container Services { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            RegisterServices();

            Start<HomeViewModel>();
        }

        private void Start<T>() where T : ViewModelBase
        {
            var windowViewModel = Services.GetInstance<MainViewModel>();
            windowViewModel.CurrentViewModel = Services.GetInstance<T>();

            var mainWindow = new MainWindow { DataContext = windowViewModel };
            mainWindow.ShowDialog();
        }
        public void RegisterServices()
        {
            Services = new Container();
            Services.RegisterSingleton<IStudentRepository, StudentRepository>();
            Services.RegisterSingleton<IProductRepository, ProductRepository>(); 
            Services.RegisterSingleton<IMessenger, Messenger>();
            Services.RegisterSingleton<MainViewModel>(); 
            Services.RegisterSingleton<StudentViewModel>();
            Services.RegisterSingleton<AddStudentViewModel>();
            Services.RegisterSingleton<HomeViewModel>();
            Services.RegisterSingleton<ProductViewModel>();
            Services.RegisterSingleton<AddProductViewModel>(); 

            Services.Verify();
        }
    }
}
