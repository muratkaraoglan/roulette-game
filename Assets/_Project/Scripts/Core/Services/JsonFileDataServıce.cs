using System.IO;
using UnityEngine;

namespace _Project.Scripts.Core.Services
{
    public class JsonFileDataServıce<T> : ISaveService<T> where T: new()
    {
        private readonly string _filePath;

        public JsonFileDataServıce(string fileName)
        {
            _filePath = Path.Combine(Application.persistentDataPath, fileName);
        }

        public void Save(T data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_filePath, json);
        }

        public T Load()
        {
            if (!File.Exists(_filePath))
                return new T();

            string json = File.ReadAllText(_filePath);
            return JsonUtility.FromJson<T>(json);
        }
    }
}