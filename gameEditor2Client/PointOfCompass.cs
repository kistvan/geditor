using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace gameEditor2Client
{
    class PointOfCompass
    {
        public const int NORTH = 0;
        public const int EAST = 1;
        public const int SOUTH = 2;
        public const int WEST = 3;

        private int currnentDir = 0;

        public int turnRight() {
            currnentDir++;
            if(currnentDir == 0 || currnentDir == 4) {
                this.currnentDir = 0;
            }
            return currnentDir;
        }

        public int turnLeft() {
            currnentDir--;
            if(currnentDir == -1) {
                this.currnentDir = 3;
            }
            return currnentDir;
        }

        public int getDirection() {
            return currnentDir;
        }

        public int getPrevDirection() {
            int n = currnentDir - 1;
            if(n == -1) {
                return WEST;
            }
            return n;
        }

        public Vector3 getDir() { 
            if(currnentDir == NORTH) {
                return new Vector3(0,0,1);
            } else if(currnentDir == EAST) {
                return new Vector3(1,0,0);
            } else if(currnentDir == SOUTH) {
                return new Vector3(0, 0, -1);
            } else if(currnentDir == WEST) {
                return new Vector3(-1,0,0);
            }
            throw new System.ArgumentException("現在の方向値が未定義です。 dir:" + currnentDir);
        }

        public Vector3 getDir(int dir) {
            if(dir == NORTH) {
                return new Vector3(0,0,1);
            }
            else if (dir == EAST)
            {
                return new Vector3(1,0,0);
            }
            else if (dir == SOUTH)
            {
                return new Vector3(0, 0, -1);
            }
            else if (dir == WEST)
            {
                return new Vector3(-1,0,0);
            }
            throw new System.ArgumentException("現在の方向値が未定義です。 dir:" + currnentDir);
        }




    }
}
