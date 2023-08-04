using System;
using System.IO;
using UnityEngine;

namespace TowerDefence
{
    [Serializable]
    public class DataSaver<T>
    {
        public T Data;

        public static bool TryLoad(string filename, ref T data)
        {
            string path = FileHandler.Path(filename);
            
            if (File.Exists(path))
            {
                var dataString = File.ReadAllText(path);
                var saver = JsonUtility.FromJson<DataSaver<T>>(dataString);
                data = saver.Data;

                return true;
            }

            return false;
        }

        public static void Save(string filename, T data)
        {
            var wrapper = new DataSaver<T> { Data = data };
            string dataString = JsonUtility.ToJson(wrapper);
            File.WriteAllText(FileHandler.Path(filename), dataString);
        }
    }
}
