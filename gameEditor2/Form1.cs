using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SlimDX.Direct3D9;
using SlimDX;
using gameEditor2Lib.util;
using System.Runtime.InteropServices;
using gameEditor2.model;
using System.IO;
using gameEditor2Lib.io;
using gameEditor2Lib;
using gameEditor2.util;

namespace gameEditor2
{
    public partial class Form1 : Form
    {
        private gameEditor2.util.Logger logger = gameEditor2.util.Logger.getLogger();

        public Device device;

        private Mesh teapot;

        private StandardLine standardLine;

        private List<MqoModel> selections = new List<MqoModel>();

        private String projectName = "sample";


        //保存対象
        private FloorMap floorMap;

        private Camera camera;

        private MqoModel floorModel;

        private Boolean drawFloorModel = true;

        private Boolean showAlwaysFloorMap = false;

        public Form1() {
            InitializeComponent();
        }

        public void init(StreamWriter st)
        {
            st.WriteLine("初期化開始");

            pictureBox1.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);

            st.WriteLine("デバイス作成の開始");
            device = new Device(new Direct3D(), 0, DeviceType.Hardware, this.pictureBox1.Handle, CreateFlags.HardwareVertexProcessing, new PresentParameters()
            {
                BackBufferWidth = this.pictureBox1.ClientSize.Width,
                BackBufferHeight = this.pictureBox1.ClientSize.Height,
                Windowed = true,
            });
            st.WriteLine("デバイス作成の終了");
            Capabilities caps = device.Capabilities;
            st.WriteLine("adapterOrdinal:" + caps.AdapterOrdinal);
            st.WriteLine("DeviceType:" + caps.DeviceType);
            if (caps.VertexShaderVersion.Major < 3)
            {
                MessageBox.Show("VertexShader3.0 以上がサポートされていません。\n現バージョン:" + caps.VertexShaderVersion);
                device.Dispose();
                Environment.Exit(0);
            }
            else if (caps.PixelShaderVersion.Major < 3)
            {
                MessageBox.Show("PixelShader3.0 以上がサポートされていません。\n現バージョン:" + caps.VertexShaderVersion);
                device.Dispose();
                Environment.Exit(0);
            }

            st.WriteLine("DeviceContextの初期化");
            DeviceContext.init(device);

            st.WriteLine("ZEnableの初期化");
            device.SetRenderState(RenderState.ZEnable, true);

            st.WriteLine("cameraの初期化");
            camera = new Camera(device);

            st.WriteLine("Floorの初期化");
            floorMap = new FloorMap(device);

//            teapot = Mesh.CreateTeapot(device);
//            device.SetRenderState(RenderState.ShadeMode, ShadeMode.Flat);

            st.WriteLine("照明の初期化");
            device.SetRenderState(RenderState.Lighting, true);
            device.SetLight(0, new Light() { 
                Type = LightType.Directional,
                Diffuse = Color.White,
                Ambient = Color.Gray,
                Direction = new Vector3(1.0f, 1.0f, 0.0f),
            });
            device.EnableLight(0,true);
            standardLine = new StandardLine(device);
            //射影変換
            device.SetTransform(TransformState.Projection,
                                 Matrix.PerspectiveFovLH((float)(Math.PI / 4),
                                                         (float)this.pictureBox1.ClientSize.Width / (float)this.pictureBox1.ClientSize.Height,
                                                         0.1f, 1500.0f));
            //ビュー
            device.SetTransform(TransformState.View,
                                 camera.getLookAtLh());

            device.SetTransform(TransformState.World, Matrix.Identity);

            st.WriteLine("FloorModelの初期化");
            floorModel = new FloorModel(new MqoParser(device).parse(".\\projects\\sample\\model\\floor.mqo", new MqoModel()));

            st.WriteLine("初期化終了");
        }

