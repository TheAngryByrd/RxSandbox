using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Specialized;
using System.Windows.Shapes;
using System.Windows.Media;

namespace RxSandbox
{
	public partial class MarbleDiagramControl : UserControl
	{
        public Diagram Diagram
        {
            get { return (Diagram)GetValue(DiagramProperty); }
            set { SetValue(DiagramProperty, value); }
        }

        public static readonly DependencyProperty DiagramProperty =
            DependencyProperty.Register("Diagram", typeof(Diagram), typeof(MarbleDiagramControl),
            new PropertyMetadata(null, OnDiagramChanged));

	    
	    private static void OnDiagramChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MarbleDiagramControl)d).OnDiagramChanged(e);
        }


        
        protected virtual void OnDiagramChanged(DependencyPropertyChangedEventArgs e)
        { 
            var old = e.OldValue as Diagram;
            var @new = e.NewValue as Diagram;

            if (old != null)
            {
                _disposable.Dispose();
            }

            Clear();

            if (@new != null)
            {
                IEnumerable<IObservable<EventPattern<NotifyCollectionChangedEventArgs>>> q =
                    from s in @new.GetSeries()
                    select Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(s.Marbles, "CollectionChanged");
                

                _disposable = q.Merge().ObserveOnDispatcher().Subscribe(x => CollectionChanged(x.Sender, x.EventArgs));
                
                RepaintDefinition();
            }
        }

        
        private readonly Dictionary<Series, int> _seriesRows = new Dictionary<Series, int>();
	    private IDisposable _disposable;

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            NotifyCollectionChangedEventArgs e = eventArgs;
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var collection = sender as ObservableCollection<Marble>;
                if (collection == null)
                    return;

                var series = Diagram.GetSeries().First(s => s.Marbles == collection);
                var marble = e.NewItems[0] as Marble;
                string style = "s:" + marble.Kind;

                var marbleControl = new MarbleControl
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Style = Application.Current.Resources[style] as Style,
                    DataContext = marble
                };

                int column = marble.Order + 1;
                int row = _seriesRows[series];
                marbleControl.SetValue(Grid.ColumnProperty, column);
                marbleControl.SetValue(Grid.RowProperty, row);


                if (column + 2 > grid.ColumnDefinitions.Count)
                {
                    for (int i = grid.ColumnDefinitions.Count -1; i < column + 1; i++)
                    {
                        var columnDefinition = new ColumnDefinition { Width = new GridLength(40) };
                        grid.ColumnDefinitions.Insert(i, columnDefinition);
                    }

                    foreach (var rec in grid.Children.OfType<Rectangle>())
                    {
                        rec.SetValue(Grid.ColumnSpanProperty, grid.ColumnDefinitions.Count - 1);
                    }
                }

                grid.Children.Add(marbleControl);
            }
        }

	    private void Clear()
        {
	        grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.RemoveRange(1, grid.ColumnDefinitions.Count - 2);            
            _seriesRows.Clear();
        }

        private void RepaintDefinition()
        {
            var brush = new BrushConverter().ConvertFromString("#FF1E1E90") as Brush;
            int rowIndex = 0;

            foreach (var series in Diagram.Inputs.Concat(new [] {Diagram.Output}) )
            {
                var row = new RowDefinition { Height = new GridLength(40) };

                var textBlock = new TextBlock {Text = series.Name, VerticalAlignment = System.Windows.VerticalAlignment.Center};
                textBlock.SetValue(Grid.RowProperty, rowIndex);
                textBlock.SetValue(Grid.ColumnProperty, 0);
                
                var line = new Rectangle { Name="line", VerticalAlignment = VerticalAlignment.Center, Height = 3, Margin = new Thickness(0, 0, 10, 0),Fill = brush};
                line.SetValue(Grid.RowProperty, rowIndex);
                line.SetValue(Grid.ColumnProperty, grid.ColumnDefinitions.Count -1);

                grid.RowDefinitions.Add(row);
                grid.Children.Add(textBlock);
                grid.Children.Add(line);

                _seriesRows.Add(series, rowIndex);
                ++rowIndex;
            }

            var q =
                from s in Diagram.GetSeries()
                from m in s.Marbles
                select new {s, m};


            foreach (var v in q)
            {
               
                
                CollectionChanged(v.s.Marbles, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, v.m));
            }
        }

        

		public MarbleDiagramControl()
		{
			this.InitializeComponent();
		}
	}
}