using HonasGame.JSON;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.Tiled
{
    public abstract class TiledLayer
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Id { get; protected set; }
        public string Name { get; protected set; }

        public TiledLayer(JObject jObj)
        {
            Id      = (int)jObj.GetValue<double>("id");
            Name    =      jObj.GetValue<string>("name");
        }
    }
}
