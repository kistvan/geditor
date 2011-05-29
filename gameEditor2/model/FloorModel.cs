using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D9;
using SlimDX;
using System.Drawing;
using gameEditor2Lib;

namespace gameEditor2.model
{
    class FloorModel : MqoModel
    {
        private Effect effect;
        private MqoModel model;

        public FloorModel(MqoModel model) {
            this.model = model;
            effect = EffectRepository.getInstance().getEffect(".\\resources\\fx\\selectedObj.fx");
        }

        public override void draw()
        {
            Device device = DeviceContext.getDevice();
            Matrix world = device.GetTransform(TransformState.World);
            Matrix m =  device.GetTransform(TransformState.View) * device.GetTransform(TransformState.Projection);
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
