using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace gameEditor2Lib.model.mqo
{
    class MaterialChunk
    {
        private Boolean useVertexColor = false;
        private Color color = Color.White;
        public Color Color {
            get { return color; }
        }

        public MaterialChunk(String line) {

//            Console.WriteLine(line);
            int start = line.IndexOf(" col(");
            String l = line.Substring(start+5);
            l = l.Substring(0, l.IndexOf(')'));
            String[] data = l.Split(' ');
            float n = 0.0039f;
            int r = (int)(float.Parse(data[0]) / n);
            r = Math.Min(r, 255);
            int g = (int)(float.Parse(data[1]) / n);
            g = Math.Min(g, 255);
            int b = (int)(float.Parse(data[2]) / n);
            b = Math.Min(b, 255);
            int a = (int)(float.Parse(data[3]) / n);
            a = Math.Min(a, 255);
            color = Color.FromArgb(a, r, g, b);
        }
    }
}
