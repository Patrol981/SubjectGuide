using System;
using SubjectGuide.Managers;
using UnityEngine;

namespace SubjectGuide.SaveSystem {
  public class SaveScript : MonoBehaviour {
    [SerializeField] private GameManager _gameManager;

    private static string s_savePath = String.Empty;
    private static SaveExtensionType s_saveExt = SaveExtensionType.Json;

    public void Load() {

    }

    public void Save() {
      var saveData = SaveHelper.GetMapObstacles(_gameManager.MapScript.ObstaclesParent);
      switch (s_saveExt) {
        case SaveExtensionType.Json:
          SaveHelper.Save<string>(s_savePath, saveData);
          break;
        case SaveExtensionType.Binary:
          SaveHelper.Save<byte[]>(s_savePath, saveData);
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