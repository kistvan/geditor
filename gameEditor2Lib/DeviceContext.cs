using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D9;

namespace gameEditor2Lib
{
    public class DeviceContext
    {
        private Device device;

        private static DeviceContext context;

        private DeviceContext() { }

        public static void init(Device device) {
            getContext().device = device;
        }

        public static DeviceContext getContext() { 
            if(context == null) {
                context = new DeviceContext();
            }
            return context;
        }

        public static Device getDevice() {
            return getContext().device;
        }
    }
}
