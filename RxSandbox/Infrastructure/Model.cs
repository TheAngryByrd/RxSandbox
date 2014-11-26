using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using RxSandbox.Properties;

namespace RxSandbox
{
    public abstract class ObservableSource
    {
        public string Name { get; set; }
        public Type Type { get; protected set; }
        public IObservable<string> ObservableStr { get; protected set; }

        protected TypeConverter Converter { get; set; }

        protected ObservableSource()
        {
            Name = "";
        }

        protected void CreateTypeConverter(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);
            if ((converter == null) || !converter.CanConvertTo(typeof(string))
                || !converter.CanConvertFrom(typeof(string)))
                throw new InvalidOperationException("Type '" + type + "' doesn't provide appropriate TypeConverter");

            Converter = converter;
        }
    }


    public abstract class ObservableInput : ObservableSource
    {
        public abstract void OnNext(string next);
        public abstract void OnError(Exception exception);
        public abstract void OnCompleted();

        public bool IsActive { get; protected set; }

        protected ObservableInput()
        {
            IsActive = true;
        }
    }


    internal interface IObservableInput
    {
        object Observable { get; }
    }

    public sealed class ObservableInput<T> : ObservableInput, IObservableInput
    {
        object IObservableInput.Observable { get { return Observable; } }

        private readonly Subject<T> _observable;
        public IObservable<T> Observable
        {
            get { return _observable; }
        }

        public ObservableInput()
        {
            CreateTypeConverter(typeof(T));

            Type = typeof(T);
            _observable = new Subject<T>();

            if (typeof(T) == typeof(string))
                ObservableStr = (IObservable<string>)(object)Observable;
            else
            {
                ObservableStr = Observable.Select(s => Converter.ConvertToString(s));
            }
        }


        public void OnNext(T next)
        {
            _observable.OnNext(next);
        }

        public override void OnNext(string s)
        {
            if (!IsActive)
                throw new InvalidOperationException("juz wykonano OnError lub OnCompleted");

            if (Type == typeof(string))
                OnNext((T)(object)s);
            else
            {
                OnNext((T)Converter.ConvertFromString(s));
            }
        }

        public override void OnError(Exception exception)
        {
            if (!IsActive)
                throw new InvalidOperationException("juz wykonano OnError lub OnCompleted");

            _observable.OnError(exception);
            IsActive = false;
        }

        public override void OnCompleted()
        {
            if (!IsActive)
                throw new InvalidOperationException("juz wykonano OnError lub OnCompleted");

            _observable.OnCompleted();
            IsActive = false;
        }


    }


    public class ObservableOutput<T> : ObservableSource
    {
        public IObservable<T> Observable { get; private set; }

        public ObservableOutput(IObservable<T> output)
        {
            CreateTypeConverter(typeof(T));
            Type = typeof(T);
            Observable = output;

            if (typeof(T) == typeof(string))
                ObservableStr = (IObservable<string>)(object)Observable;
            else
            {
                ObservableStr = Observable.Select(s => Converter.ConvertToString(s));
            }
        }
    }

    public class ExpressionDefinition
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        //public string Group { get; private set; }
        //public LambdaExpression Expression { get; private set; }
        public string ExpressionString { get; private set; }


        internal Dictionary<string, Type> SourceTypes { get; private set; }
        internal Delegate CompiledExpression { get; private set; }
        public Diagram Diagram { get; private set; }


        private ExpressionDefinition()
        {

        }
        
        internal ExpressionDefinition(string name, string description, string expressionString)
        {
            Name = name;
            Description = description;
            ExpressionString = expressionString;
        }


        private static readonly DiagramContainer _container;
        static ExpressionDefinition()
        {
            if (File.Exists(Settings.Default.DiagramFilePath))
            {
                var fileContent = File.ReadAllText("Diagrams.xml");             
                _container = XmlSerializationHelper.FromXml<DiagramContainer>(fileContent);                    
            }
        }


        private static MethodInfo GetOperator(LambdaExpression expression)
        {
            var methodCallExpression = expression.Body as MethodCallExpression;
            if (methodCallExpression != null && methodCallExpression.Method.IsOperator())
            {
                return methodCallExpression.Method;
            }

            return null;
        }

        public static ExpressionDefinition Create(Delegate @delegate, ExpressionSettings settings)
        {
            if (@delegate == null) throw new ArgumentNullException("delegate");
            if (settings == null) throw new ArgumentNullException("settings");

            SetNameAndDescription(settings);
            SetDiagram(settings);

            return Create(settings, @delegate, @delegate.Method.GetParameters().ToDictionary(p => p.Name, p => p.ParameterType));
        }

        public static ExpressionDefinition Create(Delegate @delegate)
        {
            if (@delegate == null) throw new ArgumentNullException("delegate");
            return Create(@delegate, new ExpressionSettings());
        }

        public static ExpressionDefinition Create(LambdaExpression expression, ExpressionSettings settings)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            if (settings == null) throw new ArgumentNullException("settings");

            settings.Operator = settings.Operator ?? GetOperator(expression);

            SetNameAndDescription(settings);
            SetDiagram(settings);

            if (settings.CodeSample == null)
                settings.CodeSample = expression.ToString();

            return Create(settings, expression.Compile(), expression.Parameters.ToDictionary(p => p.Name, p => p.Type));
        }

        public static ExpressionDefinition Create(LambdaExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            return Create(expression, new ExpressionSettings());
        }

        private static void SetNameAndDescription(ExpressionSettings settings)
        {
            if (settings.Operator != null)
            {
                if (settings.Name == null)
                    settings.Name = settings.Operator.Name;

                if (settings.Description == null)
                    settings.Description = settings.Operator.GetSummary();
            }
        }

        private static void SetDiagram(ExpressionSettings settings)
        {
            if (settings.Diagram == null && _container != null)
            {
                var diagramId = settings.DiagramId ?? settings.Name;

                settings.Diagram = _container.Diagrams.FirstOrDefault(d => d.Id == diagramId);
            }
        }


        private static ExpressionDefinition Create(ExpressionSettings settings,
            Delegate expression, Dictionary<string, Type> sourceTypes)
        {
            // todo: walidacja expression (np sprawdzenie argumentow i rezultatu)

            return new ExpressionDefinition
            {
                CompiledExpression = expression,
                SourceTypes = sourceTypes,
                ExpressionString = settings.CodeSample,
                Name = settings.Name,
                Description = settings.Description,
                Diagram = settings.Diagram
            };
        }
    }

  


    public class ExpressionInstance : IDisposable
    {
        public Diagram Diagram { get; private set;}
        
        public ObservableInput this[string name]
        {
            get { return Inputs.FirstOrDefault(g => g.Name == name); }
        }

        public ExpressionDefinition Definition { get; private set; }
        public ReadOnlyCollection<ObservableInput> Inputs { get; private set; }
        public ReadOnlyCollection<ObservableSource> Output { get; private set; }

        private ExpressionInstance()
        { }

        public static ExpressionInstance Create(ExpressionDefinition definition)
        {
            // input
            var inputs = new ObservableCollection<ObservableInput>();
            foreach (var pair in definition.SourceTypes)
            {
                var inputType = typeof(ObservableInput<>).MakeGenericType(pair.Value.GetGenericArguments()[0]);
                var input = (ObservableInput)Activator.CreateInstance(inputType);
                input.Name = pair.Key;
                inputs.Add(input);
            }

            // create output
            var outputObject = definition
                .CompiledExpression
                .DynamicInvoke(inputs
                   .OfType<IObservableInput>()
                   .Select(g => g.Observable)
                   .ToArray());

            var outputType = typeof(ObservableOutput<>).MakeGenericType(
                definition.CompiledExpression.Method.ReturnType.GetGenericArguments()[0]);
            var output = (ObservableSource)Activator.CreateInstance(outputType, outputObject);

            return new ExpressionInstance
            {
                Definition = definition,
                Inputs = new ReadOnlyObservableCollection<ObservableInput>(inputs),
                Output = new ReadOnlyObservableCollection<ObservableSource>(new ObservableCollection<ObservableSource>(new []{output})),
                
            }.SetUpDiagram();
        }

        #region Diagram

        private Dictionary<string, Series> _diagramSeries;
        private IDisposable _diagramDisposable;

        private ExpressionInstance SetUpDiagram()
        {
            Diagram = new Diagram
            {
                Inputs = new ObservableCollection<Series>(Inputs.Select(g => new Series { Name = g.Name })),
                Output = new ObservableCollection<Series>(Output.Select(g => new Series { Name = g.Name })),
            };

            _diagramSeries = Diagram.ToSeriesDictionary();

            var _notifications =
                (
                    from g in Inputs.OfType<ObservableSource>().Concat(Output)
                    select
                        from v in g.ObservableStr.Materialize()
                        select new Record(g.Name, v)
                )
                .Merge()
                .Timestamp()
                .Scan((previous, current) => current.Value.IncrementOrder(previous, current));

            _diagramDisposable = _notifications.Subscribe(BuildDiagram);
            
            return this;
        }

        private void BuildDiagram(Timestamped<Record> record)
        {
            var point = new Marble
            {
                Kind = record.Value.Notification.Kind,
                Order = record.Value.Order,
                Value = record.Value.Notification.Kind == NotificationKind.OnNext ?
                    record.Value.Notification.Value : null
            };

            _diagramSeries[record.Value.Name].Marbles.Add(point);
        }

        private class Record
        {
            public string Name { get; set; }
            public Notification<string> Notification { get; set; }
            public int Order { get; set; }

            public Record(string name, Notification<string> value)
            {
                Name = name;
                Notification = value;
            }
            public Timestamped<Record> IncrementOrder(Timestamped<Record> previous, Timestamped<Record> current)
            {
                current.Value.Order = (current.Timestamp - previous.Timestamp).TotalMilliseconds < 50 &&
                    current.Value.Name != previous.Value.Name  //  nie mozna na sobie narysowac czegokolwiek
                    ? previous.Value.Order : previous.Value.Order + 1;
                return current;
            }
            public override string ToString()
            {
                return "[" + Name + " , " + Notification + " , " + Order + "]";
            }
        } 

        #endregion


        private bool _disposed;
        public void Dispose()
        {
            if (!_disposed)
            {
                _diagramDisposable.Dispose();
                _disposed = true;
            }
        }
    }


    public class ExpressionSettings
    {
        public string Name { get; set; }            
        public string Description { get; set; }     
        public string CodeSample { get; set; }      

        public MethodInfo Operator { get; set; }    
     
        public Diagram Diagram { get; set; }        
        public string DiagramId { get; set; }             
    }

}