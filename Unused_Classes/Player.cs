using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unused_Classes
{
    class Player
    {
        //private int _playerStructureBaseAddress, _petHP, _maxPetHP, _targetSelected;
        //private string _name, _playerHP, _playerMaxHP, _playerMP, _playerMaxMP, _playerTP, _playerMaxTP;
        //private float _positionX, _positionY, _positionZ, _facingAngle, _targetDistance;
        //private ushort _targetID;

        public Player()
        {

        }


        public static bool IsMoving
        {
            get { return true; }
        }

        public static WayPoint Location
        {
            get { return new WayPoint(0, 0, 0); }
        }

        public static void Face(WayPoint pt)
        { }

        public static bool IsFacing(WayPoint pt)
        {
            return false;
        }

        public static void MoveForward()
        { }

        public static void Stop()
        { }
    }
}
