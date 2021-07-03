using Academy.Base; 
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Academy.Management.ViewModels
{
    class HomeViewModel : ViewModelBase
    {
        private readonly IMessenger messenger;

        public HomeViewModel(IMessenger messenger)
        {
            this.messenger = messenger;
        }


        private RelayCommand studentsCommand = null;

        public RelayCommand StudentsCommand => studentsCommand ??= new RelayCommand(
        () =>
        {
            messenger.Send(new NavigationMessage { ViewModelType = typeof(StudentViewModel) });
        });

        private RelayCommand productsCommand = null;

        public RelayCommand ProductsCommand => productsCommand ??= new RelayCommand(
        () =>
        {
            messenger.Send(new NavigationMessage { ViewModelType = typeof(ProductViewModel) });
        });
    }
}
