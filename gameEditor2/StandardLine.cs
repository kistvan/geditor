using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D9;
using System.Runtime.InteropServices;
using System.Drawing;
using SlimDX;

namespace gameEditor2
{
    class StandardLine
    {
        private Device device;

        private VertexBuffer vBuf;
        private VertexDeclaration vertexDecl;
        private Material material;
        private int primitiveCount;

        struct Vertex
        {
            public Vector3 Position;
            public int Color;
        }

        public StandardLine(Device device)
        {
            this.device = device;


            List<Vertex> list = new List<Vertex>();
            list.Add(new Vertex(){ Color = Color.Green.ToArgb(), Position = new Vector3(0f, 0f, 0f)});
            list.Add(new Vertex(){ Color = Color.Green.ToArgb(), Position = new Vector3(0f, 10.0f, 0f)});
            list.Add(new Vertex(){ Color = Color.Red.ToArgb(), Position = new Vector3(0f, 0.0f, 0f)});
            list.Add(new Vertex(){ Color = Color.Red.ToArgb(), Position = new Vector3(10.0f, 0.0f, 0f)});
            list.Add(new Vertex(){ Color = Color.Blue.ToArgb(), Position = new Vector3(0f, 0.0f, 0f)});
            list.Add(new Vertex(){ Color = Color.Blue.ToArgb(), Position = new Vector3(0.0f, 0.0f, 10.0f)});


            float f = 0.0f;
            list.Add(new Vertex() { Color = Color.Gray.ToArgb(), Position = new Vector3(-0.5f, 0.0f, 0f) });
            list.Add(new Vertex() { Color = Color.Gray.ToArgb(), Position = new Vector3(-0.5f, 0.0f, 10.0f) });
            for (int i = 0; i < 20; i++)
            {
                float z;
                if(i % 2 == 0) {
                    z = 10.0f;
                    f = f + 1.0f;
                }
                else
                {
                    z = 0f;
                }
                list.Add(new Vertex() { Color = Color.Gray.ToArgb(), Position = new Vector3(f - 0.5f, 0.0f, z)});
            }
            f = 0.0f;
            list.Add(new Vertex() { Color = Color.Gray.ToArgb(), Position = new Vector3(-0.5f, 0.0f, 0.0f) });
            list.Add(new Vertex() { Color = Color.Gray.ToArgb(), Position = new Vector3(0.0f, 0.0f, 0.0f) });
            for (int i = 0; i < 20; i++)
            {
                float x;
                if (i % 2 == 0)
                {
                    x = 10.0f;
                    f = f + 1.0f;
                }
                else
                {
                    x = -0.5f;
                }
                list.Add(new Vertex() { Color = Color.Gray.ToArgb(), Position = new Vector3(x, 0.0f, f - 0.5f) });
            }

            primitiveCount = list.Count() / 2;

            vBuf = new VertexBuffer(device, list.Count() * Marshal.SizeOf(typeof(Vertex)), Usage.WriteOnly, VertexFormat.None, Pool.Managed);
            vBuf.Lock(0, 0, LockFlags.None).WriteRange(list.ToArray()
//                new Vertex[]{
                //マスのガイド
                //new Vertex(){ Color = Color.Gray.ToArgb(), Position = new Vector3(0.5f, 0.0f, 0.0f)},
                //new Vertex(){ Color = Color.Gray.ToArgb(), Position = new Vector3(0.5f, 0.0f, 10.0f)},
                //new Vertex(){ Color = Color.Gray.ToArgb(), Position = new Vector3(1.0f, 0.0f, 0.0f)},
                //new Vertex(){ Color = Color.Gray.ToArgb(), Position = new Vector3(1.0f, 0.0f, 10.0f)},

                //new Vertex(){ Color = Color.Gray.ToArgb(), Position = new Vector3(0.0f, 0.0f, 0.5f)},
                //new Vertex(){ Color = Color.Gray.ToArgb(), Position = new Vector3(10.0f, 0.0f, 0.5f)},
                //new Vertex(){ Color = Color.Gray.ToArgb(), Position = new Vector3(0.0f, 0.0f, 1.0f)},
                //new Vertex(){ Color = Color.Gray.ToArgb(), Position = new Vector3(10.0f, 0.0f, 1.0f)},
//            }
        );
            vBuf.Unlock();
            var vertexElems = new[] {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, (short)Marshal.SizeOf(typeof(Vector3)), DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd
            };

            vertexDecl = new VertexDeclaration(device, vertexElems);
            material = new Material()
            {
                Diffuse = Color.Blue,
                Ambient = Color.GhostWhite,
            };
        }

        public void draw() {
            int lighting = device.GetRenderState(RenderState.Lighting);
            if(lighting == 1) {
                device.SetRenderState(RenderState.Lighting, false);
            }
            device.SetStreamSource(0, vBuf, 0, Marshal.SizeOf(typeof(Vertex)));
            device.VertexDeclaration = vertexDecl;
//            device.Material = null;
            device.DrawPrimitives(PrimitiveType.LineList, 0, primitiveCount);

            if(lighting == 1) {
                //元に戻す
                device.SetRenderState(RenderState.Lighting, true);
            }
        }
    }
}
