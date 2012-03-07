using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind
{
    public enum ComboType
    {
        STRAIGHT = 0,
        BENT = 1,
        TRIPLE = 2
    }

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

        public ComboType Type {
            get
            {
                if (NumberOfDoors == 3)
                {
                    return ComboType.TRIPLE;
                }

                if (hasNorth && hasSouth || hasEast && hasWest)
                {
                    return ComboType.STRAIGHT;
                }
                else
                {
                    return ComboType.BENT;
                }
            }
            }

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
            while (combo == 1 || combo == 2 || combo == 4 || combo == 8)
            {
                combo = Utilities.randomInt() % ((1 << 4) - 1) + 1;
            }

            bool north = false, south = false, east = false, west = false;

            if ((combo & 1) != 0)
            {
                north = true;
            }
            if ((combo & 2) != 0)
            {
                south = true;
            }
            if ((combo & 4) != 0)
            {
                east = true;
            }
            if ((combo & 8) != 0)
            {
                west = true;
            }

            return new DoorDirections(north, south, east, west);
        }

        public int toIndex() {
            if (NumberOfDoors > 1) {
                throw new Exception("Cannot convert multiple doors to an index.");
            }
            else if (NumberOfDoors == 0)
            {
                throw new Exception("Cannot convert no doors to an index.");
            }
            if (_north)  
            {
                return 0;
            }
            else if (_south) 
            {
                return 1;
            }
            else if (_east)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        public int NumberOfDoors
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
            get { return _west; }
        }


        //This method accounts for the fact that only 3 tile images are used, and simply rotated to fith the correct orientation.
        internal float imageRotation()
        {
            float rads = 0.0f;
            switch (Type)
            {
                case ComboType.STRAIGHT:
                    if (!hasNorth)
                    {
                        return (float)Math.PI / 2;
                    }
                    break;
                case ComboType.BENT:
                    if (hasEast && hasSouth)
                    {
                        return (float)Math.PI / 2;
                    }
                    else if (hasSouth && hasWest)
                    {
                        return (float)Math.PI;
                    }
                    else if (hasWest && hasNorth)
                    {
                        return (float)(-Math.PI / 2);
                    }
                    break;
                case ComboType.TRIPLE:
                    if (!hasNorth)
                    {
                        return (float)Math.PI / 2;
                    }
                    else if (!hasEast)
                    {
                        return (float)Math.PI;
                    }
                    else if (!hasSouth)
                    {
                        return (float)(-Math.PI / 2);
                    }
                    break;

            }
            return rads;
        }
    }
}
