using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gameEditor2Client
{
    class TopPosition
    {
        private float x;
        private float y;
        private float width;
        private float height;

        public TopPosition(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public float X
        {
            get { return x;}
            set {x = value;}
        }
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        public float Width
        {
            get { return width; }
            set { width = value; }
        }
        public float Height
        {
            get { return height; }
            set { height = value; }
        }
        public float WidthPos {
            get { return width + x; }
        }
        public float HeightPos {
            get { return height + y; }
        }

    }
}