        public void DrawFrame() {
            int from = Environment.TickCount;
            logger.calcFps(from);

            foreach (MqoModel model in selections)
            {
                //選択中に
                model.setDiffuse(Color.Yellow);
            }

            DateTime dt = DateTime.Now;

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            device.BeginScene();


            if(drawFloorModel) {
                floorModel.draw();
            }
            if(showAlwaysFloorMap) {
                device.Clear(ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            }
            floorMap.draw();

            //選択中の色を戻す
            foreach(MqoModel m in selections) {
                m.setDiffuse(Color.White);
            }

//            teapot.DrawSubset(0);
            standardLine.draw();

            device.EndScene();
            device.Present();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoggerView loggerView = new LoggerView();
//            this.AddOwnedForm(loggerView);
#if DEBUG
            loggerView.Show();
#endif
            logger.View = loggerView;

//            logger.debug("main form読み込み終了");
        }

        public void OnResourceUnload()
        {
            //vertices.Dispose();
            //vertexDecl.Dispose();

            //index.Dispose();

            //teapot.Dispose();

            //device.Dispose();
        }

        private void mapSizeY_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown o = (NumericUpDown)sender;
            floorMap.setY(o.Value);
        }

        private void mapSizeX_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown o = (NumericUpDown)sender;
            floorMap.setX(o.Value);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //右クリックドラッグでカメラの移動
            if (e.Button == MouseButtons.Right) {
                selections.Clear();
                camera.Rotate = true;
            } else if(e.Button == MouseButtons.Middle) {
                selections.Clear();
                camera.Trans = true;
            }
            
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            camera.zoomCamera(e.Delta);
            device.SetTransform(TransformState.View,
                                     camera.getLookAtLh());
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) {
                camera.Rotate = false;
            } else if(e.Button == MouseButtons.Middle) {
                camera.Trans = false;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Boolean changed = false;
            if(camera.Rotate) {
                changed = true;
                camera.rotateCamera(e.X, e.Y);
            }
            else if (camera.Trans)
            {
                changed = true;
                camera.transCamera(e.X, e.Y);
            }
            else {
                //スクリーン座標を取得
                Vector3 near = new Vector3(e.Location.X, e.Location.Y, device.Viewport.MinZ);
                Vector3 far = new Vector3(e.Location.X, e.Location.Y, device.Viewport.MaxZ);
                Matrix proj = device.GetTransform(TransformState.Projection);
                Matrix view = device.GetTransform(TransformState.View);
                Matrix world = device.GetTransform(TransformState.World);
                Matrix m = world * view * proj;
                Vector3 nearPosition = Vector3.Unproject(near, device.Viewport.X, device.Viewport.Y, pictureBox1.Width, pictureBox1.Height, device.Viewport.MinZ,device.Viewport.MaxZ,m);
                Vector3 farPosition = Vector3.Unproject(far, device.Viewport.X, device.Viewport.Y, pictureBox1.Width, pictureBox1.Height, device.Viewport.MinZ, device.Viewport.MaxZ, m);

                Vector3 dir = farPosition - nearPosition;

                selections.Clear();
                selections = 
                floorMap.selectObject(nearPosition, dir);

            }



            if(changed) {
                device.SetTransform(TransformState.View,
                                     camera.getLookAtLh());
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(selections.Count() == 1) {
                if(selections.Last() is MapNode) {
                    MapNode node = (MapNode)selections.Last();
                } else if(selections.Last() is MapEdge) {
                    //マップエッジのダイアログ表示

                    EdgeForm ef = new EdgeForm();
                    ef.MapEdge = (MapEdge)selections.Last();
                    ef.ShowDialog();
                }
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (MqoModel m in selections)
                {
                    if (m is MapEdge)
                    {
//                        m.Enabled = !m.Enabled;
                    }
                }
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            drawFloorModel = ((CheckBox)sender).Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            showAlwaysFloorMap = ((CheckBox)sender).Checked;

        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
        }

        private void pictureBox1_DoubleClick_1(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Middle)
            {
                camera.clearPosition();
                device.SetTransform(TransformState.View,
                                     camera.getLookAtLh());
            }

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            mapSizeX.Enabled = !((CheckBox)sender).Checked;
            mapSizeY.Enabled = !((CheckBox)sender).Checked;
        }

        private void saveWithNameMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            String current = Directory.GetCurrentDirectory();
            dialog.InitialDirectory = current + @"\projects\" + projectName + @"\data";
            dialog.FileName = "samplefloor.bin";

            if(dialog.ShowDialog() == DialogResult.OK) {
                //editorの設定
                EditorConfig editorConfig = new EditorConfig();
                editorConfig.drawFloorModel = drawFloorModel;
                editorConfig.showAlwaysFloorMap = showAlwaysFloorMap;
                editorConfig.lockMapSize = !mapSizeX.Enabled;
                editorConfig.mapXsize = floorMap.getXsize();
                editorConfig.mapYsize = floorMap.getYsize();

                List<List<Boolean>> xb = new List<List<bool>>();
                foreach(List<MapEdge> ll in floorMap.MapPosition.EdgeXList) {
                    List<Boolean> l = new List<Boolean>();
                    foreach(MapEdge me in ll) {
                        l.Add(me.Enabled);
                    }
                    xb.Add(l);
                }
                editorConfig.edgeXListEnable = xb;

                List<List<Boolean>> yb = new List<List<bool>>();
                foreach(List<MapEdge> ll in floorMap.MapPosition.EdgeYList) {
                    List<Boolean> l = new List<bool>();
                    foreach(MapEdge me in ll) {
                        l.Add(me.Enabled);
                    }
                    yb.Add(l);
                }
                editorConfig.edgeYListEnable = yb;

                editorConfig.encounterRatio = (int)encountRatioUpDown.Value;


                editorConfig.toFile(dialog.FileName);
            }
        }

        private void openFileMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            String current = Directory.GetCurrentDirectory();
            dialog.InitialDirectory = current + @"\projects\" + projectName + @"\data";

            if(dialog.ShowDialog() == DialogResult.OK) {
                floorMap.dispose();
                floorMap = new FloorMap(device);

                EditorConfig editorConfig = new EditorConfig();
                Boolean result = editorConfig.fromFile(dialog.FileName);
                if(!result) {
                    MessageBox.Show("ファイルの読み込みが正常に終了しませんでした。");
                }

                drawFloorModel = editorConfig.drawFloorModel;
                showAlwaysFloorMap = editorConfig.showAlwaysFloorMap;

                floorMap.setX(editorConfig.mapXsize);
                floorMap.setY(editorConfig.mapYsize);
                mapSizeX.Value = editorConfig.mapXsize;
                mapSizeY.Value = editorConfig.mapYsize;

                //コントロールの読み込み
                showFloorModelcheckBox.Checked = drawFloorModel;
                showAlwaysFloorModelcheckBox.Checked = showAlwaysFloorMap;
                mapSizeX.Enabled = !editorConfig.lockMapSize;
                mapSizeY.Enabled = !editorConfig.lockMapSize;
                encountRatioUpDown.Value = editorConfig.encounterRatio;

                //enableの構築
                for (int i = 0; i < editorConfig.mapXsize - 1; i++)
                {
                    List<Boolean> bl = editorConfig.edgeXListEnable.ElementAt(i);
                    List<MapEdge> el = floorMap.MapPosition.EdgeXList.ElementAt(i);
                    for (int j = 0; j < editorConfig.mapYsize; j++)
                    {
                        Boolean b = bl.ElementAt(j);
                        el.ElementAt(j).Enabled = b;
                    }
                }
                for (int i = 0; i < editorConfig.mapXsize; i++ )
                {
                    List<Boolean> bl = editorConfig.edgeYListEnable.ElementAt(i);
                    List<MapEdge> el = floorMap.MapPosition.EdgeYList.ElementAt(i);
                    for (int j = 0; j < editorConfig.mapYsize - 1; j++)
                    {
                        Boolean b = bl.ElementAt(j);
                        el.ElementAt(j).Enabled = b;
                    }

                }
            }

        }


    }
}
