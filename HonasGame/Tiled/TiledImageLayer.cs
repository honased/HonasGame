using HonasGame.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HonasGame.Tiled
{
    public class TiledImageLayer : TiledLayer
    {
        public string Image { get; private set; }
        public float OffsetX { get; private set; } = 0.0f;
        public float OffsetY { get; private set; } = 0.0f;
        public float Opacity { get; private set; } = 1.0f;
        public float ParallaxX { get; private set; } = 1.0f;
        public float ParallaxY { get; private set; } = 1.0f;

        public TiledImageLayer(JObject jObj) : base(jObj)
        {
            Image = Path.GetFileNameWithoutExtension(jObj.GetValue<string>("image"));
            Opacity = (float)jObj.GetValue<double>("opacity");
            if(jObj.CheckField("offsetx")) OffsetX = (float)jObj.GetValue<double>("offsetx");
            if(jObj.CheckField("offsety")) OffsetY = (float)jObj.GetValue<double>("offsety");
            if(jObj.CheckField("parallaxx")) ParallaxX = (float)jObj.GetValue<double>("parallaxx");
            if(jObj.CheckField("parallaxy")) ParallaxY = (float)jObj.GetValue<double>("parallaxy");
        }
    }
}
