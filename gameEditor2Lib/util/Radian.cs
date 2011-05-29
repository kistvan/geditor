using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gameEditor2Lib.util
{
    public static class Radian
    {
        public static float ninty = Radian.getRadian(90);

        public static float oneEighty = (float)Math.PI;

        public static float getRadian(int degree) {
            return (float)((double)degree / 180.0 * Math.PI);
        }

        public static float getRadian(float degree) {
            return (float)((double)degree / 180.0 * Math.PI);
        }
    }
}
