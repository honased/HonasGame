using HonasGame.JSON;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.Tiled
{
    public class TiledObjectLayer : TiledLayer
    {
        public TiledObject[] Objects { get; private set; }

        public TiledObjectLayer(JObject jObj) : base(jObj)
        {
            JArray arr = jObj.GetValue<JArray>("objects");
            Objects = new TiledObject[arr.Count];
            
            for(int i = 0; i < Objects.Length; i++)
            {
                Objects[i] = new TiledObject(arr.GetValue<JObject>(i));
            }
        }
    }
}
