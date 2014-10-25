using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unused_Classes
{
    //From some outer class: Navigation n = new Navigation(LoadFile("C:/xyz/.."); And set n.NavigateWaypoints() in a loop(foreach waypoint in the loaded file), containing n.SetDestinationToNearest();
    class Navigation
    {
        private Route<WayPoint> _waypoints;

        public Navigation(Route<WayPoint> waypoints)
        {
            _waypoints = waypoints;
        }

        public void NavigateWaypoints()
        {
            // Set our destination point that we'll be using throughout this method.
            WayPoint dest = _waypoints.Peek();

            // Are we close to the current waypoint? If so, push it to the end and move to the next.
            if (dest.Distance(Player.Location) < 3)
            {
                _waypoints.Dequeue();
                return;
            }

            if (!Player.IsFacing(dest))
            {
                Player.Face(dest);
            }

            if (!Player.IsMoving)
            {
                Player.MoveForward();
            }
        }

        public WayPoint GetClosestToMe()
        {
            // Store my location.
            WayPoint myLoc = Player.Location;

            // Since we want the closest, we start with the highest possible value
            // to make sure we have some valid data!
            double closest = double.MaxValue;

            WayPoint ret = new WayPoint();

            // Cycle through, testing distance, and set our return Point to the closest
            // one we find.
            foreach (WayPoint p in _waypoints)
            {
                if (p.Distance(myLoc) < closest)
                {
                    closest = p.Distance(myLoc);
                    ret = p;
                }
            }
            return ret;
        }

        public void SetDestinationToNearest()
        {
            _waypoints.CycleTo(GetClosestToMe());
        }

    }
}
