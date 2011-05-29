using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SlimDX.Direct3D9;

namespace gameEditor2Lib
{
    public class EffectRepository
    {
        private static EffectRepository repo;

        private Dictionary<String, Effect> map = new Dictionary<String, Effect>();

        private EffectRepository() { }

        public static EffectRepository getInstance() {
            if(repo == null) {
                repo = new EffectRepository();
            }
            return repo;
        }

        public Effect getEffect(String filePath) { 
            if(map.ContainsKey(filePath)) {
                return map[filePath];
            }
            Effect e = Effect.FromFile(DeviceContext.getDevice(), filePath, ShaderFlags.SkipValidation);
            map[filePath] = e;
            return e;
        }
    }
}
