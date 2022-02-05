using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.Assets
{
    public static class AssetLibrary
    {
        private static Dictionary<string, object> _assets = new Dictionary<string, object>();

        public static void AddAsset(string key, object asset)
        {
            _assets.Add(key, asset);
        }

        public static T GetAsset<T>(string key)
        {
            if(_assets[key] is T asset)
            {
                return asset;
            }

            throw new Exception($"Asset {key} is not of the given type.");
        }
    }
}
