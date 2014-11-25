using System;
using System.Linq.Expressions;
using System.Windows.Input;

namespace RxSandbox
{
    public class MethodCallCommand : ICommand
    {
        private readonly Action _action;
        public MethodCallCommand(object target, string methodName)
        {
            var methodCall = Expression.Call(Expression.Constant(target), methodName, new Type[0]);

            _action = System.Linq.Expressions.Expression<Action>.Lambda(methodCall).Compile() as Action;
        }

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            _action();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}