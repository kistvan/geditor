using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using gameEditor2Lib;
using SlimDX.Direct3D9;
using SlimDX;

namespace gameEditor2Client.model
{
    class Floor
    {
        private MqoModel model;

        private Effect effect;

        private Device device;

        public List<List<Boolean>> edgeXEnable;
        public List<List<Boolean>> edgeYEnable;

        public Floor(String path, Device device) {
            this.device = device;
            model = new MqoParser(device).parse(path, new MqoModel());
            effect = EffectRepository.getInstance().getEffect(@"resources\fx\selectedObj.fx");
        }

        public void draw() {
            Matrix world = device.GetTransform(TransformState.World);
            Matrix m = device.GetTransform(TransformState.View) * device.GetTransform(TransformState.Projection);
            effect.Technique = "technique1";
            effect.Begin();

            effect.SetValue("WorldViewProj", m);
            //effect.SetValue("DiffuseColor", Color.White.ToArgb());
            //effect.SetValue("AmbiColor", Color.Gray.ToArgb());
            effect.SetValue("LightDir", Vector3.TransformNormal(device.GetLight(0).Direction, Matrix.Invert(world)));
            effect.BeginPass(0);

            model.drawSimple();

            effect.EndPass();
            effect.End();

        }

    }
}
