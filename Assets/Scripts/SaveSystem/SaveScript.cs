using System;
using System.Threading.Tasks;
using SubjectGuide.Managers;
using SubjectGuide.Map;
using UnityEngine;

namespace SubjectGuide.SaveSystem {
  public class SaveScript : MonoBehaviour {
    [SerializeField] private GameManager _gameManager;

    private static string s_savePath = String.Empty;
    private static SaveExtensionType s_saveExt = SaveExtensionType.Json;

    public async void Load() {
      string ext = String.Empty;
      switch (s_saveExt) {
        case SaveExtensionType.Binary:
          ext = ".bin";
          break;
        case SaveExtensionType.Json:
          ext = ".json";
          break;
      }
      var loadedData = await SaveHelper.Load($"{s_savePath}{ext}");
      await _gameManager.LoadSave(loadedData);
    }

    public void Save() {
      var obstacles = SaveHelper.GetMapObstacles(_gameManager.MapScript.ObstaclesParent);
      var subjects = SaveHelper.GetSubjectsInfo(_gameManager.SubjectManager.SubjectParent);
      var mapSave = new MapSave(obstacles, subjects);
      switch (s_saveExt) {
        case SaveExtensionType.Json:
          SaveHelper.Save<string>(s_savePath, mapSave);
          break;
        case SaveExtensionType.Binary:
          SaveHelper.Save<byte[]>(s_savePath, mapSave);
          break;
      }
    }

    public static string SavePath {
      get { return s_savePath; }
      set { s_savePath = value; }
    }

    public static SaveExtensionType SaveExtensionType {
      get { return s_saveExt; }
      set { s_saveExt = value; }
    }
  }
}