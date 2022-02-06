using HonasGame.JSON;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.Tiled
{
    public class TiledObject
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }

        public TiledObject(JObject jObj)
        {
            X = jObj.GetValue<double>("x");
            Y = jObj.GetValue<double>("y");
            Width = (int)jObj.GetValue<double>("width");
            Height = (int)jObj.GetValue<double>("height");
            Id = (int)jObj.GetValue<double>("id");
            Name = jObj.GetValue<string>("name");
            Type = jObj.GetValue<string>("type");
        }
    }
}
