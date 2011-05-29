using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SlimDX;
using SlimDX.Direct3D9;
using gameEditor2Lib.model.mqo;
using gameEditor2Lib.util;
using System.Drawing;

namespace gameEditor2Lib
{
    public class MqoModel
    {
        private List<ObjectChunk> objects = new List<ObjectChunk>();
        private List<MaterialChunk> materials = new List<MaterialChunk>();
        private Boolean isObjects = false;
        private Boolean isMaterials = false;
        private Boolean isVerteies = false;
        private Boolean isFaces = false;

        private int vertexNum = 0;
        private int faces = 0;

        public int VertexNum
        {
            get { return vertexNum; }
        }
        public int Faces {
            get { return faces; }
        }

        private List<Vector3> verteies = new List<Vector3>();

        public List<Vector3> Verteies
        {
            get { return verteies; }
        }

        private List<short> indeies = new List<short>();
        public List<short> Indeies {
            get { return indeies; }
        }

        protected VertexModel vm;
        public VertexModel VertexModel {
            set { this.vm = value; }
            get { return vm; }
        }

        protected Matrix rotate = Matrix.Identity;
        protected Matrix trans = Matrix.Identity;

        private List<Vector3> vertexNormals = new List<Vector3>();
        public List<Vector3> VertexNormals {
            get { return vertexNormals; }
        }

        protected Material material;
        public Material Material {
            set { this.material = value; }
            get { return material; }
        }

        protected Boolean enabled = true;
        public Boolean Enabled {
            set { enabled = value; }
            get { return enabled; }
        }

        protected List<Color> vertexColors = new List<Color>();
        public List<Color> VertexColors {
            get { return vertexColors; }
        }

        protected EffectRepository effectRepository = EffectRepository.getInstance();

        public virtual void onInit(Device device) { }

        public void appendLine(String line) {

            if (!isObjects && line.Trim().StartsWith("Object"))
            {
                isObjects = true;
                objects.Add(new ObjectChunk());
                return;
            } else if(isObjects && line.Trim().StartsWith("vertex")) {
                isVerteies = true;
                String n = line.Replace("vertex", "").Replace("{", "");
                objects.Last().VertexNum = Convert.ToInt32(n);
                return;
            }
            else if (isObjects && !isVerteies && line.Trim().StartsWith("face") && line.Contains('{'))
            {
                isFaces = true;
                objects.Last().FaceNum = Convert.ToInt32(line.Replace("face", "").Replace("{", "").Trim()); ;
                return;
            //material
            }
            else if (!isObjects && line.Trim().StartsWith("Material"))
            {
                isMaterials = true;
                return;
            }

            if (isVerteies)
            {
                //vertexエリアの終了時
                if (line.Trim().Equals("}"))
                {
                    //頂点の数だけ法線を初期化
                    isVerteies = false;
                    objects.Last().initNormals();
                    return;
                }
                String[] vArray = line.Trim().Split(new char[] { ' ' });
                Vector3 nv = new Vector3(float.Parse(vArray[0]) / 100.0f, float.Parse(vArray[1]) / 100.0f, -1 * float.Parse(vArray[2]) / 100.0f);
                objects.Last().appendVertex(nv);
            } else if(isFaces) {
                //faceの終わり
                if(line.Trim().Equals("}")) {
                    isFaces = false;
                    //足した法線を規格化
                    objects.Last().normarizeNormals();
                    return;
                }
                int start = line.IndexOf("V(") + 2;
                //indexを追加
                String v = line.Substring(start, line.IndexOf(')') - start).Trim();
                String[] ar = v.Split(new char[] { ' ' });
                short i0 = short.Parse(ar[0]);
                short i1 = short.Parse(ar[1]);
                short i2 = short.Parse(ar[2]);
                objects.Last().appendIndex(i0);
                objects.Last().appendIndex(i1);
                objects.Last().appendIndex(i2);
                
                //面法線を求める
                Vector3 nv = Vector3.Cross((objects.Last().Verteices[i1] - objects.Last().Verteices[i0]), (objects.Last().Verteices[i2] - objects.Last().Verteices[i0]));
                nv.Normalize();

                //面法線を各頂点へ足す
                objects.Last().addNormal(i0, nv);
                objects.Last().addNormal(i1, nv);
                objects.Last().addNormal(i2, nv);

                //マテリアルが設定されている場合はセット
                if(line.IndexOf(" M(") != -1) {
                    String tmpL = line.Substring(line.IndexOf(" M(")+3);
                    String mate = tmpL.Substring(0, tmpL.IndexOf(')'));
                    objects.Last().setVertexColors(i0, materials[int.Parse(mate)].Color);
                    objects.Last().setVertexColors(i1, materials[int.Parse(mate)].Color);
                    objects.Last().setVertexColors(i2, materials[int.Parse(mate)].Color);
                }


                //objectsの終わり
            } else if (isObjects && line.Trim().Equals("}")) {
                isObjects = false;
            } else if(isMaterials) {
                //materialの終わり
                if(line.Trim().Equals("}")) {
                    isMaterials = false;
                    return;
                }
                //material
                materials.Add(new MaterialChunk(line));
            }

            //ファイルの終わり
            if(line.Trim().Equals("Eof")) {
                //頂点数などを合計しとく
                int vNum, vFace ;
                vNum = vFace = 0;
                List<Vector3> sV = new List<Vector3>();
                List<Vector3> sNormal = new List<Vector3>();
                List<short> sIndex = new List<short>();
                verteies.Clear();
                indeies.Clear();
                vertexNormals.Clear();
                foreach (ObjectChunk oc in objects)
                {
                    //頂点、インデックス、頂点法線を合計してセット
                    verteies.AddRange(oc.Verteices);
                    indeies.AddRange(oc.IndeiesAddVertex(vNum));
                    vertexNormals.AddRange(oc.Normals);
                    //頂点カラーも
                    vertexColors.AddRange(oc.VertexColors);

                    vNum += oc.VertexNum;
                    vFace += oc.FaceNum;

                }
                vertexNum = vNum;
                faces = vFace;

            }


        }

        public void translation(float x, float y, float z) {
            trans = Matrix.Translation(x,y,z);
        }

        public void rotateY(float radian) {
            rotate = rotate * Matrix.RotationY(radian);
        }

        public virtual void draw() {
            vm.draw(rotate * trans);
        }

        public virtual void drawSimple() {
            vm.drawSimple();
        }

        public void setDiffuse(Color color) {
            material.Diffuse = color;
        }

        public virtual void dispose() {
            vm.dispose();
        }
    }
}
