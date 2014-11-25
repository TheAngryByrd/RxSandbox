using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using System.Diagnostics;
using RxSandbox.Properties;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace RxSandbox
{
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            Init();
        }

        private void PopulateExpressionNodes(IEnumerable<ExpressionDefinition> expressions,
            TreeViewItem node)
        {            
            foreach (var o in expressions)
            {
                var item = new TreeViewItem { Header = o.Name, Tag = o };
                item.MouseDoubleClick += item_MouseDoubleClick;
                node.Items.Add(item);
            }
        }

        private void Init()
        {
            var assemblies = new [] {Assembly.GetExecutingAssembly().FullName,
                Settings.Default.ExtensionsAssembly}.Distinct();

            foreach (var assemblyName in assemblies)
            {
                if (!string.IsNullOrEmpty(assemblyName))
                {
                    try
                    {
                        var assembly = Assembly.Load(assemblyName);
                        if (assembly != null)
                        {
                            var q =
                                from t in assembly.GetTypes()
                                where typeof(IExpressionProvider).IsAssignableFrom(t) &&
                                      t.GetConstructor(new Type[0]) != null && !t.IsAbstract
                                let instance = Activator.CreateInstance(t) as IExpressionProvider
                                select new { instance, expressions = instance.GetExpressions().ToArray() };

                            foreach (var ins in q.ToArray())
                            {
                                var node = new TreeViewItem { Header = ins.instance.GetType().Name, IsExpanded = true };
                                PopulateExpressionNodes(ins.expressions, node);
                                operatorsNode.Items.Add(node);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Loading extension assembly failed.");
                    }
                }
            }

           
        }

        private void item_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var definition = ((sender as TreeViewItem).Tag as ExpressionDefinition);

            var instanceVM = new ExpressionInstanceVM(definition);            
            var control = new ExpressionControl {DataContext = instanceVM};
            var item = new TabItem
            {
                Tag = definition.Name,
                Header = definition.Name,
                Content = control
            };
            
            EventHandler closeHandler = null;
            closeHandler = (o, ea) =>
                               {
                                   tabControl.Items.Remove(item);
                                   instanceVM.CloseRequested -= closeHandler;
                               };
            instanceVM.CloseRequested += closeHandler;

            tabControl.Items.Add(item);
            item.Focus();     
        }


        protected override void OnClosed(EventArgs e)
        {
            foreach (TabItem tabItem in tabControl.Items)
                ((tabItem.Content as ExpressionControl).DataContext as ExpressionInstanceVM).CleanUp();

            base.OnClosed(e);
        }
       
    }
}
