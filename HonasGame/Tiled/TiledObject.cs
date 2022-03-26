using HonasGame.JSON;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.Tiled
{
    public class TiledObject
    {
        private const uint FLIPPED_HORIZONTALLY = 0x80000000;
        private const uint FLIPPED_VERTICALLY = 0x40000000;

        public float X { get; private set; }
        public float Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public bool FlippedHorizontally { get; private set; }
        public bool FlippedVertically { get; private set; }

        public TiledPolyLine PolyLine { get; private set; }

        public class TiledPolyLine
        {
            public List<Tuple<float, float>> Positions { get; private set; }
            public TiledPolyLine(JArray jArr, float x, float y)
            {
                Positions = new List<Tuple<float, float>>();

                for(int i = 0; i < jArr.Count; i++)
                {
                    JObject jObj = jArr[i] as JObject;
                    Positions.Add(new Tuple<float, float>((float)jObj.GetValue<double>("x") + x, (float)jObj.GetValue<double>("y") + y));
                }
            }
        }

        public TiledObject(JObject jObj)
        {
            X = (float)jObj.GetValue<double>("x");
            Y = (float)jObj.GetValue<double>("y");
            Width = (int)jObj.GetValue<double>("width");
            Height = (int)jObj.GetValue<double>("height");
            Id = (int)jObj.GetValue<double>("id");
            Name = jObj.GetValue<string>("name");
            Type = jObj.GetValue<string>("type");
            if(jObj.CheckField("gid"))
            {
                int gid = (int)jObj.GetValue<double>("gid");
                FlippedHorizontally = (gid & FLIPPED_HORIZONTALLY) > 0;
                FlippedVertically = (gid & FLIPPED_VERTICALLY) > 0;
            }
            if(jObj.CheckField("polyline"))
            {
                PolyLine = new TiledPolyLine(jObj.GetValue<JArray>("polyline"), X, Y);
            }
            else
            {
                PolyLine = null;
            }
        }
    }
}
