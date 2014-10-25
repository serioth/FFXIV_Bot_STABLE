using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Unused_Classes
{
    class RouteManager
    {
        public List<Route<WayPoint>> Routes;

        public RouteManager()
        {
            Routes = new List<Route<WayPoint>>();
        }

        public RouteManager(string filePath)
            : this()
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Could not find the specified file!", filePath);
            }

            XElement file = XElement.Load(filePath);

            IEnumerable<XElement> xroutes = file.Descendants("Route");
            foreach (XElement xroute in xroutes)
            {
                Route<WayPoint> route = new Route<WayPoint>(xroute);
                Routes.Add(route);
            }
        }

        public void Save(string filePath)
        {
            XElement file = new XElement("RoutesManager");

            foreach (Route<WayPoint> route in Routes)
                file.Add(route.GetXml());

            file.Save(filePath);
        }
    }
}
