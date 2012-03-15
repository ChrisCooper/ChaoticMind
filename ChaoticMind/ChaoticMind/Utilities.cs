using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoticMind {
    class Utilities {
        static Random rand = new Random();

        public static Microsoft.Xna.Framework.Vector2 randomVector() {
            return new Vector2((float)rand.NextDouble() - 0.5f, (float)rand.NextDouble() - 0.5f);
        }

        public static Microsoft.Xna.Framework.Vector2 randomNormalizedVector() {
            Vector2 result = randomVector();
            result.Normalize();
            return result;
        }

        public static double randomDouble() {
            return rand.NextDouble();
        }

        public static int randomInt() {
            return rand.Next();
        }
        public static int randomInt(int low, int high) {
            return rand.Next() % high + low;
        }
    }
}
