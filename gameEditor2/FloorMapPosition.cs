using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using gameEditor2.model;
using SlimDX;
using gameEditor2Lib;

namespace gameEditor2
{
    class FloorMapPosition
    {
        private List<List<MapNode>> list = new List<List<MapNode>>();
        private List<List<MapEdge>> edgeXList = new List<List<MapEdge>>();
        private List<List<MapEdge>> edgeYList = new List<List<MapEdge>>();
        public List<List<MapEdge>> EdgeXList {
            get { return edgeXList; }
        }
        public List<List<MapEdge>> EdgeYList {
            get { return edgeYList; }
        }

        public FloorMapPosition(MqoModel model) {
            List<MapNode> xList = new List<MapNode>();
            xList.Add((MapNode)model);
            list.Add(xList);
        }

        public void draw() {
            foreach(List<MapNode> l in list) {
                foreach(MapNode m in l) {
                    m.draw();
                }
            }
            foreach (List<MapEdge> l in edgeXList)
            {
                foreach (MapEdge m in l)
                {
                    m.draw();
                }
            }
            foreach (List<MapEdge> l in edgeYList)
            {
                foreach (MapEdge m in l)
                {
                    m.draw();
                }
            }
        }

        public void incrementX(FloorMap.GetModel getModel, FloorMap.getEdgeModel getEdgeModel) {
            int xsize = getXsize();
            int ysize = getYsize();
            List<MapNode> l = new List<MapNode>();
            List<MapEdge> el = new List<MapEdge>();
            for (int i = 0; i < ysize; i++ )
            {
                MapNode node = getModel(xsize, i);
                l.Add(node);

                MapEdge eg = getEdgeModel(xsize, i, true);
                el.Add(eg);
            }
            list.Add(l);
            edgeXList.Add(el);
            if (ysize > 1)
            {
                List<MapEdge> ell = new List<MapEdge>();
                //既存のYEdgeを全部追加 回数だけ合わせる
                    for (int i = 0; i < ysize - 1; i++ )
                    {
                        MapEdge m = getEdgeModel(xsize, i, false);
                        ell.Add(m);
                  }
                edgeYList.Add(ell);
            }
        }

        public void decrementX()
        {
            int ysize = getYsize();
            List<MapNode> l = list.ElementAt(list.Count() - 1);
            foreach(MqoModel m in l) {
                m.dispose();
            }
            l.Clear();
            list.RemoveAt(list.Count() - 1);

            List<MapEdge> el = edgeXList.ElementAt(edgeXList.Count() - 1);
            foreach(MapEdge e in el) {
                e.dispose();
            }
            el.Clear();
            edgeXList.RemoveAt(edgeXList.Count() - 1);

            if(ysize > 1) {
                List<MapEdge> ll = edgeYList.ElementAt(edgeYList.Count() - 1);
                foreach(MapEdge e in ll) {
                    e.dispose();
                }
                ll.Clear();
                edgeYList.RemoveAt(edgeYList.Count() - 1);
            }
        }

        public void incrementY(FloorMap.GetModel getModel, FloorMap.getEdgeModel getEdgeModel) {
            int xsize = getXsize();
            int ysize = getYsize();
            for (int i = 0; i < list.Count(); i++ )
            {
                for (int j = 0; j < list.ElementAt(i).Count(); j++ )
                {
                    list.ElementAt(i).Add(getModel(i, ysize));
                    break;
                }
            }
            for (int i = 0; i < xsize; i++ )
            {
                List<MapEdge> l = new List<MapEdge>();
                if (edgeYList.Count() > i)
                {
                    l = edgeYList.ElementAt(i);
                    MapEdge e = getEdgeModel(i, ysize - 1, false);
                    l.Add(e);
                }
                else {
                    MapEdge e = getEdgeModel(i, ysize- 1, false);
                    l.Add(e);
                    edgeYList.Add(l);
                }
            }
            if(xsize > 1) {
                //XList全部一個増やす
                int xPos = 1;
                foreach(List<MapEdge> xl in edgeXList) {
                    MapEdge e = getEdgeModel(xPos, ysize, true);
                    xl.Add(e);
                    xPos++;
                }
            }
        }

        public void decrementY() {
            int xsize = getXsize();
            for (int i = 0; i < list.Count(); i++)
            {
                for (int j = 0; j < list.ElementAt(i).Count(); j++)
                {
                    List<MapNode> l = list.ElementAt(i);
                    MqoModel m = l.ElementAt(l.Count() - 1);
                    m.dispose();
                    l.RemoveAt(l.Count() - 1);
                    break;
                }
            }
            foreach (List<MapEdge> l in edgeYList)
            {
                MapEdge e = l.ElementAt(l.Count() - 1);
                e.dispose();
                l.RemoveAt(l.Count() - 1);
            }
            if (xsize > 1)
            {
                //xlistを全部ひとつ減らす
                foreach(List<MapEdge> me in edgeXList) {
                    MapEdge e = me.ElementAt(me.Count() - 1);
                    e.dispose();
                    me.RemoveAt(me.Count() - 1);
                }
            }
        }

        public int getXsize()
        {
            return list.Count();
        }

        public int getYsize() {
            return list.ElementAt(0).Count();
        }
        




        //選択系は分割するかも
        public List<MqoModel> getIntercects(Vector3 pos, Vector3 dir) {
            List<MqoModel> list = new List<MqoModel>();
            float tempD = float.MaxValue;
            float distance = 0;
            MqoModel addModel = null;
            foreach (List<MapNode> l in this.list)
            {
                foreach (MapNode n in l) {
                    if (n.intercects(pos, dir, out distance)) {
                        if(distance < tempD) {
                            addModel = n;
                            tempD = distance;
                        }
                    }
                }
            }
            foreach(List<MapEdge> el in this.edgeXList) {
                foreach (MapEdge e in el)
                {
                    if(e.intercects(pos, dir, out distance)) {
                        if (distance < tempD)
                        {
                            addModel = e;
                            tempD = distance;
                        }
                    }
                }
            }
            distance = 0;
            foreach (List<MapEdge> el in this.edgeYList)
            {
                foreach (MapEdge e in el)
                {
                    if (e.intercects(pos, dir, out distance))
                    {
                        if (distance < tempD)
                        {
                            addModel = e;
                            tempD = distance;
                        }
                    }
                }
            }
            if (addModel != null)
            {
                list.Add(addModel);
                addModel = null;
            }
            return list;
        }

        public void dispose()
        {
            foreach(List<MapEdge> ll in edgeXList) {
                foreach(MapEdge e in ll) {
                    e.dispose();
                }
            }
            foreach(List<MapEdge> ll in edgeYList) {
                foreach(MapEdge e in ll) {
                    e.dispose();
                }
            }
            edgeXList.Clear();
            edgeYList.Clear();
        }
    }

}
