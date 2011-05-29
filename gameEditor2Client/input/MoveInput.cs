using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.DirectInput;
using System.Collections;

namespace gameEditor2Client.input
{
    class MoveInput
    {

        private CameraPosition camera;

        private KeyboardState state;
        private KeyboardState preState;

        public Boolean aquire = false;

        public MoveInput(CameraPosition camera) {
            this.camera = camera;
        }

        public void accept(Keyboard kb) {
            if(!aquire) {
                return;
            }
            state = kb.GetCurrentState();
            if(preState == null) {
                preState = kb.GetCurrentState();
                return;
            }

            if (isPressStart(Key.W))
            {
                camera.moveForward();
            } else if(isPressStart(Key.S)) {
                camera.moveBack();
            } else if(isPressStart(Key.D)) {
                camera.turnRight();
            } else if(isPressStart(Key.A)) {
                camera.turnLeft();
            }

            preState = state;
        }

        public Boolean isPressStart(Key key) {
            return preState.IsReleased(key) && state.IsPressed(key);
        }

        public Boolean isRelease(Key key) {
            return preState.IsPressed(key) && state.IsReleased(key);
        }

    }
}
