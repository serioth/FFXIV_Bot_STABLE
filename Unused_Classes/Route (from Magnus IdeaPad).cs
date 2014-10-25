using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Unused_Classes
{
    class Route <T> : Queue<T>
    {
        public string From;
        public string To;
        public string Name;
        public List<WayPoint> Points;

        public Route(string name, string from, string to)
        {
            Points = new List<WayPoint>();
            Name = name;
            From = from;
            To = to;
        }

        public Route(string name)
            : this(name, null, null)
        {
        }

        public Route()
        {
        }

        public Route(XElement xml)
            : this(
                xml.Attribute("Name").Value,
                xml.Attribute("From").Value,
                xml.Attribute("To").Value)
        {
            foreach (XElement xpoint in xml.Descendants("Point"))
                Points.Add(new WayPoint(xpoint));
        }

        public XElement GetXml()
        {
            XElement xroute = new XElement("Route",
                new XAttribute("Name", Name),
                new XAttribute("From", From),
                new XAttribute("To", To));

            foreach (WayPoint point in Points)
                xroute.Add(point.GetXml());

            return xroute;
        }

        /// <summary>
        /// Dequeues the next item in the Queue and automatically pushes it to the back.
        /// This method hides the Queue&lt;T&gt;.Dequeue method on purpose!
        /// </summary>
        /// <returns>An object of type T.</returns>
        public new T Dequeue()
        {
            // For whatever reason, MS decided to not let their collections classes have virtual
            // functions that we can override easily. However, thankfully, they provided us with the
            // various uses of the 'new' keyword! Yay! Or is that fail? Oh well...
            T tmp = base.Dequeue();
            Enqueue(tmp);
            return tmp;
        }

        /// <summary>
        /// Cycles to the next occurance of the specified item. If no occurances are found,
        /// this method does nothing but waste CPU cycles!
        /// </summary>
        /// <param name="item">The item to set the Queue's current position to.</param>
        public void CycleTo(T item)
        {
            // Using a for loop here to avoid and endless cycle if no match is found!
            T tmp = Peek();
            for (int i = 0; i < Count; i++)
            {
                // If the 'item' object implements a custom Equals method, it will be used.
                // Otherwise, this pretty much just checks for reference equality.
                // Hint: IMPLEMENT PROPER EQUALITY OPERATORS!
                if (!tmp.Equals(item))
                {
                    // Toss it to the back.
                    Dequeue();
                    // Check the next one.
                    tmp = Peek();
                }
                else
                {
                    // We've hit our item. Stop here!
                    return;
                }
            }
            // No item was found, so make sure we end up where we started from.
            Dequeue();
        }

    }
}
