using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D9;
using System.Drawing;
using gameEditor2Lib;
using gameEditor2Lib.util;
using gameEditor2.util;

namespace gameEditor2.model
{
    class MapNode : MqoModel
    {
        private Logger logger = Logger.getLogger();

        private gameEditor2.collision.BoundingSphere sphere = new gameEditor2.collision.BoundingSphere();

        private Effect effect;

        private Device device;


        public override void onInit(Device device)
        {
            base.onInit(device);
            effect = effectRepository.getEffect(".\\resources\\fx\\selectedObj.fx");
            this.device = device;

            //とりあえず
            material = new Material { 
                Diffuse = Color.White,
                Ambient = Color.Gray,
            };
        }

        public override void draw()
        {
            sphere.setPosition(Vector3.Zero, 0.125f);
//            float rad = (float)((3) / 180 * Math.PI);
//            float rad = (float)((3f * logger.getFpsWeight()) / 180f * Math.PI);
            float rad = (float)((3f * logger.getFpsWeight()) / 180f * Math.PI);
            rotate = rotate * Matrix.RotationY(rad);
            Matrix world = rotate * trans;

            Matrix m = world * device.GetTransform(TransformState.View) * device.GetTransform(TransformState.Projection);

            effect.SetValue("WorldViewProj", m);
            effect.SetValue("DiffuseColor", material.Diffuse.ToArgb());
            effect.SetValue("AmbiColor", material.Ambient.ToArgb());
            effect.SetValue("LightDir", Vector3.TransformNormal(device.GetLight(0).Direction, Matrix.Invert(world)));
            effect.BeginPass(0);

            vm.drawSimple();

            effect.EndPass();

            //            sphere.debugDraw(world);
        }

        public Boolean intercects(Vector3 pos, Vector3 dir, out float distance) {
            Boolean b = sphere.intersects(pos, dir, rotate * trans, out distance);
            return b;
        }

        public override void dispose()
        {
            base.dispose();
            sphere.dispose();
        }
    }
}
