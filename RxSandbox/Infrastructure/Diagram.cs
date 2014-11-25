using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Xml.Serialization;
using System.Collections;
using System.Windows.Markup;

namespace RxSandbox
{
    public class Marble
    {
        [XmlAttribute]
        [DefaultValue(null)]
        public string Value { get; set; }
        
        [XmlAttribute]
        [DefaultValue(false)]
        public bool IsNull { get; set; }
        
        [XmlAttribute]
        [DefaultValue(NotificationKind.OnNext)]
        public NotificationKind Kind { get; set; }
        
        [XmlAttribute]
        public int Order { get; set; }

        public Marble()
        {
            
            IsNull = false;
            Kind = NotificationKind.OnNext;
        }
    }


    [ContentProperty("Marbles")]
    public class Series
    {
        [XmlAttribute]
        [DefaultValue("")]
        public string Name { get; set; }

        //[XmlAttribute]
        //public string TypeName { get; set; }

        [XmlElement("Marble")]
        public ObservableCollection<Marble> Marbles { get; set; }

        public Series()
        {
            Name = "";
            Marbles = new ObservableCollection<Marble>();
        }

    }

    [ContentProperty("Inputs")]
    public class Diagram
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlElement("Input")]
        public ObservableCollection<Series> Inputs { get; set; }

        public Series Output { get; set; }

        public Diagram()
        {
            Inputs = new ObservableCollection<Series>();
        }
    }
    
    public class DiagramContainer
    {
        [XmlElement("Diagram")]
        public ObservableCollection<Diagram> Diagrams { get; set; }

        public DiagramContainer()
        {
            Diagrams = new ObservableCollection<Diagram>();
        }
    }
}
