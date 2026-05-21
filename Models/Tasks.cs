using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Windows;
using TaskManager_Kazakov.Classes;

namespace TaskManager_Kazakov.Models
{
    public class Tasks : Notification
    {
        public int Id { get; set; }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (Regex.IsMatch(value, @"^.{1,50}$"))
                {
                    name = value;
                    onPropertyChanged("Name");
                }
                else
                {
                    MessageBox.Show("Наименование: 1–50 символов.", "Ошибка ввода");
                }
            }
        }

        private string priority;
        public string Priority
        {
            get => priority;
            set
            {
                if (Regex.IsMatch(value, @"^.{1,30}$"))
                {
                    priority = value;
                    onPropertyChanged("Priority");
                }
                else
                {
                    MessageBox.Show("Приоритет: 1–30 символов.", "Ошибка ввода");
                }
            }
        }

        private DateTime dateExecute = DateTime.Now;
        public DateTime DateExecute
        {
            get => dateExecute;
            set
            {
                if (value.Date >= DateTime.Today)
                {
                    dateExecute = value;
                    onPropertyChanged("DateExecute");
                }
                else
                {
                    MessageBox.Show("Дата не может быть в прошлом.", "Ошибка ввода");
                }
            }
        }

        private string comment = "";
        public string Comment
        {
            get => comment;
            set
            {
                if (Regex.IsMatch(value, @"^.{0,1000}$"))
                {
                    comment = value;
                    onPropertyChanged("Comment");
                }
                else
                {
                    MessageBox.Show("Комментарий: до 1000 символов.", "Ошибка ввода");
                }
            }
        }

        private bool done;
        public bool Done
        {
            get => done;
            set
            {
                done = value;
                onPropertyChanged(nameof(Done));
                onPropertyChanged(nameof(IsDoneText));
            }
        }

        [NotMapped]
        private bool isEnable;
        [NotMapped]
        public bool IsEnable
        {
            get => isEnable;
            set
            {
                isEnable = value;
                onPropertyChanged(nameof(IsEnable));
                onPropertyChanged(nameof(IsEnableText));
            }
        }

        [NotMapped]
        public string IsEnableText => IsEnable ? "Сохранить" : "Изменить";

        [NotMapped]
        public string IsDoneText => Done ? "Не выполнено" : "Выполнено";

        // ⚠️ Лучше перенести эти команды в VM_Tasks, но если оставляете здесь:
        [NotMapped]
        public RelayCommand OnEdit => new RelayCommand(obj =>
        {
            IsEnable = !IsEnable;
            if (!IsEnable)
            {
                // Сохранение через глобальный доступ (не идеально, но работает)
                var mainVM = MainWindow.init?.DataContext as ViewModels.VM_Pages;
                mainVM?.vm_Tasks.tasksContext.SaveChanges();
            }
        });

        [NotMapped]
        public RelayCommand OnDelete => new RelayCommand(obj =>
        {
            if (MessageBox.Show("Удалить задачу?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var mainVM = MainWindow.init?.DataContext as ViewModels.VM_Pages;
                if (mainVM != null)
                {
                    // Удаляем из ObservableCollection
                    mainVM.vm_Tasks.Tasks.Remove(this);
                    // Удаляем из DbSet (правильно для EF6)
                    mainVM.vm_Tasks.tasksContext.Tasks.Remove(this);
                    // Сохраняем изменения
                    mainVM.vm_Tasks.tasksContext.SaveChanges();
                }
            }
        });

        [NotMapped] // ← БЫЛО [NotMapped]] — ИСПРАВЛЕНО!
        public RelayCommand OnDone => new RelayCommand(obj => Done = !Done);
    }
}