using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;

namespace RxSandbox
{
    public class StandardOperators : ExpressionAttributeBasedProvider
    {
        // combinators

        [Expression]
        public static ExpressionDefinition Merge()
        {

            Expression<Func<IObservable<string>, IObservable<string>, IObservable<string>,
                IObservable<string>>> expression
                    = (a, b, c) => Observable.Merge(a, b, c);
            
            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition Zip()
        {
            Expression<Func<IObservable<string>, IObservable<string>,
                IObservable<string>>> expression
                    = (a, b) => a.Zip(b, (x, y) => x + " - " + y);

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition CombineLatest()
        {
            Expression<Func<IObservable<string>, IObservable<string>,
              IObservable<string>>> expression
                  = (a, b) => a.CombineLatest(b, (x, y) => x + " - " + y);

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition Amb()
        {
            Expression<Func<IObservable<string>, IObservable<string>,
              IObservable<string>>> expression
                  = (a, b) => a.Amb(b);

            return ExpressionDefinition.Create(expression);
        }


        [Expression]
        public static ExpressionDefinition Concat()
        {
            Expression<Func<IObservable<string>, IObservable<string>,
                IObservable<string>>> expression
                    = (a, b) => a.Concat(b);

            return ExpressionDefinition.Create(expression);
        }


        

        //todo: repeat
   



        //linq


        [Expression]
        public static ExpressionDefinition Take()
        {
            Expression<Func<IObservable<string>, IObservable<string>>> expression
                    = (a) => a.Take(4);

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition TakeWhile()
        {
            Expression<Func<IObservable<string>, IObservable<string>>> expression
                    = (a) => a.TakeWhile( i => i.Length < 3);

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition TakeUntil()
        {
            Expression<Func<IObservable<string>, IObservable<string>,
                IObservable<string>>> expression
                    = (a, b) => a.TakeUntil(b);

            return ExpressionDefinition.Create(expression);
        }


        [Expression]
        public static ExpressionDefinition Skip()
        {
            Expression<Func<IObservable<string>, IObservable<string>>> expression
                    = (a) => a.Skip(4);

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition SkipWhile()
        {
            Expression<Func<IObservable<string>, IObservable<string>>> expression
                    = (a) => a.SkipWhile(i => i.Length < 3);

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition SkipUntil()
        {

            Expression<Func<IObservable<string>, IObservable<string>,
                IObservable<string>>> expression
                    = (a, b) => a.SkipUntil(b);

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition Where()
        {
            Expression<Func<IObservable<string>,
                IObservable<string>>> expression
                    = (a) => a.Where( i => i.Length % 2 == 0 );

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition Select()
        {
            Expression<Func<IObservable<string>,
                IObservable<int>>> expression
                    = (a) => a.Select(i => i.Length);

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition SelectMany()
        {
            Expression<Func<IObservable<long>, IObservable<long>>> expression
                = a => a.SelectMany(v => Observable.Interval(TimeSpan.FromMilliseconds(1000)).Select(_ => v).Take((int)v));

            return ExpressionDefinition.Create(expression);
        }


        // tutaj -> tak samo jak scan

        [Expression]
        public static ExpressionDefinition Aggregate()
        {            
            Expression<Func<IObservable<string>,
                IObservable<int>>> expression
                    = (a) => a.Aggregate(0, (acc, v) => acc + v.Length);

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition Count()
        {
            Expression<Func<IObservable<string>,
                IObservable<int>>> expression
                    = a => a.Count();

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition Sum()
        {            
            Expression<Func<IObservable<int>,
                IObservable<int>>> expression
                    = a => a.Sum();

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition Max()
        {
            Expression<Func<IObservable<int>,
                IObservable<int>>> expression
                    = a => a.Max();

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition Min()
        {
            Expression<Func<IObservable<int>,
                IObservable<int>>> expression
                    = a => a.Min();

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition Average()
        {
            Expression<Func<IObservable<double>,
                IObservable<double>>> expression
                    = a => a.Average();

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition All()
        {
            Expression<Func<IObservable<string>,
                IObservable<bool>>> expression
                    = a => a.All(i => i.Length < 3);

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition Any()
        {
            Expression<Func<IObservable<string>,
                IObservable<bool>>> expression
                    = a => a.Any(i => i.Length < 3);

            return ExpressionDefinition.Create(expression);
        }


        [Expression]
        public static ExpressionDefinition GroupBy()
        {
            string code =
@"
from s in a
group s by s.Length
  into gr
  from ss in gr
  select ""Key: "" + gr.Key + ""  Value: "" + ss;
";
            Expression<Func<IObservable<string>, IObservable<string>>> expression = a =>
              from s in a
              group s by s.Length
              into gr
                  from ss in gr
                  select "Key: " + gr.Key + "  Value: " + ss;

            var @operator = ReflectionHelper.GetMethod(
                () => Observable.GroupBy<string, string>(null, _ => _));

            return ExpressionDefinition.Create(expression,
                new ExpressionSettings { Operator = @operator, CodeSample = code});
        }


        //exceptions


        [Expression]
        public static ExpressionDefinition Catch()
        {
            Expression<Func<IObservable<string>, IObservable<string>,
                IObservable<string>>> expression
                    = (a, b) => a.Catch(b);
  
            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition OnErrorResumeNext()
        {
            Expression<Func<IObservable<string>, IObservable<string>,
                IObservable<string>>> expression
                    = (a, b) => a.OnErrorResumeNext(b);

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition Repeat()
        {
            Expression<Func<IObservable<string>,IObservable<string>>> expression
                    = a => a
                        .SelectMany( v => string.IsNullOrEmpty(v) ? Observable.Throw<string>(new Exception()) : Observable.Return(v))
                        .Retry();
            
            return ExpressionDefinition.Create(expression);
        }




        //rx

        [Expression]
        public static ExpressionDefinition Delay()
        {

            Expression<Func<IObservable<string>,
                IObservable<string>>> expression
                    = (a) => a.Delay(TimeSpan.FromSeconds(2));

            return ExpressionDefinition.Create(expression);
        }


        [Expression]
        public static ExpressionDefinition Throttle()
        {
            Expression<Func<IObservable<string>,
                IObservable<string>>> expression
                    = (a) => a.Throttle(TimeSpan.FromSeconds(2));

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition Sample()
        {
            Expression<Func<IObservable<string>,
                IObservable<string>>> expression
                    = (a) => a.Sample(TimeSpan.FromSeconds(2));

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition DistinctUntilChanged()
        {
            Expression<Func<IObservable<string>,
                IObservable<string>>> expression
                    = (a) => a.DistinctUntilChanged();

            return ExpressionDefinition.Create(expression);
        }


        [Expression]
        public static ExpressionDefinition Scan()
        {
            Expression<Func<IObservable<string>,
                IObservable<int>>> expression
                    = (a) => a.Scan( 0, (acc, v) => acc + v.Length);

            return ExpressionDefinition.Create(expression);
        }

        [Expression]
        public static ExpressionDefinition BufferWithCount()
        {
            Expression<Func<IObservable<string>,
                IObservable<string>>> expression
                    = a => a.Buffer(3).
                        Select(i => string.Join(",", i.ToArray()));

            var @operator = ReflectionHelper.GetMethod(
                () => Observable.Buffer<string>(null,3));

            return ExpressionDefinition.Create(expression, 
                new ExpressionSettings{Operator = @operator});
        }

        [Expression]
        public static ExpressionDefinition BufferWithTime()
        {
            Expression<Func<IObservable<string>,
                IObservable<string>>> expression
                    = a => a.Buffer(TimeSpan.FromSeconds(3)).
                        Select(i => string.Join(",", i.ToArray()));

            var @operator = ReflectionHelper.GetMethod(
                () => Observable.Buffer<string>(null,TimeSpan.FromSeconds(3)));

            return ExpressionDefinition.Create(expression,
                new ExpressionSettings { Operator = @operator });
        }

    }

    public class CustomExpressions : ExpressionAttributeBasedProvider
    {
        [Expression]
        public static ExpressionDefinition Merge()
        {
            Expression<Action> temp = () => Observable.Merge(new IObservable<string>[0]);
            var @operator = (temp.Body as MethodCallExpression).Method;

            Func<IObservable<string>, IObservable<string>, IObservable<string>,
                IObservable<string>> expression
                    = (a, b, c) => Observable.Merge(a, b, c);

            return ExpressionDefinition.Create(expression, new ExpressionSettings
                { Operator = @operator });
        }

        [Expression]
        public static ExpressionDefinition IncrementalSearch()
        {
            Func<IObservable<string>, IObservable<Person>, IObservable<Person>> expression
                    = (codeChanged, webServiceCall) =>
                          {
                            var q =
                                from code in codeChanged
                                from x in Observable.Return(new Unit()).Delay(TimeSpan.FromSeconds(4)).TakeUntil(codeChanged)
                                from result in webServiceCall.TakeUntil(codeChanged)
                                select result;

                              return q;
                          };

            return ExpressionDefinition.Create(expression, new ExpressionSettings
               {
                   Name = "Incremental find",
                   Description = @"Send the code of the person you are looking for, "
                        + "after four seconds (if you don't send new code again) web service "
                        + "will be called. The result won't be returned if new code is provided "
                        + "in the meantime.",                   
               });
        }
    }

    [TypeConverter(typeof(PersonConverter))]
    public class Person
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class PersonConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof (string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }
        
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] v = ((string)value).Split(new[] { ',' });
                return new Person {Code = v[0], Name = v[1]};
            }
            return base.ConvertFrom(context, culture, value);
        }
        
        public override object ConvertTo(ITypeDescriptorContext context,
           CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof (string))
            {
                var person = value as Person;
                return person.Code + "," + person.Name;
            }                
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [AttributeUsage(AttributeTargets.Method,AllowMultiple = false, Inherited = true)]
    public class ExpressionAttribute : Attribute { }

    public interface IExpressionProvider
    {
        IEnumerable<ExpressionDefinition> GetExpressions();
    }

    public abstract class ExpressionAttributeBasedProvider : IExpressionProvider
    {
        public IEnumerable<ExpressionDefinition> GetExpressions()
        {
            var q =
                from m in this.GetType().GetMethods()
                let attr = Attribute.GetCustomAttribute(m, typeof(ExpressionAttribute)) as ExpressionAttribute
                where attr != null
                select m.Invoke(null, null) as ExpressionDefinition;
            return q.ToList();
        }
    }
}