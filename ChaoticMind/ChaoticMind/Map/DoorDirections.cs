using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind
{


    class DoorDirections
    {
        public static DoorDirections NORTH = new DoorDirections(true, false, false, false);
        public static DoorDirections SOUTH = new DoorDirections(false, true, false, false);
        public static DoorDirections EAST = new DoorDirections(false, false, true, false);
        public static DoorDirections WEST = new DoorDirections(false, false, false, true);

        bool _north;
        bool _south;
        bool _east;
        bool _west;

        public DoorDirections(bool north, bool south, bool east, bool west)
        {
            _north = north;
            _south = south;
            _east = east;
            _west = west;

        }

        public static DoorDirections RandomDoors() {
            int combo = Utilities.randomInt() % (1 << 3) + 1;
            
            //Prevent single doors
            while (combo == 1 || combo == 2 || combo == 4 || combo == 8){
                combo = Utilities.randomInt() % (1 << 3) + 1;
            }

            bool north = (combo & 1) != 0;
            bool south = (combo & 2) != 0;
            bool east = (combo & 4) != 0;
            bool west = (combo & 8) != 0;

            return new DoorDirections(north, south, east, west);
        }

        public int toIndex() {
            if (numberOfDoors > 1) {
                throw new Exception("Cannot convert multiple doors to an index.");
            }
            else if (numberOfDoors == 0)
            {
                throw new Exception("Cannot convert no doors to an index.");
            }
            if (_north) return 0;
            else if (_south) return 1;
            else if (_east) return 2;
            else return 3;
        }

        private int numberOfDoors
        {
            get { return (_north ? 1 : 0) + (_south ? 1 : 0) + (_east ? 1 : 0) + (_west ? 1 : 0); }
        }

        public bool hasNorth
        {
            get { return _north; }
        }
        public bool hasSouth
        {
            get { return _south; }
        }
        public bool hasEast
        {
            get { return _east; }
        }
        public bool hasWest
        {
            get { return _east; }
        }
        
    }
}
