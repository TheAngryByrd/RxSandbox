using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using System.Threading;
using System.Reflection;

namespace RxSandbox.Tests
{

    [TestFixture]
    public class SampleDiagram
    {

        private DiagramContainer _container;

        [TestFixtureSetUp]
        public void SetUp()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);
            _container = new DiagramContainer();
        }

        [Test]
        public void Merge()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Merge();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");
                var b = instance.GetInput("b");
                var c = instance.GetInput("c");

                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                c.OnNext2(gen.Next());
                a.OnCompleted2();
                b.OnNext2(gen.Next());
                b.OnCompleted2();
                c.OnNext2(gen.Next());
                c.OnCompleted2();

                AddDiagram("Merge", instance.Diagram);
            }
        }



        [Test]
        public void Zip()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Zip();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");
                var b = instance.GetInput("b");

                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                a.OnCompleted2();
                b.OnNext2(gen.Next());
                b.OnCompleted2();

                AddDiagram("Zip", instance.Diagram);
            }
        }

        [Test]
        public void CombineLatest()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.CombineLatest();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");
                var b = instance.GetInput("b");

                a.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                a.OnCompleted2();
                b.OnNext2(gen.Next());
                b.OnCompleted2();

                AddDiagram("CombineLatest", instance.Diagram);
            }
        }

        [Test]
        public void Amb()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Amb();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");
                var b = instance.GetInput("b");

                a.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                b.OnCompleted2();
                a.OnCompleted2();

                AddDiagram("Amb", instance.Diagram);
            }
        }

        [Test]
        public void ForkJoin()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.ForkJoin();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");
                var b = instance.GetInput("b");

                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                b.OnCompleted2();
                a.OnCompleted2();

                AddDiagram("ForkJoin", instance.Diagram);
            }
        }

        [Test]
        public void Concat()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Concat();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");
                var b = instance.GetInput("b");

                b.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                a.OnCompleted2();
                b.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                b.OnCompleted2();

                AddDiagram("Concat", instance.Diagram);
            }
        }





        // linq


        [Test]
        public void Take()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Take();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnCompleted2();

                AddDiagram("Take", instance.Diagram);
            }
        }

        [Test]
        public void TakeWhile()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.TakeWhile();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2("a");
                a.OnNext2("aa");
                a.OnNext2("aaa");
                a.OnNext2("aaaa");
                a.OnNext2("aaaaa");
                a.OnNext2("a");
                a.OnNext2("aa");
                a.OnNext2("aaa");
                a.OnCompleted2();

                AddDiagram("TakeWhile", instance.Diagram);
            }
        }


        [Test]
        public void TakeUntil()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.TakeUntil();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");
                var b = instance.GetInput("b");

                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                b.OnCompleted2();
                a.OnCompleted2();

                AddDiagram("TakeUntil", instance.Diagram);
            }
        }

        [Test]
        public void Skip()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Skip();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnCompleted2();

                AddDiagram("Skip", instance.Diagram);
            }
        }

        [Test]
        public void SkipWhile()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.SkipWhile();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2("a");
                a.OnNext2("aa");
                a.OnNext2("aaa");
                a.OnNext2("aaaa");
                a.OnNext2("aaaaa");
                a.OnNext2("a");
                a.OnNext2("aa");
                a.OnNext2("aaa");
                a.OnCompleted2();

                AddDiagram("SkipWhile", instance.Diagram);
            }
        }

        [Test]
        public void SkipUntil()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.SkipUntil();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");
                var b = instance.GetInput("b");

                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                b.OnCompleted2();
                a.OnCompleted2();

                AddDiagram("SkipUntil", instance.Diagram);
            }
        }


        [Test]
        public void Where()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Where();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2("a");
                a.OnNext2("aa");
                a.OnNext2("aaa");
                a.OnNext2("aaaa");
                a.OnNext2("aaaaa");
                a.OnNext2("aaaaaa");
                a.OnCompleted2();

                AddDiagram("Where", instance.Diagram);
            }
        }


        [Test]
        public void Select()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Select();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2("a");
                a.OnNext2("aa");
                a.OnNext2("aaa");
                a.OnNext2("aaaa");
                a.OnNext2("aaaaa");
                a.OnNext2("aaaaaa");
                a.OnCompleted2();

                AddDiagram("Select", instance.Diagram);
            }
        }

        [Test]
        public void SelectMany()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.SelectMany();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2("2");
                a.OnCompleted2();

                Thread.Sleep(3000);

                AddDiagram("SelectMany", instance.Diagram);
            }
        }

        [Test]
        public void Aggregate()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Aggregate();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                Thread.Sleep(100);
                a.OnNext2("a");
                a.OnNext2("aa");
                a.OnNext2("aaa");
                a.OnNext2("aa");
                a.OnNext2("a");
                a.OnCompleted2();

                AddDiagram("Aggregate", instance.Diagram);
            }
        }

        [Test]
        public void Count()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Count();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnCompleted2();

                AddDiagram("Count", instance.Diagram);
            }
        }

        [Test]
        public void Sum()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Sum();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2("2");
                a.OnNext2("123");
                a.OnNext2("55");
                a.OnNext2("-50");
                a.OnCompleted2();

                AddDiagram("Sum", instance.Diagram);
            }
        }

        [Test]
        public void Max()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Max();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2("2");
                a.OnNext2("123");
                a.OnNext2("55");
                a.OnNext2("-50");
                a.OnCompleted2();

                AddDiagram("Max", instance.Diagram);
            }
        }

        [Test]
        public void Min()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Min();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2("2");
                a.OnNext2("123");
                a.OnNext2("55");
                a.OnNext2("-50");
                a.OnCompleted2();

                AddDiagram("Min", instance.Diagram);
            }
        }

        [Test]
        public void Average()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Average();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2("2");
                a.OnNext2("123");
                a.OnNext2("55");
                a.OnNext2("-50");
                a.OnCompleted2();

                AddDiagram("Average", instance.Diagram);
            }
        }


        [Test]
        public void All()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.All();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2("a");
                a.OnNext2("aa");
                a.OnNext2("aaa");
                a.OnNext2("aaaa");
                a.OnCompleted2();

                AddDiagram("All", instance.Diagram);
            }
        }

        [Test]
        public void Any()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Any();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2("aaaa");
                a.OnNext2("aaa");
                a.OnNext2("aa");
                a.OnNext2("a");
                a.OnCompleted2();

                AddDiagram("Any", instance.Diagram);
            }
        }



        [Test]
        public void GroupBy()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.GroupBy();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2("a");
                a.OnNext2("a");
                a.OnNext2("aa");
                a.OnNext2("aaa");
                a.OnNext2("aaa");
                a.OnNext2("aa");
                a.OnNext2("a");
                a.OnNext2("a");
                a.OnNext2("a");
                a.OnCompleted2();

                AddDiagram("GroupBy", instance.Diagram);
            }
        }

        //rx


        [Test]
        public void Delay()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Delay();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2(DateTime.Now.ToString("F"));
                a.OnNext2(DateTime.Now.ToString("F"));
                a.OnCompleted2();

                AddDiagram("Delay", instance.Diagram);
            }
        }

        [Test]
        public void Throttle()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Throttle();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2(DateTime.Now.ToString("F"));
                a.OnNext2(DateTime.Now.ToString("F"));
                Thread.Sleep(3000);
                a.OnNext2(DateTime.Now.ToString("F"));
                a.OnNext2(DateTime.Now.ToString("F"));
                a.OnCompleted2();

                AddDiagram("Throttle", instance.Diagram);
            }
        }


        [Test]
        public void Sample()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Sample();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2(DateTime.Now.ToString("F"));
                a.OnNext2(DateTime.Now.ToString("F"));
                Thread.Sleep(3000);
                a.OnNext2(DateTime.Now.ToString("F"));
                a.OnNext2(DateTime.Now.ToString("F"));
                a.OnCompleted2();

                AddDiagram("Sample", instance.Diagram);
            }
        }


        [Test]
        public void DistinctUntilChanged()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.DistinctUntilChanged();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                a.OnNext2("a");
                a.OnNext2("b");
                a.OnNext2("b");
                a.OnNext2("b");
                a.OnNext2("c");
                a.OnNext2("c");
                a.OnCompleted2();

                AddDiagram("DistinctUntilChanged", instance.Diagram);
            }
        }


        [Test]
        public void Scan()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Scan();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                Thread.Sleep(100);
                a.OnNext2("a");
                a.OnNext2("aa");
                a.OnNext2("aaa");
                a.OnNext2("aa");
                a.OnNext2("a");
                a.OnCompleted2();

                AddDiagram("Scan", instance.Diagram);
            }
        }


        [Test]
        public void BufferWithCount()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.BufferWithCount();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                Thread.Sleep(100);
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnCompleted2();

                AddDiagram("BufferWithCount", instance.Diagram);
            }
        }


        [Test]
        public void BufferWithTime()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.BufferWithTime();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");

                Thread.Sleep(100);
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                Thread.Sleep(3000);
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnCompleted2();

                AddDiagram("BufferWithTime", instance.Diagram);
            }
        }


        // exceptions


        [Test]
        public void Catch()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.Catch();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");
                var b = instance.GetInput("b");

                b.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnError2();
                b.OnNext2(gen.Next());
                b.OnNext2(gen.Next());

                AddDiagram("Catch", instance.Diagram);
            }
        }

        [Test]
        public void OnErrorResumeNext()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);

            var gen = new InputGenerator();
            var definition = StandardOperators.OnErrorResumeNext();

            using (var instance = ExpressionInstance.Create(definition))
            {
                var a = instance.GetInput("a");
                var b = instance.GetInput("b");

                b.OnNext2(gen.Next());
                b.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnNext2(gen.Next());
                a.OnCompleted2();
                b.OnNext2(gen.Next());
                b.OnNext2(gen.Next());

                AddDiagram("OnErrorResumeNext", instance.Diagram);
            }
        }

        //[Test]
        //public void Repeat()
        //{
        //    Console.WriteLine(MethodBase.GetCurrentMethod().Name);

        //    var gen = new InputGenerator();
        //    var definition = ExpressionProvider.Repeat();
        //    var viewModel = new ExpressionInstanceVM(definition);

        //    var a = viewModel.GetInput("a");

        //    a.OnNext2("a");
        //    a.OnNext2("b");
        //    a.OnNext2("");
        //    a.OnNext2("a");
        //    a.OnNext2("b");

        //    AddDiagram("Repeat", viewModel.Diagram);
        //}





        private void AddDiagram(string id, Diagram diagram)
        {

            diagram.Id = id;
            Console.WriteLine(XmlSerializationHelper.ToXml(diagram));
            _container.Diagrams.Add(diagram);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(XmlSerializationHelper.ToXml(_container));
        }
    }

    internal static class Extensions
    {
        internal static void OnNext2(this ObservableInput input, string value)
        {
            input.OnNext(value);
            Thread.Sleep(100);
        }
        internal static void OnError2(this ObservableInput input)
        {
            input.OnError(new Exception());
            Thread.Sleep(100);
        }
        internal static void OnCompleted2(this ObservableInput input)
        {
            input.OnCompleted();
            Thread.Sleep(100);
        }

        internal static ObservableInput GetInput(this ExpressionInstance instance, string inputName)
        {
            return instance.Inputs.First(g => g.Name == inputName);
        }
    }

    public class InputGenerator
    {
        private static readonly char[] _chars = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'r', 's', 't', 'u', 'w', 'y', 'z' };
        private int i = 0;

        public string Next()
        {
            return _chars[i++].ToString();
        }
    }
}
