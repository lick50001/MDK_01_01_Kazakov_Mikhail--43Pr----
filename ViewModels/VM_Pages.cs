using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Kazakov.Classes;
using TaskManager_Kazakov.View; // Добавьте это пространство имен

namespace TaskManager_Kazakov.ViewModels
{
    public class VM_Pages : Notification
    {
        public VM_Tasks vm_Tasks = new VM_Tasks();

        public VM_Pages()
        {
            MainWindow.init.frame.Navigate(new Main(vm_Tasks));
        }

        public RelayCommand OnClose
        {
            get
            {
                return new RelayCommand(obj =>
                MainWindow.init.Close());
            }
        }
    }
}