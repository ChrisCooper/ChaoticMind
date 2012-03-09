using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind {
    public enum ComboType {
        STRAIGHT = 0,
        BENT = 1,
        TRIPLE = 2,
        SINGLE = 3,
        NONE = 4
    }

    class DoorDirections {
        public static DoorDirections NORTH = new DoorDirections(true, false, false, false);
        public static DoorDirections SOUTH = new DoorDirections(false, true, false, false);
        public static DoorDirections EAST = new DoorDirections(false, false, true, false);
        public static DoorDirections WEST = new DoorDirections(false, false, false, true);

        bool _north;
        bool _south;
        bool _east;
        bool _west;

        public ComboType Type {
            get {
                if (NumberOfDoors == 0){
                    return ComboType.NONE;
                }
                else if (NumberOfDoors == 1) {
                    return ComboType.SINGLE;
                }
                else if (NumberOfDoors == 3){
                    return ComboType.TRIPLE;
                }
                if (hasNorth && hasSouth || hasEast && hasWest) {
                    return ComboType.STRAIGHT;
                }
                else {
                    return ComboType.BENT;
                }
            }
        }

        public DoorDirections(bool north, bool south, bool east, bool west) {
            _north = north;
            _south = south;
            _east = east;
            _west = west;

        }

        public static DoorDirections RandomDoors() {
            int combo = randomDirection();

            //Prevent single doors
            while (combo == 1 || combo == 2 || combo == 4 || combo == 8) {
                combo = randomDirection();
            }

            bool north = (combo & 1) != 0;
            bool south = (combo & 2) != 0;
            bool east = (combo & 4) != 0;
            bool west = (combo & 8) != 0;

            DoorDirections newDirections = new DoorDirections(north, south, east, west);

            if (newDirections.NumberOfDoors < 2 || newDirections.NumberOfDoors > 3) {
                throw new Exception("Incorrect number of doors (" + newDirections.NumberOfDoors + ") returned by DoorDirections.RandomDoors()! ");
            }

            return newDirections;
        }

        public static int randomDirection() {
            return Utilities.randomInt() % ((1 << 4) - 2) + 1;
        }

        public int toIndex() {
            if (NumberOfDoors > 1) {
                throw new Exception("Cannot convert multiple doors to an index.");
            }
            else if (NumberOfDoors == 0) {
                throw new Exception("Cannot convert no doors to an index.");
            }
            if (_north) return 0;
            else if (_south) return 1;
            else if (_east) return 2;
            else return 3;
        }

        public int NumberOfDoors {
            get { return (_north ? 1 : 0) + (_south ? 1 : 0) + (_east ? 1 : 0) + (_west ? 1 : 0); }
        }

        public bool hasNorth {
            get { return _north; }
            set { _north = value; }
        }
        public bool hasSouth {
            get { return _south; }
            set { _south = value; }
        }
        public bool hasEast {
            get { return _east; }
            set { _east = value; }
        }
        public bool hasWest {
            get { return _west; }
            set { _west = value; }
        }


        //This method accounts for the fact that only 3 tile images are used, and simply rotated to fit the correct orientation.
        internal float tileRotation() {
            switch (Type) {
                case ComboType.STRAIGHT:
                    if (!hasNorth) {
                        return (float)Math.PI / 2;
                    }
                    break;
                case ComboType.BENT:
                    if (hasEast && hasSouth) {
                        return (float)Math.PI / 2;
                    }
                    else if (hasSouth && hasWest) {
                        return (float)Math.PI;
                    }
                    else if (hasWest && hasNorth) {
                        return (float)(-Math.PI / 2);
                    }
                    break;
                case ComboType.TRIPLE:
                    if (!hasNorth) {
                        return (float)Math.PI / 2;
                    }
                    else if (!hasEast) {
                        return (float)Math.PI;
                    }
                    else if (!hasSouth) {
                        return (float)(-Math.PI / 2);
                    }
                    break;
                case ComboType.SINGLE:
                    if (hasNorth) return 0.0f;
                    if (hasSouth) return (float)Math.PI;
                    if (hasEast) return (float)Math.PI/2;
                    if (hasWest) return (float)(-Math.PI/2);
                    break;
            }
            return 0.0f;
        }
    }
}
