using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RxSandbox.Infrastructure;

namespace RxSandbox
{
    public class ObservableSourceVM : ViewModelBase, IDisposable
    {
        protected readonly ObservableCollection<Notification<string>> _history;
        public ReadOnlyObservableCollection<Notification<string>> History { get; private set; }
        public DelegateCommand<object> ClearHistory { get; private set; }

        public ObservableSourceVM()
        {
            ClearHistory = new DelegateCommand<object>(ClearHistoryHandler);
            _history = new ObservableCollection<Notification<string>>();
            History = new ReadOnlyObservableCollection<Notification<string>>(_history);

            if (IsInDesignMode())
            {
                _history.Add(new Notification<string>.OnNext("a"));
                _history.Add(new Notification<string>.OnNext("b"));
                _history.Add(new Notification<string>.OnNext("c"));
                _history.Add(new Notification<string>.OnCompleted());
            }
        }

        private void ClearHistoryHandler(object obj)
        {
            _history.Clear();
        }

        private bool _disposed;
        public void Dispose()
        {
            if (!_disposed)
            {
                OnDispose();
                _disposed = true;
            }
        }

        protected virtual void OnDispose()
        {
            
        }
    }

    public class ObservableInputVM : ObservableSourceVM
    {
        public string DisplayName { get { return string.Format("{0} : {1}", Name, _input.Type.Name); } }
        public string Name { get; private set; }
        public string Value { get; set; }
        private readonly ObservableInput _input;

        public DelegateCommand<object> OnNext { get; private set; }
        public DelegateCommand<object> OnError { get; private set; }
        public DelegateCommand<object> OnCompleted { get; private set; }

        private IDisposable _disposable;

        
        public ObservableInputVM()
        {
            OnNext = new DelegateCommand<object>(OnNextHandler, (o) => _input.IsActive);
            OnError = new DelegateCommand<object>(OnErrorHandler, (o) => _input.IsActive);
            OnCompleted = new DelegateCommand<object>(OnCompletedHandler, (o) => _input.IsActive);

            if (IsInDesignMode())
            {
                Name = "a";
                _input = new ObservableInput<string>();
            }
        }

        public ObservableInputVM(ObservableInput input)
            : this()
        {
            _input = input;
            Name = input.Name;
            _disposable = _input.ObservableStr.Materialize().ObserveOnDispatcher().Subscribe(_history.Add);
        }

        #region Commands

        private void OnErrorHandler(object obj)
        {
            if (!_input.IsActive)
                return;
            _input.OnError(new Exception("Exception from generator."));
            RefreshCommands();
        }

        private void OnCompletedHandler(object obj)
        {
            if (!_input.IsActive)
                return;
            _input.OnCompleted();
            RefreshCommands();
        }

        private void OnNextHandler(object obj)
        {
            if (!_input.IsActive)
                return;
            _input.OnNext(Value);
        }

        protected override void RefreshCommands()
        {
            OnNext.RaiseCanExecuteChanged();
            OnError.RaiseCanExecuteChanged();
            OnCompleted.RaiseCanExecuteChanged();
        } 

        #endregion


        protected override void OnDispose()
        {
            if (_disposable != null)
            {
                _disposable.Dispose();
                _disposable = null;
            }
        }
    }

    public class ObservableOutputVM : ObservableSourceVM
    {        
        public ObservableOutputVM()
        {
        }

        private IDisposable _disposable;

        public ObservableOutputVM(ObservableSource generator)
            : this()
        {
            _disposable = generator.ObservableStr.Materialize().ObserveOnDispatcher().Subscribe(_history.Add);
        }

        protected override void OnDispose()
        {
            if (_disposable != null)
            {
                _disposable.Dispose();
                _disposable = null;
            }
        }
    }

    public class ExpressionInstanceVM : ViewModelBase
    {        
        // commands
        public DelegateCommand<object> Reset { get; private set; }
        public DelegateCommand<object> Close { get; private set; }

        // public
        public ExpressionDefinition Definition { get; private set; }
        public ReadOnlyObservableCollection<ObservableInputVM> Inputs { get; private set; }
        private ObservableOutputVM output;
        public ObservableOutputVM Output
        {
            get { return output; }
            private set
            {
                if (value != output)
                {
                    output = value;
                    OnPropertyChanged("Output");
                }                
            }
        }

        private Diagram _diagram;
        public Diagram Diagram
        {
            get { return _diagram; }
            private set { _diagram = value; OnPropertyChanged("Diagram"); }
        }

        //private                
        private ExpressionInstance _instance;        
        private readonly ObservableCollection<ObservableInputVM> _inputs;

        
        public ExpressionInstanceVM()
        {
            Reset = new DelegateCommand<object>(ResetHandler);
            Close = new DelegateCommand<object>(CloseHandler);

            _inputs = new ObservableCollection<ObservableInputVM>();
            Inputs = new ReadOnlyObservableCollection<ObservableInputVM>(_inputs);

            if (IsInDesignMode())
            {
                Definition = new ExpressionDefinition("Zip", "To jest opis Zip ... ", "(a,b) => z.Zip(b, (x,y) => x + y)");
                _inputs.Add(new ObservableInputVM());
                Output = new ObservableOutputVM();
            }
        }

        public ExpressionInstanceVM(ExpressionDefinition definition)
            : this()
        {
            Definition = definition;                            
            Reset.Execute(null);
        }


        private void ResetHandler(object obj)
        {
            CleanUp();
            _cleanedUp = false;
            _instance = ExpressionInstance.Create(Definition);

            // input & output
            _inputs.Clear();
            foreach (var input in _instance.Inputs.Select(g => new ObservableInputVM(g)))
                _inputs.Add(input);

            Output = new ObservableOutputVM(_instance.Output);

            Diagram = _instance.Diagram;
        }

        public event EventHandler CloseRequested;
        protected virtual void OnCloseRequested(EventArgs args)
        {
            EventHandler ev = CloseRequested;
            if (ev != null)
                ev(this, args);
        }

        private void CloseHandler(object obj)
        {
            CleanUp();
            OnCloseRequested(EventArgs.Empty);
        }

        private bool _cleanedUp;
        public void CleanUp()
        {
            if (!_cleanedUp)
            {
                Inputs.OfType<IDisposable>().Run(d => d.Dispose());
                if (Output != null)
                    Output.Dispose();
                if (_instance != null)
                    _instance.Dispose();                
                _cleanedUp = true;
            }
        }
    }
}