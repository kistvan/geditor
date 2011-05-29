using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gameEditor2
{
    class MouseDelta
    {
        private int prevX;
        private int prevY;
        private int x;
        private int y;

        public void calc(int x, int y) {
            if (prevX == 0 && this.x == 0 && prevY == 0 && this.y == 0)
            {
                prevX = x;
                prevY = y;
            }
            else
            {
                prevX = this.x;
                prevY = this.y;
            }
            this.x = x;
            this.y = y;
        }

        public int getDeltaX() {
            return prevX - x;
        }

        public int getDeltaY() {
            return prevY - y;
        }

        public void clear() {
            prevX = prevY = x = y = 0;
        }

    }
}
