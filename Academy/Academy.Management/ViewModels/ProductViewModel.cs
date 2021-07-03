using Academy.Base;
using Academy.Base.Model;
using Academy.Base.Servieces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Management.ViewModels
{
    class ProductViewModel:ViewModelBase
    {
        public ObservableCollection<Product> Products { get; set; }

        public ProductViewModel(IProductRepository productRepository, IMessenger messanger)
        {
            messanger.Register<ReloadMessage>(this, item => { LoadProducts(); });
            this.productRepository = productRepository;
            this.messanger = messanger;
            LoadProducts();
        }
         

        public void LoadProducts()
        {
            Products = new ObservableCollection<Product>(productRepository.GetAll());
        }
        private RelayCommand addCommand = null;
        private readonly IProductRepository productRepository;
        private readonly IMessenger messanger;

        public RelayCommand AddCommand => addCommand ??= new RelayCommand(
        () =>
        {
            var view = App.Services.GetInstance<AddProductViewModel>();
            view.Type = TypeSave.CREATE;
            messanger.Send(new NavigationMessage { ViewModelType = typeof(AddProductViewModel) });
        });

        private RelayCommand backCommand = null;

        public RelayCommand BackCommand => backCommand ??= new RelayCommand(
        () =>
        {
            messanger.Send(new NavigationMessage { ViewModelType = typeof(HomeViewModel) });
        });

        private RelayCommand<Product> editCommand = null;

        public RelayCommand<Product> EditCommand => editCommand ??= new RelayCommand<Product>(
        (product) =>
        {
            var view = App.Services.GetInstance<AddProductViewModel>();
            view.Product = product;
            view.Type = TypeSave.UPLOAD;
            messanger.Send(new NavigationMessage { ViewModelType = typeof(AddProductViewModel) });
        });

        private RelayCommand<Product> deleteCommand = null;

        public RelayCommand<Product> DeleteCommand => deleteCommand ??= new RelayCommand<Product>(
        (product) =>
        {
            productRepository.Delete(product.Id);
            LoadProducts();
        });
    }
}
