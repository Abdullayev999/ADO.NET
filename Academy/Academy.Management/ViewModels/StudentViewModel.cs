using Academy.Base;
using Academy.Base.Model;
using Academy.Base.Servieces; 
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Management.ViewModels
{
    public class StudentViewModel:ViewModelBase
    {
        public ObservableCollection<Student> Students { get; set; }
        //public DataTable Students { get; set; }

        public StudentViewModel(IStudentRepository studentRepository,IMessenger messanger)
        {  
            messanger.Register<ReloadMessage>(this,item => { LoadStudents(); });
            this.studentRepository = studentRepository;
            this.messanger = messanger;
            LoadStudents();
        }

        public void LoadStudents()
        {
             Students = new ObservableCollection<Student>(studentRepository.GetAll());
           // Students = studentRepository.GetAllDataTable();
        }
        private RelayCommand addCommand = null;
        private readonly IStudentRepository studentRepository;
        private readonly IMessenger messanger;

        public RelayCommand AddCommand => addCommand ??= new RelayCommand(
        () =>
        {
            var view = App.Services.GetInstance<AddStudentViewModel>(); 
            view.Type = TypeSave.CREATE;
            messanger.Send(new NavigationMessage { ViewModelType = typeof(AddStudentViewModel) });
        });

        private RelayCommand backCommand = null;

        public RelayCommand BackCommand => backCommand ??= new RelayCommand(
        () =>
        {
            messanger.Send(new NavigationMessage { ViewModelType = typeof(HomeViewModel) });    
        });

        private RelayCommand<Student> editCommand = null;

        public RelayCommand<Student> EditCommand => editCommand ??= new RelayCommand<Student>(
        (student) =>
        {
            var view = App.Services.GetInstance<AddStudentViewModel>();
            view.Student = student;
            view.Type = TypeSave.UPLOAD;
            messanger.Send(new NavigationMessage { ViewModelType = typeof(AddStudentViewModel) });
        });

        private RelayCommand<Student> deleteCommand = null;

        public RelayCommand<Student> DeleteCommand => deleteCommand ??= new RelayCommand<Student>(
        (student) =>
        {
            studentRepository.Delete(student.Id);
            LoadStudents();
        });
    }




}