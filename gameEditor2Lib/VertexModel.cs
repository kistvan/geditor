using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D9;
using SlimDX;
using System.Drawing;
using System.Runtime.InteropServices;

namespace gameEditor2Lib
{
    public class VertexModel
    {

        struct Vertex
        {
            public Vector3 Position;
            public Vector3 Normal;
            public int Color;
        }

        private VertexBuffer vertices;
        private VertexDeclaration vertexDecl;

        private IndexBuffer index;

        private Device device;
        private MqoModel model;
        private Material material;

        private Mesh mesh;
        public VertexModel(MqoModel model, Device device) {

            this.device = device;
            this.model = model;

            vertices = new VertexBuffer(device, model.VertexNum * Marshal.SizeOf(typeof(Vertex)), Usage.WriteOnly, VertexFormat.None, Pool.Managed);
//            Console.WriteLine(model.VertexNum);
            Vertex[] vs = new Vertex[model.VertexNum];
            for (int i = 0; i < model.VertexNum; i++)
            {
//                Console.WriteLine(model.VertexColors[i].ToArgb());
                vs[i] = new Vertex() { 
                    Color = model.VertexColors[i].ToArgb(), 
//                    Color = Color.Gray.ToArgb(),
                    Position = new Vector3(model.Verteies[i].X, model.Verteies[i].Y, model.Verteies[i].Z),
                    Normal = new Vector3(model.VertexNormals[i].X, model.VertexNormals[i].Y, model.VertexNormals[i].Z),
                };
            }


            vertices.Lock(0, 0, LockFlags.None).WriteRange(
                vs
        );
            vertices.Unlock();

            var vertexElems = new[] {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, (short)Marshal.SizeOf(typeof(Vector3)), DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
                new VertexElement(0, (short)((short)Marshal.SizeOf(typeof(Vector3)) * 2), DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd
            };

            vertexDecl = new VertexDeclaration(device, vertexElems);

            index = new IndexBuffer(device, 3 * model.Faces * sizeof(short), Usage.WriteOnly, Pool.Managed, true);

            index.Lock(0, 0, LockFlags.None).WriteRange(model.Indeies.ToArray()
            );
            index.Unlock();
            material = new Material()
            {
                Diffuse = Color.White,
                Ambient = Color.FromArgb(255,255,255),
            };

            //mesh = new Mesh(device, model.Faces, model.VertexNum, MeshFlags.WriteOnly, vertexElems);
            //mesh.LockVertexBuffer(LockFlags.None).WriteRange(vs);
            //mesh.UnlockVertexBuffer();
            //mesh.LockIndexBuffer(LockFlags.None).WriteRange(model.Indeies.ToArray());
            //mesh.UnlockIndexBuffer();

        }

        public void draw(Matrix worldM) {
            device.SetTransform(TransformState.World, worldM);
            device.SetStreamSource(0, vertices, 0, Marshal.SizeOf(typeof(Vertex)));
            device.Indices = index;
            device.VertexDeclaration = vertexDecl;
            device.Material = material;
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, model.VertexNum, 0, model.Faces);
//            mesh.DrawSubset(0);

            //元に戻しておく
//            device.SetTransform(TransformState.World, Matrix.Identity);
        }

        public void drawSimple() {
            device.SetStreamSource(0, vertices, 0, Marshal.SizeOf(typeof(Vertex)));
            device.Indices = index;
            device.VertexDeclaration = vertexDecl;
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, model.VertexNum, 0, model.Faces);
//            device.SetTransform(TransformState.World, Matrix.Identity);
        }

        public void dispose() {
            vertexDecl.Dispose();
            index.Dispose();
            vertices.Dispose();
            model = null;
//            mesh.Dispose();
        }
    }
}
