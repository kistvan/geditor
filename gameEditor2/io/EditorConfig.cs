using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using gameEditor2.model;

namespace gameEditor2.io
{
    class EditorConfig
    {
        public String header = "3DG";

        public float version = 0.01f;

        public Boolean drawFloorModel = false;

        public Boolean showAlwaysFloorMap = false;

        public Boolean lockMapSize = false;

        public int mapXsize = 1;

        public int mapYsize = 1;

        public void toFile(String path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fs);
            try
            {
                writer.Write(header);
                writer.Write(version);
                writer.Write(drawFloorModel);
                writer.Write(showAlwaysFloorMap);
                writer.Write(lockMapSize);
                writer.Write(mapXsize);
                writer.Write(mapYsize);


            }
            finally {
                writer.Close();
            }

        }

        public Boolean fromFile(String path) {
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryReader reader = new BinaryReader(fs);
            try
            {
                String h = reader.ReadString();
                float v = BitConverter.ToSingle(reader.ReadBytes(sizeof(float)), 0);
                drawFloorModel = reader.ReadBoolean();
                showAlwaysFloorMap = reader.ReadBoolean();
                lockMapSize = reader.ReadBoolean();
                mapXsize = reader.ReadInt32();
                mapYsize = reader.ReadInt32();

                //edgeXのxはmapX-1
                //yはmapYsize

                return true;

            }
            catch (EndOfStreamException eex)
            {
                //握りつぶす
                return false;
            }
            finally
            {
                reader.Close();
            }
        }
    }

}
