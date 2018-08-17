using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace SummerEditor
{

    public class EDirectoryHelper
    {
        public static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        public static void CreateDirectory(string path)
        {
            if (Directory.Exists(path)) return;
            Directory.CreateDirectory(path);
        }

        public static List<T> CollectAll<T>(string path) where T : UnityEngine.Object
        {
            List<T> l = new List<T>();
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                if (file.Contains(".meta")) continue;
                T asset = (T)AssetDatabase.LoadAssetAtPath(file, typeof(T));
                if (asset == null) throw new Exception("Asset is not " + typeof(T) + ": " + file);
                l.Add(asset);
            }
            return l;
        }
    }
}
