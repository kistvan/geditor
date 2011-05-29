using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using gameEditor2Client.screenEffect;
using gameEditor2Lib;

namespace gameEditor2Client
{
    class MoveEvent
    {
        public int encountRatio = 0;
        public MoveEvent() {
        }

        public void onMoveEvent(int x, int y) {
            Random rand = new Random();
            int val = rand.Next(100);

            if(val <= encountRatio) {
                Flash flash = new Flash(DeviceContext.getDevice());
                ScreenEffects.getInstance().addEffect(flash);
            }
        }
    }
}
