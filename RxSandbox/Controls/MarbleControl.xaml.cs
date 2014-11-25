using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RxSandbox
{
    //[TemplateVisualState(Name = "None", GroupName = "Notification")]
    //[TemplateVisualState(Name = "OnNext", GroupName = "Notification")]
    //[TemplateVisualState(Name = "OnError", GroupName = "Notification")]
    //[TemplateVisualState(Name = "OnCompleted", GroupName = "Notification")]
    public partial class MarbleControl : UserControl
    {
        public MarbleControl()
        {
            InitializeComponent();
        }


        public Marble Marble
        {
            get { return (Marble)GetValue(MarbleProperty); }
            set { SetValue(MarbleProperty, value); }
        }
        public static readonly DependencyProperty MarbleProperty =
            DependencyProperty.Register("Marble", typeof(Marble), typeof(MarbleControl),
              new PropertyMetadata(null));





        //#region NotificationType (DependencyProperty)

        //public NotificationUIKind NotificationType
        //{
        //    get { return (NotificationUIKind)GetValue(NotificationTypeProperty); }
        //    set { SetValue(NotificationTypeProperty, value); }
        //}
        //public static readonly DependencyProperty NotificationTypeProperty = 
        //    DependencyProperty.Register("NotificationType", typeof(NotificationUIKind), typeof(MarbleControl), 
        //    new PropertyMetadata(NotificationUIKind.None, OnNotificationTypeChanged));
            
        //private static void OnNotificationTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((MarbleControl)d).OnNotificationTypeChanged(e);
        //}

        //protected virtual void OnNotificationTypeChanged(DependencyPropertyChangedEventArgs e)
        //{
        //    var notification = (NotificationUIKind) e.NewValue;
        //    GoToState(true);
        //}
            
        //#endregion

        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();

        //    GoToState(false);
        //}

        //private void GoToState(bool useTransitions)
        //{
        //    if (NotificationType == NotificationUIKind.None)
        //    {
        //        VisualStateManager.GoToState(this, "None", useTransitions);
        //    }
        //    else if (NotificationType == NotificationUIKind.OnError)
        //    {
        //        VisualStateManager.GoToState(this, "OnError", useTransitions);
        //    }
        //    else if (NotificationType == NotificationUIKind.OnNext)
        //    {
        //        VisualStateManager.GoToState(this, "OnNext", useTransitions);
        //    }
        //    else if (NotificationType == NotificationUIKind.OnCompleted)
        //    {
        //        VisualStateManager.GoToState(this, "OnCompleted", useTransitions);
        //    }
        //}
    }

  


}
