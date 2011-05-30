using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SlimDX.Direct3D9;
using gameEditor2Lib;

namespace gameEditor2Client.screenEffect
{
    class Flash : ScreenEffect
    {
        private Color color = Color.White;

        private int alpha = 0;

        private TopPositionVertex vertex;

        public Flash(Device device) {
            vertex = new TopPositionVertex(device, new TopPosition(0,0,device.Viewport.Width, device.Viewport.Height));

            
        }

        #region ScreenEffect メンバ

        void ScreenEffect.anim(Effect effect)
        {
            alpha++;
            effect.Technique = "technique0";
            effect.Begin();
            effect.SetValue("alpha", alpha / 255f);
            effect.BeginPass(0);
            vertex.draw();
            effect.EndPass();
            effect.End();
        }

        bool ScreenEffect.finished()
        {
            bool b = alpha >= 255;
            if(b) {
                vertex.dispose();
            }
            return b;
        }

        #endregion
    }
}
