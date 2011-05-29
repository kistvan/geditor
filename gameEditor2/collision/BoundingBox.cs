using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D9;
using gameEditor2Lib;

namespace gameEditor2.collision
{
    class BoundingBox
    {
        private SlimDX.BoundingBox box;
        private Mesh mesh;

        public BoundingBox() {
            box = new SlimDX.BoundingBox();
        }

        public BoundingBox(Vector3 minimum, Vector3 maximum)
        {
            box = new SlimDX.BoundingBox(minimum, maximum);
        }

        public BoundingBox(Vector3[] points) {
            box = SlimDX.BoundingBox.FromPoints(points);
        }

        public void setPosition(Vector3 minimum, Vector3 maximum)
        {
            Vector3[] c = box.GetCorners();
            String s = "";
            //Console.WriteLine("---------------");
            //Console.WriteLine(c[0]);//上 右上
            //Console.WriteLine(c[1]);//終点 上 右下
            //Console.WriteLine(c[2]);//下 右下
            //Console.WriteLine(c[3]);//下 右上
            //Console.WriteLine(c[4]);//上 左上
            //Console.WriteLine(c[5]);//上 左下
            //Console.WriteLine(c[6]);//下 左下
            //Console.WriteLine(c[7]);//始点 下 左上
            if (mesh == null)
            {
                float width = 0.50f;
                float height = 0.05f;
                float depth = 0.05f;
//                mesh = Mesh.CreateBox(DeviceContext.getDevice(), width, height, depth);
                //mesh = new Mesh(DeviceContext.getDevice(), 12, 8, MeshFlags.VertexBufferManaged, VertexFormat.Position);
                //mesh.LockVertexBuffer(LockFlags.None).WriteRange(box.GetCorners());
                //mesh.UnlockVertexBuffer();
            }
        }

        public Boolean intersects(Vector3 positon, Vector3 dir, Matrix world, out float distance) {
            Vector3 min = Vector3.TransformCoordinate(box.Minimum, world);
            Vector3 max = Vector3.TransformCoordinate(box.Maximum, world);
            Boolean b = SlimDX.BoundingBox.Intersects(new SlimDX.BoundingBox(min, max), new Ray(positon, dir), out distance);
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
