using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Unused_Classes
{
    public struct WayPoint : IEquatable<WayPoint> //IEquatable allows us to override .Equals, in order to check equality between two instances in a custom way
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        //:this() allows you to call one constructor from another within the same class
        public WayPoint(float x, float y, float z)
            : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public WayPoint(XElement xml)
            : this(Convert.ToSingle(xml.Attribute("X").Value),
                   Convert.ToSingle(xml.Attribute("Y").Value),
                   Convert.ToSingle(xml.Attribute("Z").Value)) //Calls the base constructor with these parameters
        { }

        public double Distance(WayPoint to)
        {
            return Distance(to.X, to.Y, to.Z);
        }

        public double Distance(float toX, float toY, float toZ)
        {
            float dX = X - toX;
            float dY = Y - toY;
            float dZ = Z - toZ;
            return Math.Sqrt(dX * dX + dY * dY + dZ * dZ);
        }

        public double Angle(float toX, float toY)
        {
            float dX = X - toX;
            float dY = Y - toY;

            return Math.Atan2(dY, dX);
        }

        public XElement GetXml()
        {
            return new XElement("Point", new XAttribute("X", X), new XAttribute("Y", Y), new XAttribute("Z", Z));
        }

        public bool Equals(WayPoint other)
        {
            return other.X == X && other.Y == Y && other.Z == Z;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (obj.GetType() != typeof(WayPoint))
            {
                return false;
            }
            return Equals((WayPoint)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = X.GetHashCode();
                result = (result * 397) ^ Y.GetHashCode();
                result = (result * 397) ^ Z.GetHashCode();
                return result;
            }
        }

        public static bool operator ==(WayPoint left, WayPoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(WayPoint left, WayPoint right)
        {
            return !left.Equals(right);
        }



    }
}
