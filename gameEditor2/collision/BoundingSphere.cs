using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D9;
using gameEditor2Lib;

namespace gameEditor2.collision
{
    class BoundingSphere
    {
        private SlimDX.BoundingSphere sphere;

        private Mesh mesh;

        public BoundingSphere() {
            sphere = new SlimDX.BoundingSphere();
        }

        public BoundingSphere(Vector3 center, float r) {
            sphere = new SlimDX.BoundingSphere(center, r);
        }

        public void setPosition(Vector3 center, float r) {
            sphere.Center = center;
            sphere.Radius = r;
            if(mesh == null) {
                mesh = Mesh.CreateSphere(DeviceContext.getDevice(), r, 6, 6);
            }
        }

        public Boolean intersects(Vector3 positon, Vector3 dir, Matrix world, out float distance) {
            sphere.Center = Vector3.TransformCoordinate(Vector3.Zero, world);
            Boolean b = SlimDX.BoundingSphere.Intersects(sphere, new Ray(positon, dir), out distance);
            sphere.Center = Vector3.Zero;
            return b;
        }

        public void debugDraw(Matrix world) {
            Device device = DeviceContext.getDevice();
            device.SetTransform(TransformState.World, world);
            int mode = device.GetRenderState(RenderState.FillMode);
            Boolean changed = false;
            if(mode != (int)FillMode.Wireframe) {
                changed = true;
                device.SetRenderState(RenderState.FillMode, FillMode.Wireframe);
            }
            mesh.DrawSubset(0);
            if(changed) {
                device.SetRenderState(RenderState.FillMode, mode);
            }
            //元に戻しておく
            device.SetTransform(TransformState.World, Matrix.Identity);
        }

        public void dispose() { 
            if(mesh != null) {
                mesh.Dispose();
            }
        }
    }
}
