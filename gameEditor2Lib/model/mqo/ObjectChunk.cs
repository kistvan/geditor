using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using System.Drawing;

namespace gameEditor2Lib.model.mqo
{
    class ObjectChunk
    {
        private int faceNum;
        public int FaceNum {
            set { faceNum = value; }
            get { return faceNum; }
        }
        private int vertexNum;
        public int VertexNum {
            set { vertexNum = value; }
            get { return vertexNum; }
        }

        private List<Vector3> verteices = new List<Vector3>();
        public List<Vector3> Verteices {
            get { return verteices; }
        }
        private List<short> indeies = new List<short>();
        public List<short> Indeies
        {
            get { return indeies; }
        }
        private List<Vector3> normals = new List<Vector3>();
        public List<Vector3> Normals
        {
            get { return normals; }
        }

        private List<Color> vertexColors = new List<Color>();
        public List<Color> VertexColors {
            get { return vertexColors; }
        }

        public void appendVertex(Vector3 v) {
            verteices.Add(v);
            //設定がないとき用
            vertexColors.Add(Color.White);
         }

        public void appendIndex(short i) {
            indeies.Add(i);
        }

        public void initNormals() {
            foreach (Vector3 v in verteices)
            {
                normals.Add(Vector3.Zero);
            }
        }

        public void addNormal(int index, Vector3 v) {
            normals[index] = normals[index] + v;
        }

        public void normarizeNormals() {
            for (int i = 0; i < normals.Count(); i++ )
            {
                normals[i] = Vector3.Normalize(normals[i]);
            }
        }

        public List<short> IndeiesAddVertex(int vNum) {
            List<short> list = new List<short>();
            foreach(short i in indeies) {
                list.Add((short)(i + (short)vNum));
            }
            return list;
        }

        public void setVertexColors(int index, Color color) {
            vertexColors[index] = color;
        }
    }
}
