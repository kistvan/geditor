using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D9;
using System.Drawing;
using System.Runtime.InteropServices;

namespace gameEditor2Client.screenEffect
{
    class TopPositionVertex
    {
        struct CUSTOM_VX {
            public Vector4 Position;
            public int Color;
        };

        private Device device;

        private VertexBuffer vertices;
        private VertexDeclaration vertexDecl;

        private TopPosition tPosition;

        private int vNum;
        private int faceNum;

        public TopPositionVertex(Device device, TopPosition tPosition)
        {
            this.device = device;
            this.tPosition = tPosition;
            vNum = 4;
            faceNum = 2;
            vertices = new VertexBuffer(device, vNum * Marshal.SizeOf(typeof(CUSTOM_VX)), Usage.WriteOnly, VertexFormat.None, Pool.Managed);
            CUSTOM_VX[] vs = new CUSTOM_VX[vNum];

            Color4 white = new Color4(Color.White);
            white.Alpha = 1.0f;
            float w = device.Viewport.Width;
            float h = device.Viewport.Height;
            vs[0] = new CUSTOM_VX()
            {
                Position = new Vector4(tPosition.X, tPosition.HeightPos, 0f, 1.0f),
                Color = white.ToArgb(),
            };
            vs[1] = new CUSTOM_VX()
            {
                Position = new Vector4(tPosition.X, tPosition.Y, 0f, 1.0f),
                Color = white.ToArgb(),
            };
            vs[2] = new CUSTOM_VX()
            {
                Position = new Vector4(tPosition.WidthPos, tPosition.HeightPos, 0f, 1.0f),
                Color = white.ToArgb(),
            };
            vs[3] = new CUSTOM_VX()
            {
                Position = new Vector4(tPosition.WidthPos, 0f, 0f, 1.0f),
                Color = white.ToArgb(),
            };
            
            vertices.Lock(0, 0, LockFlags.None).WriteRange(vs);
            vertices.Unlock();
            var vertexElems = new[] {
                new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.PositionTransformed, 0),
                new VertexElement(0, (short)((short)Marshal.SizeOf(typeof(Vector4))), DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd
            };
            vertexDecl = new VertexDeclaration(device, vertexElems);
        }

        public void draw() {
            device.SetStreamSource(0, vertices, 0, Marshal.SizeOf(typeof(CUSTOM_VX)));
            device.VertexDeclaration = vertexDecl;
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, faceNum);
        }

        public void dispose() {
            vertexDecl.Dispose();
            vertices.Dispose();
        }
    }
}
