using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SlimDX.Direct3D9;

namespace gameEditor2Lib
{
    public class MqoParser
    {
        private Device device;

        public MqoParser(Device device) {
            this.device = device;
        }

        public MqoModel parse(String filePath, MqoModel model) {
            StreamReader r = new StreamReader(filePath, Encoding.GetEncoding("SJIS"));
            String line;
            while((line = r.ReadLine()) != null) {
                model.appendLine(line);
            }

            VertexModel vm = new VertexModel(model, device);
            model.VertexModel = vm;
            model.onInit(device);
            return model;
        }
    }
}
