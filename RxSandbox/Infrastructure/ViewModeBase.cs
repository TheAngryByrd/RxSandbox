using System.ComponentModel;
using System.Windows;

namespace RxSandbox
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected static bool IsInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(new DependencyObject());
        }
        protected virtual void RefreshCommands()
        {
        } 


        public event PropertyChangedEventHandler PropertyChanged;
        public void RaiseNotifyPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChangedImpl(propertyName);
        }

        protected virtual void OnPropertyChangedImpl(string propertyName)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
