using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D9;
using gameEditor2.model;
using gameEditor2Lib.util;
using SlimDX;
using gameEditor2Lib;

namespace gameEditor2
{


    class FloorMap
    {
        private FloorMapPosition position;
        public FloorMapPosition MapPosition {
            get { return position; }
        }
        private MqoParser mParser;

        public delegate MapNode GetModel(int x, int y);
        public delegate MapEdge getEdgeModel(int x, int y, Boolean xEdge);

        private Effect effect;

        public FloorMap(Device device) {
            mParser = new MqoParser(device);
            MqoModel model = getModelInstance(0,0);
            position = new FloorMapPosition(model);

            effect = EffectRepository.getInstance().getEffect(".\\resources\\fx\\selectedObj.fx");

        }

        public void draw()
        {
            effect.Technique = "technique0";
            effect.Begin();
            position.draw();
            effect.End();
        }

        private MapNode getModelInstance(int x, int y) {
            MqoModel model = mParser.parse(".\\resources\\model\\node.mqo", new MapNode());
            model.translation(1.0f * x, 0.0f, y * 1.0f);
            return (MapNode)model;
        }
        private MapEdge getEdgeInstance(int x, int y, Boolean xEdge) {
            MqoModel model = mParser.parse(".\\resources\\model\\edge.mqo", new MapEdge());
            if (xEdge)
            {
                model.translation(x - 0.5f, 0.0f, y * 1.0f);
            }
            else {
                model.translation(x, 0.0f, y + 0.5f);
                model.rotateY(Radian.ninty);

            }
            return (MapEdge)model;
        }

        public void setX(decimal d) {
            int index = (int)d;
            while(position.getXsize() < index) {
                position.incrementX(getModelInstance, getEdgeInstance);
            }
            while(position.getXsize() > index && position.getXsize() != 1) {
                position.decrementX();
            }
        }

        public void setY(decimal d) {
            int index = (int)d;
            while (position.getYsize() < index)
            {
                position.incrementY(getModelInstance, getEdgeInstance);
            }
            while (position.getYsize() > index && position.getYsize() != 1)
            {
                position.decrementY();
            }
        }

        public List<MqoModel> selectObject(Vector3 pos, Vector3 dir) {
            List<MqoModel> list = position.getIntercects(pos, dir);
            return list;
        }

        public int getXsize() {
            return position.getXsize();
        }

        public int getYsize() {
            return position.getYsize();
        }

        public void dispose() {
            position.dispose();
        }
    }
}
