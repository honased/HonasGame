using HonasGame.JSON;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.Tiled
{
    public class TiledTileLayer : TiledLayer
    {
        public uint[] Data { get; private set; }

        public TiledTileLayer(JObject jObj) : base(jObj)
        {
            Width = (int)jObj.GetValue<double>("width");
            Height = (int)jObj.GetValue<double>("height");

            JArray dataArr = jObj.GetValue<JArray>("data");
            Data = new uint[dataArr.Count];

            for(int i = 0; i < Data.Length; i++)
            {
                Data[i] = Convert.ToUInt32(dataArr.GetValue<double>(i));
            }
        }
    }
}
