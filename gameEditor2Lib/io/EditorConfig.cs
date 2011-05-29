using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace gameEditor2Lib.io
{
    public class EditorConfig
    {
        public String header = "3DG";

        public float version = 0.01f;

        public Boolean drawFloorModel = false;

        public Boolean showAlwaysFloorMap = false;

        public Boolean lockMapSize = false;

        public List<List<Boolean>> edgeXListEnable;

        public List<List<Boolean>> edgeYListEnable;

        public int mapXsize = 1;

        public int mapYsize = 1;

        public int encounterRatio = 0;

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

                //edgeXのxはmapX-1
                //yはmapYsize
                foreach (List<Boolean> edgel in edgeXListEnable)
                {
                    foreach (Boolean ed in edgel)
                    {
                        writer.Write(ed);
                    }
                }
                foreach (List<Boolean> edgel in edgeYListEnable)
                {
                    foreach (Boolean ed in edgel)
                    {
                        writer.Write(ed);
                    }
                }
                writer.Write(encounterRatio);
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
                edgeXListEnable = new List<List<bool>>();
                for(int i = 0; i < mapXsize - 1; i++) {
                    List<Boolean> bl = new List<bool>();
                    for (int j = 0;  j < mapYsize; j++)
                    {
                        Boolean b = reader.ReadBoolean();
                        bl.Add(b);
                    }
                    edgeXListEnable.Add(bl);
                }
                //edgeYのxはmapX
                //yはmapYsize-1
                edgeYListEnable = new List<List<bool>>();
                for (int i = 0; i < mapXsize; i++)
                {
                    List<Boolean> bl = new List<bool>();
                    for (int j = 0; j < mapYsize - 1; j++ )
                    {
                        Boolean b = reader.ReadBoolean();
                        Console.WriteLine(b);
                        bl.Add(b);
                    }
                    edgeYListEnable.Add(bl);
                }
                encounterRatio = reader.ReadInt32();
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
