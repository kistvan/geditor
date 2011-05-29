using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using gameEditor2Client.screenEffect;
using SlimDX.Direct3D9;

namespace gameEditor2Client
{
    class ScreenEffects
    {
        private static ScreenEffects instance;

        private List<ScreenEffect> effects = new List<ScreenEffect>();

        private ScreenEffects() { 
        }

        public static ScreenEffects getInstance() {
            if(instance == null) {
                instance = new ScreenEffects();
            }
            return instance;
        }

        public void addEffect(ScreenEffect effect) {
            effects.Add(effect);
        }

        public void draw(Effect effect) {
            List<ScreenEffect> l = new List<ScreenEffect>();
            foreach(ScreenEffect ef in effects) {
                ef.anim(effect);
                if(!ef.finished()) {
                    l.Add(ef);
                }
            }
            effects.Clear();
            effects = l;
        }

        public Boolean hasEffect() {
            return effects.Count() != 0;
        }
    }
}
