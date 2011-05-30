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
using gameEditor2Client.model;
using gameEditor2Lib;
using gameEditor2Lib.io;
using SlimDX.DirectInput;

namespace gameEditor2Client
{
        using D3dDvice = SlimDX.Direct3D9.Device;
using gameEditor2Client.input;
    using gameEditor2Lib.util;
    using gameEditor2Client.screenEffect;
    public partial class Form1 : Form
    {
        private SlimDX.Direct3D9.Device device;

        private EffectRepository repository = EffectRepository.getInstance();

        private Floor floor;

        private SlimDX.DirectInput.Keyboard kbDevice = new SlimDX.DirectInput.Keyboard(new DirectInput());

        private CameraPosition cameraPosition;

        private MoveInput moveInput;

        private TopPositionVertex topVertex;

        private ScreenEffects sEffects = ScreenEffects.getInstance();

        public Form1()
        {
            InitializeComponent();

            device = new D3dDvice(new Direct3D(), 0, SlimDX.Direct3D9.DeviceType.Hardware, this.Handle, CreateFlags.HardwareVertexProcessing, new PresentParameters
            {
                BackBufferWidth = this.ClientSize.Width,
                BackBufferHeight = this.ClientSize.Height,
                Windowed = true,
            
            });

            DeviceContext.init(device);


            device.SetRenderState(RenderState.ZEnable, true);
            device.SetRenderState(RenderState.Lighting, false);
            device.SetLight(0, new Light()
            {
                Type = LightType.Directional,
                Diffuse = Color.White,
                Ambient = Color.Gray,
                Direction = new Vector3(1.0f, 1.0f, 1.0f),
            });
//            device.EnableLight(0, true);
            device.SetTransform(TransformState.World, Matrix.Identity);
            //射影変換
            device.SetTransform(TransformState.Projection,
                                 Matrix.PerspectiveFovLH(Radian.getRadian(65),
                                                         (float)this.ClientSize.Width / (float)this.ClientSize.Height,
                                                         0.1f, 1500.0f));
            //ビュー
            Camera camera = new Camera(device);
            cameraPosition = new CameraPosition(camera);
            moveInput = new MoveInput(cameraPosition);


            //mesh = Mesh.CreateSphere(device, 0.125f, 16, 16);

            Material mt = new Material()
            {
                Diffuse = Color.White,
                Ambient = Color.Gray,
            };
            device.Material = mt;

            floor = new Floor(@".\projects\sample\model\floor.mqo", device);

            //device.SetRenderState(RenderState.FogColor, 0);
            //device.SetRenderState(RenderState.FogStart, 0.1f);
            //device.SetRenderState(RenderState.FogEnd, 1.0f);
            //device.SetRenderState(RenderState.FogVertexMode, FogMode.Linear);
            //device.SetRenderState(RenderState.FogEnable, true);

            //とりいそぎ固定で
            EditorConfig config = new EditorConfig();
            config.fromFile(@"projects\sample\data\samplefloor.bin");

            cameraPosition.setSize(config.mapXsize, config.mapYsize);

            floor.edgeXEnable = config.edgeXListEnable;
            floor.edgeYEnable = config.edgeYListEnable;
            cameraPosition.Floor = floor;

            topVertex = new TopPositionVertex(device, new TopPosition(0,0, device.Viewport.Width, device.Viewport.Height));

            cameraPosition.moveEvent.encountRatio = config.encounterRatio;

        }

        public void init() {
              Result rs = kbDevice.SetCooperativeLevel(this.Handle, CooperativeLevel.Background | CooperativeLevel.Nonexclusive);
            kbDevice.Properties.BufferSize = 10;
        }

        public void drawFrame() {
            /*
             * 回転->向いてる壁のイベント->エンカウント
             * 移動->向いてる壁のイベント->先のイベント->エンカウント
             * 
             */
            moveInput.accept(kbDevice);

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Gray, 1.0f, 0);
            device.BeginScene();

            //Matrix m = Matrix.Translation(0.0f, 0.5f, 1.0f);
            //device.SetTransform(TransformState.World, m);
            //mesh.DrawSubset(0);
            //device.SetTransform(TransformState.World, Matrix.Identity);

            floor.draw();

            if(sEffects.hasEffect()) {
                SlimDX.Direct3D9.Effect effect = repository.getEffect(@"resources\fx\topPoly.fx");
                sEffects.draw(effect);
            }

            device.EndScene();
            device.Present();
        }

        public void release()
        {
            if(kbDevice != null) {
                kbDevice.Unacquire();
            }
            topVertex.dispose();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            //TODO デバイス自体がロストしている可能性がある
            if(kbDevice == null) {
                kbDevice = new SlimDX.DirectInput.Keyboard(new DirectInput());
                init();
            }
            Result rs = kbDevice.Acquire();
            if(rs.IsSuccess) {
                moveInput.aquire = true;
            }
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            moveInput.aquire = false;
            kbDevice.Unacquire();
            kbDevice.Dispose();
            kbDevice = null;

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}
