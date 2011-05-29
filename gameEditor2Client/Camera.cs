using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D9;
using SlimDX;
using gameEditor2Lib.util;

namespace gameEditor2Client
{
    class Camera
    {
        private Device device;
        private Vector3 pos = new Vector3(0.0f, 0.5f, 0.0f);
        private Vector3 eye = new Vector3(0.0f, 0.5f, 1.0f);

        private Vector3 cameraAjust = Vector3.Zero;

        private Vector3 moveTarget = Vector3.Zero;

        private PointOfCompass poc = new PointOfCompass();
        public PointOfCompass Poc {
            get { return poc;}
        }

        private Vector3 posDiff = Vector3.Zero;

        private float diffFactor = -0.5f;

        public Camera(Device device) {
            this.device = device;
            device.SetTransform(TransformState.View, Matrix.LookAtLH(pos, eye, new Vector3(0, 1, 0)));
        }

        public void moveForward() {
            Vector3 dir = eye - pos;
            dir.Normalize();

            pos += dir;
            eye += dir;

            //1歩カメラを引く
            Vector3 tempV = posDiff * -1;
            pos += tempV;//元に戻す
            eye += tempV;
            posDiff = poc.getDir() * diffFactor;
            pos += posDiff;
            eye += posDiff;

            //Console.WriteLine("pos:" + pos);
            //Console.WriteLine("eye:" + eye);
            device.SetTransform(TransformState.View, Matrix.LookAtLH(pos, eye, new Vector3(0, 1, 0)));

        }

        public void moveBack() {
            Vector3 dir = pos - eye;
            dir.Normalize();
            moveTarget = dir;

            pos += dir;
            eye += dir;

            //1歩カメラを引く
            Vector3 tempV = posDiff * -1;
            pos += tempV;//元に戻す
            eye += tempV;
            posDiff = poc.getDir() * diffFactor;
            pos += posDiff;
            eye += posDiff;

            //Console.WriteLine("pos:" + pos);
            //Console.WriteLine("eye:" + eye);
            device.SetTransform(TransformState.View, Matrix.LookAtLH(pos, eye, new Vector3(0, 1, 0)));
        }

        public void turnRight() {
            poc.turnRight();
            if (poc.getDirection() == PointOfCompass.NORTH)
            {
                eye = pos + new Vector3(0,0,1);
            }
            Matrix mat = Matrix.Identity;

            //1歩カメラを引く
            Vector3 tempV = posDiff * -1;
            pos += tempV;//元に戻す
            posDiff = poc.getDir() * diffFactor;
            if (poc.getDirection() != PointOfCompass.NORTH)
            {

                mat = Matrix.RotationQuaternion(Quaternion.RotationAxis(new Vector3(0,1,0), Radian.ninty));
            }

            Vector3 transEye = eye;
            Vector4 v4 = Vector3.Transform(eye, mat);
            pos += posDiff;
            eye = pos + poc.getDir();

            //Console.WriteLine("pos:" + pos);
            //Console.WriteLine("eye:"+eye);

            device.SetTransform(TransformState.View, Matrix.LookAtLH(pos, eye, new Vector3(0, 1, 0)));
        }

        public void turnLeft() {
            poc.turnLeft();

            if (poc.getDirection() == PointOfCompass.NORTH)
            {
                eye = pos + new Vector3(0, 0, 1);
            }
            Matrix mat = Matrix.Identity;
            //1歩カメラを引く
            Vector3 tempV = posDiff * -1;
            pos += tempV;//元に戻す
            posDiff = poc.getDir() * diffFactor;
            if (poc.getDirection() != PointOfCompass.NORTH)
            {

                mat = Matrix.RotationQuaternion(Quaternion.RotationAxis(new Vector3(0, 1, 0), -1 * Radian.ninty));
            }

            Vector3 transEye = eye;
            Vector4 v4 = Vector3.Transform(eye, mat);
            pos += posDiff;
            eye = pos + poc.getDir();

            //Console.WriteLine("pos:" + pos);
            //Console.WriteLine("eye:" + eye);

            device.SetTransform(TransformState.View, Matrix.LookAtLH(pos, eye, new Vector3(0, 1, 0)));
        }

    }
}
