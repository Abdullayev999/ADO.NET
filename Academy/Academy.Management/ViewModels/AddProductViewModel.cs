using Academy.Base;
using Academy.Base.Model;
using Academy.Base.Servieces;
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
    class AddProductViewModel : ViewModelBase
    {
        private readonly IProductRepository productRepository;
        private readonly IMessenger messenger;
        public Product Product { get; set; }
        public TypeSave Type { get; set; }

         public AddProductViewModel(IProductRepository productRepository, IMessenger messenger)
        {
            Product = new Product();
            this.productRepository = productRepository;
            this.messenger = messenger;
        } 
        private RelayCommand saveCommand = null;

        public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(
        () =>
        {
            try
            {
                switch (Type)
                {
                    case TypeSave.UPLOAD:
                        productRepository.Update(Product);
                        break;
                    case TypeSave.CREATE:
                        productRepository.Create(Product);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Product = new Product();
            messenger.Send(new ReloadMessage());
            messenger.Send(new NavigationMessage { ViewModelType = typeof(ProductViewModel) });
        });

        private RelayCommand cancelCommand = null;

        public RelayCommand CancelCommand => cancelCommand ??= new RelayCommand(
        () =>
        {
            Product = new Product();
            messenger.Send(new NavigationMessage { ViewModelType = typeof(ProductViewModel) });
        });



    }
}
