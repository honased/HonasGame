using HonasGame.JSON;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HonasGame.Tiled
{
    public class TiledTileset
    {
        public int Columns { get; private set; }
        public int ImageWidth { get; private set; }
        public int ImageHeight { get; private set; }
        public string Name { get; private set; }
        public int TileCount { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public string Image { get; private set; }

        public TiledTileset(JObject jObj)
        {
            Columns = (int)jObj.GetValue<double>("columns");
            ImageWidth = (int)jObj.GetValue<double>("imagewidth");
            ImageHeight = (int)jObj.GetValue<double>("imageheight");
            Name = jObj.GetValue<string>("name");
            TileCount = (int)jObj.GetValue<double>("tilecount");
            TileWidth = (int)jObj.GetValue<double>("tilewidth");
            TileHeight = (int)jObj.GetValue<double>("tileheight");
            Image = Path.GetFileNameWithoutExtension(jObj.GetValue<string>("image"));
        }

        public Vector2 GetTileLocation(uint index)
        {
            return new Vector2((index % Columns) * TileWidth, (index / Columns) * TileHeight);
        }
    }
}
