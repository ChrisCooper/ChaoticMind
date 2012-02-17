using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoticMind
{
    public enum DoorDirection
    {
        NORTH = 1 << 0,
        SOUTH = 1 << 1,
        EAST = 1 << 2,
        WEST = 1 << 3
    };

    //Body type should be Kinematic
    class MapTile //:GameObject
    {
        public MapTile(DoorDirection openDoors)
        {

        }
    }
}
