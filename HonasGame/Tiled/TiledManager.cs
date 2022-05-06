using HonasGame.ECS;
using HonasGame.JSON;
using HonasGame.Tiled.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.Tiled
{
    public static class TiledManager
    {
        /// <summary>
        /// Delegate for spawning an entity.
        /// </summary>
        /// <param name="properties">The Tiled object</param>
        /// <returns>An instantiated instance of the entity</returns>
        public delegate Entity EntitySpawner(TiledObject obj);

        private static Dictionary<string, EntitySpawner> _spawnerDefs = new Dictionary<string, EntitySpawner>();

        private static TiledMap _gotoMap = null;

        public static void AddSpawnerDefinition(string type, EntitySpawner spawner)
        {
            _spawnerDefs.Add(type, spawner);
        }

        internal static Entity InstantiateEntity(string type, TiledObject obj)
        {
            return _spawnerDefs[type](obj);
        }

        internal static void Update(GameTime gameTime)
        {
            if(_gotoMap != null)
            {
                Scene.Clear();
                Scene.ClearLayers();
                Camera.Bounds = new Rectangle(0, 0, _gotoMap.Width * _gotoMap.TileWidth, _gotoMap.Height * _gotoMap.TileHeight);

                // Add layers first, otherwise objects trying to add will not exist
                for(int i = 0; i < _gotoMap.Layers.Count; i++)
                {
                    Scene.AddLayer(_gotoMap.Layers[i].Name, 100 * i);
                }

                for (int i = 0; i < _gotoMap.Layers.Count; i++)
                {
                    if (_gotoMap.Layers[i] is TiledTileLayer tileLayer)
                    {
                        var tilesEntity = new TiledTilesEntity(tileLayer.Width, tileLayer.Height, _gotoMap.TileWidth, _gotoMap.TileHeight, tileLayer.Data);
                        tilesEntity.SetTilesets(_gotoMap._tilesets);

                        Scene.AddEntity(tilesEntity, tileLayer.Name);
                    }
                    else if(_gotoMap.Layers[i] is TiledObjectLayer objectLayer)
                    {
                        foreach(TiledObject obj in objectLayer.Objects)
                        {
                            Scene.AddEntity(InstantiateEntity(obj.Type, obj), objectLayer.Name);
                        }
                    }
                    else if(_gotoMap.Layers[i] is TiledImageLayer imageLayer)
                    {
                        Scene.AddEntity(new TiledImageEntity(imageLayer), imageLayer.Name);
                    }
                }

                _gotoMap = null;
            }
        }

        public static void Goto(TiledMap map)
        {
            _gotoMap = map;
        }
    }
}
