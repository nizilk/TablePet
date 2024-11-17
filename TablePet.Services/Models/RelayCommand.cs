using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TablePet.Services.Models
{
    public class RelayCommand: ICommand
    {
        private Action<object> execute; // 执行命令的委托

        private Func<object, bool> canExecute; // 判断命令是否可执行的委托



        public event EventHandler CanExecuteChanged; // 可执行性改变的事件



        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)

        {

            this.execute = execute;

            this.canExecute = canExecute;

        }



        public bool CanExecute(object parameter)

        {

            if (canExecute != null)

                return canExecute(parameter);



            return true; // 如果可执行性委托为 null，默认返回 true

        }



        public void Execute(object parameter)

        {

            execute?.Invoke(parameter); // 执行命令

        }



        public void RaiseCanExecuteChanged()

        {

            CanExecuteChanged?.Invoke(this, EventArgs.Empty); // 触发可执行性改变事件

        }

    }
}
