using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace RxSandbox
{
    public partial class OutputControl : UserControl
    {
        public static readonly DependencyProperty ResultsProperty =
            DependencyProperty.Register("Results", typeof(ObservableCollection<string>),
            typeof(OutputControl),
            new UIPropertyMetadata(new ObservableCollection<string>()));

        public ObservableCollection<string> Results
        {
            get { return (ObservableCollection<string>)GetValue(ResultsProperty); }
            set { SetValue(ResultsProperty, value); }
        }

        public OutputControl()
        {
            InitializeComponent();
        }
    }
}
