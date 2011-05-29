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
    class MapEdge : MqoModel
    {
        private collision.BoundingBox box;
        private Effect effect;

        private Device device;

        private static int i = 0;


        public override void onInit(Device device)
        {
            base.onInit(device);
            effect = effectRepository.getEffect(".\\resources\\fx\\selectedObj.fx");
            this.device = device;
            box = new gameEditor2.collision.BoundingBox(Verteies.ToArray());
            //とりあえず
            material = new Material
            {
                Diffuse = Color.White,
                Ambient = Color.Gray,
            };
        }

        public override void draw()
        {
            box.setPosition(Vector3.Zero, new Vector3(1.0f,1.0f,1.0f));

            Matrix world = rotate * trans;

            Matrix m = world * device.GetTransform(TransformState.View) * device.GetTransform(TransformState.Projection);

            effect.SetValue("WorldViewProj", m);
            if (enabled)
            {
                effect.SetValue("DiffuseColor", material.Diffuse.ToArgb());
                effect.SetValue("AmbiColor", material.Ambient.ToArgb());
            }
            else {
                effect.SetValue("DiffuseColor", Color.Black.ToArgb());
                effect.SetValue("AmbiColor", Color.FromArgb(30, 30, 30).ToArgb());
            }
            effect.SetValue("LightDir", Vector3.TransformNormal(device.GetLight(0).Direction, Matrix.Invert(world)));
            effect.BeginPass(0);

            vm.drawSimple();

            effect.EndPass();
        }
        public Boolean intercects(Vector3 pos, Vector3 dir, out float distance)
        {
            Boolean b = box.intersects(pos, dir, rotate * trans, out distance);
            return b;
        }

        public override void dispose()
        {
            base.dispose();
        }

    }
}
