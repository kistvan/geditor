using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D9;

namespace gameEditor2Client.screenEffect
{
    interface ScreenEffect
    {
        void anim(Effect effect);

        Boolean finished();

    }
}
