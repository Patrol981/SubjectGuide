using System;
using System.IO;
using System.Threading.Tasks;
using SubjectGuide.Globals;
using UnityEngine;

namespace SubjectGuide.SaveSystem {
  public static class SaveHelper {
    public static void Save<T>(string fileName, object data) {
      var json = SerializeObjectToJson(data);
      SaveData<T>(fileName, json);
    }

    public static async Task<object> Load(string fileNameWithExt) {
      var data = await LoadData(fileNameWithExt);
      return data;
    }

    private static async void SaveData<T>(string fileName, object data) {
      var fullPath = GetSavePath();
      Directory.CreateDirectory(fullPath);
      if (typeof(T) == typeof(string)) {
        var finalPath = Path.Combine(fullPath, $"{fileName}.bin");
        await File.WriteAllTextAsync(finalPath, data.ToString());
      } else if (typeof(T) == typeof(byte[])) {
        var finalPath = Path.Combine(fullPath, $"{fileName}.json");
        await File.WriteAllBytesAsync(finalPath, (byte[])data);
      } else {
        throw new InvalidCastException("Unsupported data type. Please use byte[] or string");
      }
    }

    private static async Task<object> LoadData(string fileNameWithExt) {
      var fullPath = GetSavePath();
      var finalPath = Path.Combine(fullPath, fileNameWithExt);
      object data;
      if (fileNameWithExt.Contains(".bin")) {
        data = await File.ReadAllBytesAsync(finalPath);
      } else if (fileNameWithExt.Contains(".json")) {
        data = await File.ReadAllTextAsync(finalPath);
      } else {
        throw new FileLoadException("Incorrect file type. Please use either .json or .bin");
      }

      return data;
    }

    private static object SerializeObjectToJson(object data) {
      return JsonUtility.ToJson(data);
    }

    private static string GetSavePath() {
      var documentsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      var fullPath = Path.Combine(documentsLocation, GameConstants.SaveLocation);
      return fullPath;
    }
  }
}