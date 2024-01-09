using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SubjectGuide.Globals;
using SubjectGuide.Map;
using UnityEngine;
using Newtonsoft.Json;
using SubjectGuide.Utils;
using System.Text;

namespace SubjectGuide.SaveSystem {
  public static class SaveHelper {
    public static void Save<T>(string fileName, object data) {
      var json = SerializeObjectToJson(data);
      SaveData<T>(fileName, json);
    }

    public static async Task<MapSave> Load(string fileNameWithExt) {
      var data = await LoadData(fileNameWithExt);
      return data;
    }

    public static List<MapObstacleData> GetMapObstacles(Transform parent) {
      var obstaclesData = new List<MapObstacleData>();
      var obstacles = parent.gameObject.GetComponentsInChildren<MapObstacle>();
      foreach (var obstacle in obstacles) {
        // make sure that objects are being saved with the most actual data sets
        obstacle.ObstacleData.Position = SVector3.FromVector3(obstacle.transform.position);
        obstacle.ObstacleData.Rotation = SVector3.FromVector3(obstacle.transform.rotation.eulerAngles);
        obstaclesData.Add(obstacle.ObstacleData);
      }
      return obstaclesData;
    }

    private static async void SaveData<T>(string fileName, object data) {
      var fullPath = GetSavePath();
      Directory.CreateDirectory(fullPath);
      if (typeof(T) == typeof(string)) {
        var finalPath = Path.Combine(fullPath, $"{fileName}.json");
        await File.WriteAllTextAsync(finalPath, (string)data);
      } else if (typeof(T) == typeof(byte[])) {
        var finalPath = Path.Combine(fullPath, $"{fileName}.bin");
        var byteData = Encoding.UTF8.GetBytes((string)data);
        await File.WriteAllBytesAsync(finalPath, byteData);
      } else {
        throw new InvalidCastException("Unsupported data type. Please use byte[] or string");
      }
    }

    private static async Task<MapSave> LoadData(string fileNameWithExt) {
      var fullPath = GetSavePath();
      var finalPath = Path.Combine(fullPath, fileNameWithExt);
      MapSave data;
      if (fileNameWithExt.Contains(".bin")) {
        var bytes = await File.ReadAllBytesAsync(finalPath);
        var stringData = Encoding.UTF8.GetString(bytes);
        data = DeserializeJsonToObject(stringData);
      } else if (fileNameWithExt.Contains(".json")) {
        // var text = = await File.ReadAllTextAsync(finalPath);
        using var reader = File.OpenText(finalPath);
        data = DeserializeJsonToObject(reader);
      } else {
        throw new FileLoadException("Incorrect file type. Please use either .json or .bin");
      }

      return data;
    }

    private static string SerializeObjectToJson(object data) {
      // using Newtonsoft, mainly because JsonUtility does not throw errors if the structure is wrong
      return JsonConvert.SerializeObject(data);
    }

    private static MapSave DeserializeJsonToObject(StreamReader reader) {
      var serializer = new JsonSerializer();
      var data = (MapSave)serializer.Deserialize(reader, typeof(MapSave));
      return data;
    }

    private static MapSave DeserializeJsonToObject(string jsonString) {
      return JsonConvert.DeserializeObject<MapSave>(jsonString);
    }

    private static string GetSavePath() {
      var documentsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      var fullPath = Path.Combine(documentsLocation, GameConstants.SaveLocation);
      return fullPath;
    }
  }
}