using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HelloBUPT.SideBarMenu {
    public class DelegateCommand : ICommand{
        public Action<object> ExecuteAction { get; set; }
        public Func<object, bool> CancelExecute { get; set; }
        public event EventHandler CanExecuteChanged;
        public bool CanExecute (object parameter) {
            if(CancelExecute == null) {
                return true;
            }
            return CancelExecute(parameter);
        }
        public void Execute(object parameter) {
            if(ExecuteAction == null) {
                return;
            }
            ExecuteAction(parameter);
        }
    }
}
