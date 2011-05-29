using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gameEditor2.util
{
    class Logger
    {
        private static Logger l;

        private LoggerView view;
        public LoggerView View {
            set{view = value;}
            get {return view;}
        }

        private int preTime = 0;

        private int baseTime = 0;

        private int fpsCount = 0;

        private int fps = 60;

        private float diffTime = 16.67f;

        int dtCount = 1;

        public static Logger getLogger() {
            if(l == null) {
                l = new Logger();
            }

            return l;
        }

        public void calcFps(int from) {
            if(baseTime == 0) {
                baseTime = from;
                return;
            }
            int dt = from - preTime;
            if (dt == 0)
            {
                dtCount++;
            }
            else {
                diffTime = (float)dt / (float)dtCount;
                dtCount = 1;
            }
            preTime = from;
            if(from - baseTime >= 1000) {
                fps = fpsCount;
                view.setFpsText(fpsCount.ToString());
                fpsCount = 0;
                baseTime = from;

                return;
            }
            fpsCount++;
        }

        public float getFpsWeight() {
            return diffTime / 16.667f;
        }

    }
}
