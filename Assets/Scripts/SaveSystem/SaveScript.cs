using System;
using SubjectGuide.Managers;
using TMPro;
using UnityEngine;

namespace SubjectGuide.SaveSystem {
  public class SaveScript : MonoBehaviour {
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private TMP_InputField _saveInput;
    [SerializeField] private TMP_Dropdown _saveExtDropdown;

    private static string s_savePath = String.Empty;
    private static SaveExtensionType s_saveExt = SaveExtensionType.Json;

    public void HandleSaveInput() {
      s_savePath = _saveInput.text;
    }

    public void HandleSaveExtension() {
      s_saveExt = _saveExtDropdown.value switch {
        (int)SaveExtensionType.Json => SaveExtensionType.Json,
        (int)SaveExtensionType.Binary => SaveExtensionType.Binary,
        _ => throw new Exception("Dropdown was out of bounds"),
      };
    }

    public void Load() {

    }

    public void Save() {
      switch (s_saveExt) {
        case SaveExtensionType.Json:
          // SaveHelper.Save<string>(s_savePath, _gameManager);
          break;
        case SaveExtensionType.Binary:
          // SaveHelper.Save<byte[]>(s_savePath, _gameManager);
          break;
      }
    }

    public static string SavePath {
      get { return s_savePath; }
      set { s_savePath = value; }
    }
  }
}