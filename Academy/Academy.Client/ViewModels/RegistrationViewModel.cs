using Academy.Base;
using Academy.Base.Messages;
using Academy.Base.Model;
using Academy.Base.Servieces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Academy.Client.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private IMessenger messanger;
        private IStudentClientRepository studentClientRepository;
        public string Login { get; set; }
        public string Password { get; set; }
        public string ForwardPassword { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand RegistrationCommand { get; set; }
        public RegistrationViewModel(IMessenger messanger, IStudentClientRepository studentClientRepository)
        {
            this.messanger = messanger;
            this.studentClientRepository = studentClientRepository;
            BackCommand = new RelayCommand(Back);
            RegistrationCommand = new RelayCommand(Registration);
        }
        private void Back()
        {
            messanger.Send(new NavigationMessage { ViewModelType = typeof(LoginViewModel) });
        }
        private void Registration()
        {
            if (Password.Equals(ForwardPassword))
            {
                try
                {
                    //Student stud = studentRepository.Registation(Login, Password);
                    AuthenticationUser authentication = studentClientRepository.Authentication(Login, Password, studentClientRepository.Registation);
                    messanger.Send(new LoginUserMessage { Student = authentication.student });
                    messanger.Send(new NavigationMessage { ViewModelType = typeof(HomeViewModel) });
                    File.WriteAllText("user.bin", $"{authentication.Token}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Passwords not equel!");
            }
        }
    }
}
