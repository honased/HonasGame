using HonasGame.JSON;
using System.Collections.Generic;
using System.IO;

namespace HonasGame.Tiled
{
    public class TiledMap
    {
        internal class Tileset
        {
            public int FirstGid { get; private set; }
            public string Source { get; private set; }

            public Tileset(JObject jObj)
            {
                FirstGid = (int)jObj.GetValue<double>("firstgid");
                Source = Path.GetFileNameWithoutExtension(jObj.GetValue<string>("source"));
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public List<TiledLayer> Layers { get; private set; }
        internal List<Tileset> _tilesets;

        public TiledMap(JObject jObj)
        {
            Width       = (int)jObj.GetValue<double>("width");
            Height      = (int)jObj.GetValue<double>("height");
            TileWidth   = (int)jObj.GetValue<double>("tilewidth");
            TileHeight  = (int)jObj.GetValue<double>("tileheight");

            Layers = new List<TiledLayer>();

            JArray arr = jObj.GetValue<JArray>("layers");

            for(int i = 0; i < arr.Count; i++)
            {
                JObject layer = arr.GetValue<JObject>(i);
                switch(layer.GetValue<string>("type"))
                {
                    case "tilelayer":
                        Layers.Add(new TiledTileLayer(layer));
                        break;

                    case "objectgroup":
                        Layers.Add(new TiledObjectLayer(layer));
                        break;

                    case "imagelayer":
                        Layers.Add(new TiledImageLayer(layer));
                        break;
                }
            }

            _tilesets = new List<Tileset>();
            arr = jObj.GetValue<JArray>("tilesets");
            for(int i = 0; i < arr.Count; i++)
            {
                _tilesets.Add(new Tileset(arr.GetValue<JObject>(i)));
            }
        }

        public void Goto()
        {
            TiledManager.Goto(this);
        }
    }
}
