using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using gameEditor2Client.model;

namespace gameEditor2Client
{
    class CameraPosition
    {
        public MoveEvent moveEvent = new MoveEvent();
        private int MaxX = 0;
        private int MaxY = 0;
        private int x = 0;
        private int y = 0;
        public int X {get{return x;}}
        public int Y {get{return y;}}
        

        private Camera camera;

        private Floor floor;
        public Floor Floor{
            set { this.floor = value; }
        }

        public CameraPosition(Camera camera) {
            this.camera = camera;
        }

        public void moveForward() {
            if (PointOfCompass.NORTH == camera.Poc.getDirection())
            {
                if(MaxY - 1 == y) {
                    return;
                }
                if(moveCheck()) {
                    y++;
                    camera.moveForward();
                }
            }
            else if (PointOfCompass.EAST == camera.Poc.getDirection())
            {
                if(MaxX - 1 == x) {
                    return;
                }
                if(moveCheck()) {
                    camera.moveForward();
                    x++;
                }
            }
            else if (PointOfCompass.SOUTH == camera.Poc.getDirection())
            {
                if (y == 0)
                {
                    return;
                }
                if (moveCheck())
                {
                    camera.moveForward();
                    y--;
                }
            }
            else {
                if(x == 0) {
                    return; 
                }
                if(moveCheck()) {
                    camera.moveForward();
                    x--;
                }
            }
            moveEvent.onMoveEvent(x, y);
        }

        public void moveBack() {
            if (PointOfCompass.NORTH == camera.Poc.getDirection())
            {
                if (0 == y)
                {
                    return;
                }
                y--;
            }
            else if (PointOfCompass.EAST == camera.Poc.getDirection())
            {
                if (0 == x)
                {
                    return;
                }
                x--;
            }
            else if (PointOfCompass.SOUTH == camera.Poc.getDirection())
            {
                if (MaxY - 1 == y)
                {
                    return;
                }
                x++;
            }
            else
            {
                if (MaxX - 1 == x)
                {
                    return;
                }
                x++;
            }
            camera.moveBack();
        }

        public void turnRight() {
            camera.turnRight();
            moveEvent.onMoveEvent(x,y);
        }

        public void turnLeft() {
            camera.turnLeft();
            moveEvent.onMoveEvent(x, y);
        }

        public void setSize(int x, int y) {
            this.MaxX = x;
            this.MaxY = y;
        }


        private Boolean moveCheck() {
            if (PointOfCompass.NORTH == camera.Poc.getDirection())
            {
                //y軸の話
                if(!floor.edgeYEnable.ElementAt(x).ElementAt(y)) {
                    return false;
                }
            }
            else if (PointOfCompass.SOUTH == camera.Poc.getDirection()) {
                //y軸の話
                if (!floor.edgeYEnable.ElementAt(x).ElementAt(y-1))
                {
                    return false;
                }
            }
            else if (PointOfCompass.EAST == camera.Poc.getDirection())
            {
                //x軸の話
                if (!floor.edgeXEnable.ElementAt(x).ElementAt(y))
                {
                    return false;
                }
            }
            else {
                //x軸の話
                if (!floor.edgeXEnable.ElementAt(x-1).ElementAt(y))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
