using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using MySqlX.XDevAPI;
using TaskManager_Kazakov.Classes;

namespace TaskManager_Kazakov.Models
{
    public class Tasks : Notification
    {
        /// <summary> Код задачи </summary>
        public int Id { get; set; }

        /// <summary> Наименование </summary>
        private string name;
        /// <summary> Свойство наименования </summary>
        public string Name
        {
            get { return name; } // Аксессор чтения
            set // Аксессор записи
            {
                // проверяем входящие значение, на регулярное выражение
                Match match = Regex.Match(value, "^.{1,50}$");
                if (!match.Success) // если нет совпадения
                    MessageBox.Show("Наименование не должно быть пустым, и не более 50 символов.",
                        "Не корректный ввод значения."); // выводим сообщение
                else
                {
                    name = value; // запоминаем введённое значение
                    onPropertyChanged("Name"); // сообщаем об изменении свойства
                }
            }
        }

        /// <summary> Приоритет задачи </summary>
        private string priority;
        /// <summary> Свойство приоритета </summary>
        public string Priority
        {
            get { return priority; } // Аксессор чтения
            set // Аксессор записи
            {
                // проверяем входящие значение, на регулярное выражение
                Match match = Regex.Match(value, "^.{1,30}$");
                if (!match.Success) // если нет совпадения
                    MessageBox.Show("Приоритет не должно быть пустым, и не более 30 символов.",
                        "Не корректный ввод значения."); // выводим сообщение
                else
                {
                    priority = value; // запоминаем введённое значение
                    onPropertyChanged("Priority"); // сообщаем об изменении свойства
                }
            }
        }

        /// <summary> Поле даты выполнения </summary>
        private DateTime dateExecute;
        /// <summary> Свойство даты выполнения </summary>
        public DateTime DateExecute
        {
            get { return dateExecute; } // Аксессор чтения
            set // Аксессор записи
            {
                // Проверяем что указанная дата меньше чем текущая
                if (value.Date < DateTime.Now.Date)
                    MessageBox.Show("Дата выполнения не может быть меньше текущей.",
                        "Не корректный ввод значения."); // выводим сообщение
                else
                {
                    dateExecute = value; // запоминаем введённое значение
                    onPropertyChanged("DateExecute"); // сообщаем об изменении свойства
                }
            }
        }

        /// <summary> Комментарий </summary>
        private string comment;
        /// <summary> Свойство комментария </summary>
        public string Comment
        {
            get { return comment; } // Аксессор чтения
            set // Аксессор записи
            {
                // проверяем входящие значение, на регулярное выражение
                Match match = Regex.Match(value, "^.{1,1000}$");
                if (!match.Success) // если нет совпадения
                    MessageBox.Show("Комментарий не должен быть пустым, и не более 1000 символов.",
                        "Не корректный ввод значения."); // выводим сообщение
                else
                {
                    comment = value; // запоминаем введённое значение
                    onPropertyChanged("Comment"); // сообщаем об изменении свойства
                }
            }
        }

        /// <summary> Выполнено </summary>
        private bool done;
        /// <summary> Свойство для выполнено </summary>
        public bool Done
        {
            get { return done; } // Аксессор чтения
            set // Аксессор записи
            {
                done = value; // запоминаем введённое значение
                onPropertyChanged("Done"); // сообщаем об изменении свойства
                onPropertyChanged("IsDoneText"); // сообщаем об изменении свойства
            }
        }

        /// <summary> Видимость элементов </summary>
        [Schema.NotMapped] // исключаем поле из добавления в таблицу базы данных
        private bool isEnable;
        /// <summary> Свойство для видимости элементов </summary>
        [Schema.NotMapped] // исключаем поле из добавления в таблицу базы данных
        public bool IsEnable
        {
            get { return isEnable; } // Аксессор чтения
            set // Аксессор записи
            {
                isEnable = value; // запоминаем введённое значение
                onPropertyChanged("IsEnable"); // сообщаем об изменении свойства
                onPropertyChanged("IsEnableText"); // сообщаем об изменении свойства
            }
        }

        /// <summary> Текст для кнопки изменения </summary>
        [Schema.NotMapped] // исключаем поле из добавления в таблицу базы данных
        public string IsEnableText
        {
            get // Аксессор чтения
            {
                if (IsEnable) return "Сохранить"; // Если изменение включено, возвращаем одно значение
                else return "Изменить"; // Иначе другое
            }
        }

        /// <summary> Текст для кнопки выполнения </summary>
        [Schema.NotMapped] // исключаем поле из добавления в таблицу базы данных
        public string IsDoneText
        {
            get // Аксессор чтения
            {
                if (Done) return "Не выполнено"; // Если выполнено, возвращаем одно значение
                else return "Выполнено"; // Иначе другое
            }
        }

        /// <summary> Команда для изменения </summary>
        [Schema.NotMapped] // исключаем поле из добавления в таблицу базы данных
        public RealyCommand OnEdit
        {
            get // Аксессор чтения
            {
                return new RealyCommand(obj => { // Выполняем команду
                    IsEnable = !IsEnable; // Изменяем состояние изменения представления

                    if (!IsEnable) // Если состояние не активно
                        // Вызываем сохранение данных в контексте TaskContext
                        (MainWindow.init.DataContext as ViewModels.VM_Pages).vm_tasks.tasksContext.SaveChanges();
                });
            }
        }

        /// <summary> Команда для удаления </summary>
        [Schema.NotMapped] // исключаем поле из добавления в таблицу базы данных
        public RealyCommand OnDelete
        {
            get // Аксессор чтения
            {
                return new RealyCommand(obj => { // Выполняем команду
                    // Уточняем о том что пользователь хочет удалить объект
                    if (MessageBox.Show("Вы уверены что хотите удалить задачу?",
                        "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        // Удаляем модель из коллекции
                        (MainWindow.init.DataContext as ViewModels.VM_Pages).vm_tasks.Tasks.Remove(this);
                        // Удаляем модель из контекста данных
                        (MainWindow.init.DataContext as ViewModels.VM_Pages).vm_tasks.tasksContext.Remove(this);
                        // Вызываем сохранение данных в контексте TaskContext
                        (MainWindow.init.DataContext as ViewModels.VM_Pages).vm_tasks.tasksContext.SaveChanges();
                    }
                });
            }
        }

        /// <summary> Команда выполнения </summary>
        [Schema.NotMapped] // исключаем поле из добавления в таблицу базы данных
        public RealyCommand OnDone
        {
            get // Аксессор чтения
            {
                return new RealyCommand(obj => { // Выполняем команду
                    Done = !Done; // Изменяем состояние
                });
            }
        }
    }
}