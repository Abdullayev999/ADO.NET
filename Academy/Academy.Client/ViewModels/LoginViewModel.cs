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
    public class LoginViewModel : ViewModelBase
    {
        private IMessenger messanger;
        private IStudentClientRepository studentClientRepository;
        public RelayCommand RegistrationCommand { get; set; }
        public RelayCommand LoginCommand { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public LoginViewModel(IMessenger messanger, IStudentClientRepository studentRepository)
        {
            this.messanger = messanger;
            this.studentClientRepository = studentRepository;
            RegistrationCommand = new RelayCommand(Registration);
            LoginCommand = new RelayCommand(LoginIn);
            //LoadUserFile();
        }
        private void Registration()
        {
            messanger.Send(new NavigationMessage { ViewModelType = typeof(RegistrationViewModel) });
        }
        private void LoginIn()
        {
            try
            {
                //Student stud = studentClientRepository.Login(Login, Password);
                AuthenticationUser authentication = studentClientRepository.Authentication(Login, Password, studentClientRepository.Login);
                messanger.Send(new LoginUserMessage { Student = authentication.student });
                messanger.Send(new NavigationMessage { ViewModelType = typeof(HomeViewModel) });
                File.WriteAllText("user.bin",$"{authentication.Token.ToString()}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void LoadUserFile()
        {
            if (File.Exists("user.bin"))
            {
                //var lines = File.ReadAllLines("user.bin");
                //Login = lines[0];
                //Password = lines[1];
                //LoginIn();

                Guid token = Guid.Parse(File.ReadAllText("user.bin"));
                AuthenticationUser authentication = studentClientRepository.Authentication(token);
                messanger.Send(new LoginUserMessage { Student = authentication.student });
                messanger.Send(new NavigationMessage { ViewModelType = typeof(HomeViewModel) });
            }
        }

    }
}
