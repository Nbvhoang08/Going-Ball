using System.Collections.Generic;
using UnityEngine;

namespace Hapiga.Core.Runtime.Utils
{
    public static class RuntimeAssetUtils
    {
        /// <summary>
        /// Returns the reference to a ScriptableObject found at the path.
        /// The path will consider the Resources folder as the root.
        /// So if we have an asset named 'myAsset' under Resources/Data/myAsset,
        /// then the path will be "Data/myAsset". 
        /// Do not add the .asset file extension as it will not work.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relativeFilePath">Usage: "/Data/myAsset" Do not use the .asset extension as it won't work.</param>
        public static Object GetScriptableObjectFromResources<T>(string path)
        {
            return Resources.Load(path, typeof(T));
        }

        /// <summary>
        /// Returns all the references to a ScriptableObjects found at the path.
        /// The path will consider the Resources folder as the root.
        /// So if we have an asset named 'myAsset' under Resources/Data/myAsset,
        /// then the path will be "Data/myAsset". 
        /// Do not add the .asset file extension as it will not work.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relativeFilePath">Usage: "/Data/myAsset" Do not use the .asset extension as it won't work.</param>
        public static Object[] GetScriptableObjectsFromResources(string path)
        {
            return Resources.LoadAll(path);
        }

        public static T[] GetScriptableObjectArray<T>(Object[] objects) where T : ScriptableObject
        {
            if (objects == null || objects.Length == 0) { return null; }
            List<T> list = new List<T>();
            for (int i = 0; i < objects.Length; i++) { list.Add((T)objects[i]); }
            return list.ToArray();
        }
    }
}
