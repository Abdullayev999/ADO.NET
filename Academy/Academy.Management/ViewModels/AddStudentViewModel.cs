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
    public enum TypeSave
    {
        UPLOAD , CREATE
    }
    public class AddStudentViewModel :ViewModelBase
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMessenger messenger;
        public Student Student { get; set; }
        public TypeSave Type { get; set; }

        public AddStudentViewModel(IStudentRepository studentRepository, IMessenger messenger)
        {
            Student = new Student() { BirthDay = DateTime.Now };
            this.studentRepository = studentRepository;
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
                        var result= studentRepository.Update(Student);
                        if (!result)
                        {
                            messenger.Send<NavigationMessage>(new NavigationMessage { ViewModelType = typeof(StudentViewModel) });
                        }
                        else
                        {
                            MessageBox.Show("Student edit!");
                            Student newStud = studentRepository.GetById(Student.Id);
                            Student = newStud;
                        }
                        break;
                    case TypeSave.CREATE:
                        studentRepository.Create(Student);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                
            }

            Student = new Student() { BirthDay = DateTime.Now};
            messenger.Send(new ReloadMessage());
            messenger.Send(new NavigationMessage { ViewModelType = typeof(StudentViewModel) });
        });

        private RelayCommand cancelCommand = null;

        public RelayCommand CancelCommand => cancelCommand ??= new RelayCommand(
        () =>
        {
            Student = new Student() { BirthDay = DateTime.Now };
            messenger.Send(new NavigationMessage { ViewModelType = typeof(StudentViewModel) });
        });



    }
}
