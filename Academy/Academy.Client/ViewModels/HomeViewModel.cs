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

namespace Academy.Client.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private IMessenger messanger;
        private IStudentClientRepository studentClientRepository;
        public Student CurrentStudent { get; set; }
        public RelayCommand LogoutCommand { get; set; }
        public HomeViewModel(IMessenger messanger, IStudentClientRepository studentClientRepository)
        {
            this.messanger = messanger;
            this.studentClientRepository = studentClientRepository;
            messanger.Register<LoginUserMessage>(this,LoginUser);
            LogoutCommand = new RelayCommand(LogOut);
        }
        private void LoginUser(object message)
        {
            LoginUserMessage mess = message as LoginUserMessage;
            CurrentStudent = mess.Student;
        }
        private void LogOut()
        {
            studentClientRepository.LogOut(CurrentStudent.Id);
            if (File.Exists("user.bin"))
            {
                File.Delete("user.bin");
            }
            messanger.Send(new NavigationMessage { ViewModelType = typeof(LoginViewModel) });
        }
    }
}
