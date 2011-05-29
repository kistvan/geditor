using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D9;
using SlimDX;
using gameEditor2Lib.util;

namespace gameEditor2
{
    class Camera
    {
        private Device device;

        private MouseDelta mDelta = new MouseDelta();

        private Boolean rotate = false;
        private Boolean trans = false;

        private Vector3 from = new Vector3(-3.0f, 2.0f, -3.0f);
        private Vector3 to = Vector3.Zero;

        private Matrix viewMat = Matrix.Identity;
        private Matrix lookAtLh ;


        public Boolean Rotate {
            set { rotate = value;
            if (!value)
            {
                mDelta.clear();
            }
            }
            get { return rotate; }
        }
        public Boolean Trans {
            set { trans = value;
                if(!value) {
                    mDelta.clear();
                }
            }
            get { return trans; }
        }
        public Camera(Device device) {
            this.device = device;
            lookAtLh = Matrix.LookAtLH(
                                 from,
                                                 to,
                                                 new Vector3(0.0f, 1.0f, 0.0f));
        }

        public void rotateCamera(int x, int y) {
            mDelta.calc(x, y);


            float dx =  mDelta.getDeltaX() * 0.6f;
            float dy =  mDelta.getDeltaY() * 0.6f;

            Vector3 xDir = Vector3.TransformNormal(new Vector3(1, 0, 0), (viewMat));
            Vector3 yDir = Vector3.TransformNormal(new Vector3(0, 1, 0), (viewMat));

            //カメラ基準のX軸
            Vector3 rotDir = Vector3.Cross(to - from, yDir);


            Quaternion rt = Quaternion.RotationAxis(yDir, Radian.getRadian(dx))
            *Quaternion.RotationAxis(rotDir, Radian.getRadian(dy));

            //TODO 180度の制限

            Matrix rot = Matrix.RotationQuaternion(rt);

            viewMat *= rot;


        }

        public void transCamera(int x, int y) {
            mDelta.calc(x, y);

            float dx = mDelta.getDeltaX() * -0.05f;
            float dy = mDelta.getDeltaY() * 0.05f;

            Vector3 zaxis = Vector3.Normalize(to - from);
            Vector3 xaxis = Vector3.Normalize(Vector3.Cross(new Vector3(0.0f, 1.0f, 0.0f), zaxis));
            Vector3 yaxis = Vector3.Cross(zaxis, xaxis);

            from -= xaxis * dx;
            to -= xaxis * dx;
            from -= yaxis * dy;
            to -= yaxis * dy;
            lookAtLh = Matrix.LookAtLH(
                                 from,
                                                 to,
                                                 new Vector3(0.0f, 1.0f, 0.0f));



        }

        public void zoomCamera(int mouseDelta) {
           float  zoom = mouseDelta / 120.0f;

            Vector3 zaxis = Vector3.Normalize(to - from);
            Vector3 xaxis = Vector3.Normalize(Vector3.Cross(new Vector3(0.0f, 1.0f, 0.0f), zaxis));
            Vector3 yaxis = Vector3.Cross(zaxis, xaxis);

            float r = Vector3.Distance(to, from + zaxis * zoom);
            if(r < 1.0f) {
                return;
            }

            from += zaxis * zoom;
//            to += zaxis * zoom;
            lookAtLh = Matrix.LookAtLH(
                                 from,
                                                 to,
                                                 new Vector3(0.0f, 1.0f, 0.0f));
        }

        public void clearPosition()
        {
            viewMat = Matrix.Identity;
            from = new Vector3(-3.0f, 2.0f, -3.0f);
            to = Vector3.Zero;
            mDelta.clear();
            lookAtLh = Matrix.LookAtLH(
                                 from,
                                                 to,
                                                 new Vector3(0.0f, 1.0f, 0.0f));
        }

        public Matrix getLookAtLh() {
            return viewMat * lookAtLh;
        }

    }
}
