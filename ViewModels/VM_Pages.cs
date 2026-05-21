using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_Kazakov.Classes;

namespace TaskManager_Kazakov.ViewModels
{
    public class VM_Pages : Notification
    {
        public VM_Tasks vm_Tasks = new VM_Tasks();
        public VM_Pages()
        {
            MainWindow.init.frame.Navigate(new Views.Main(vm_Tasks));
        }

        public RealyCommand OnClose
        {
            get
            {
                return new RealyCommand(obj =>
                MainWindow.init.Close());
            }
        }
    }
}
