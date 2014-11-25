using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Collections.Generic;
using RxSandbox.Properties;

namespace RxSandbox
{

    
    public static class Extensions
    {
        public static Dictionary<string, Series> ToSeriesDictionary(this Diagram diagram)
        {
            return GetSeries(diagram).ToDictionary(s => s.Name ?? "");
        }

        public static IEnumerable<Series> GetSeries(this Diagram diagram)
        {
            return diagram.Inputs.StartWith(diagram.Output);
        }
    }



    public static class TypeExtensions
    {
        public static bool IsOperator(this MethodInfo methodInfo)
        {
            return methodInfo.DeclaringType == typeof (Observable) ||
                   methodInfo.DeclaringType == typeof (EnumerableEx);
        }

        public static string GetSummary(this MethodInfo methodInfo, string folderPath)
        {
            if (!methodInfo.IsOperator())
                throw new ArgumentException("Method is not an operator implementation.");

            string filePath = Path.ChangeExtension(Path.Combine(folderPath, Path.GetFileName(methodInfo.DeclaringType.Assembly.Location)), "xml");

            if (!File.Exists(filePath))
                return null;

            XElement xElement;
            try
            {
                var methodName = "M:" + methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
                var xDocument = XDocument.Load(filePath);
                xElement = xDocument.Root.Element("members").Elements("member").FirstOrDefault(
                    x => x.Attribute("name").Value.StartsWith(methodName));
            }
            catch (Exception e)
            {
                return null;
            }

            if (xElement == null)
                return null;

            return xElement.Element("summary").Value.Trim();
        }

        public static string GetSummary(this  MethodInfo methodInfo)
        {
            return GetSummary(methodInfo,                 
                string.IsNullOrEmpty(Settings.Default.DocumentationFolderPath) 
                ? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                : Settings.Default.DocumentationFolderPath);
        }

        public static string GetSummary2(this MethodInfo methodInfo, string folderPath)
        {
            if (!methodInfo.IsOperator())
                throw new ArgumentException("Method is not an operator implementation.");




            string filePath = Path.ChangeExtension(Path.Combine(folderPath, Path.GetFileName(methodInfo.DeclaringType.Assembly.Location)), "xml");

            if (!File.Exists(filePath))
                return null;

            XElement xElement;
            try
            {
                string methodName2 = string.Format("M:{0}.{1}{2}",
                                                   GetTypeName(methodInfo),
                                                   GetMethodName(methodInfo),
                                                   GetMethodParameters(methodInfo));

                var methodName = "M:" + methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
                var xDocument = XDocument.Load(filePath);
                xElement = xDocument.Root.Element("members").Elements("member").FirstOrDefault(
                    x => x.Attribute("name").Value.StartsWith(methodName));
            }
            catch (Exception e)
            {
                return null;
            }

            if (xElement == null)
                return null;

            return xElement.Element("summary").Value.Trim();
        }

        private static string GetMethodParameters(MethodInfo info)
        {
            if (info.GetParameters().Length == 0)
                return "";

            return string.Join(",", info.GetParameters().Select( s => GetParameterTypeName(s)).ToArray());
        }

        private static string GetParameterTypeName(ParameterInfo info)
        {
            if (!info.ParameterType.IsGenericType)
                return info.ParameterType.FullName;

            var method = info.Member as MethodInfo;
            var type = method.DeclaringType;

            return "";
        }
        private static string GetMethodName(MethodInfo info)
        {
            return info.Name +
                   (info.IsGenericMethod ? "``" + info.GetGenericArguments().Length : "");
        }

        private static string GetTypeName(MethodInfo info)
        {
            var type = info.DeclaringType;
            return type.FullName +
                   (type.IsGenericTypeDefinition ? "`" + type.GetGenericArguments().Length : "");
        }

        public static string GetSummary2(this  MethodInfo methodInfo)
        {
            return GetSummary(methodInfo,
                              Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }



    }
}