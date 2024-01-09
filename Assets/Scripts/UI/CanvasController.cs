using UnityEngine;
using TMPro;
using SubjectGuide.SaveSystem;
using System;
using SubjectGuide.Managers;

namespace SubjectGuide.UI {
  public class CanvasController : MonoBehaviour {
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private TMP_InputField _saveInput;
    [SerializeField] private TMP_Dropdown _saveExtDropdown;

    private void Awake() {
      _gameManager = FindObjectOfType<GameManager>();
    }

    public void HandleSaveInput() {
      SaveScript.SavePath = _saveInput.text;
    }

    public void HandleSaveExtension() {
      SaveScript.SaveExtensionType = _saveExtDropdown.value switch {
        (int)SaveExtensionType.Json => SaveExtensionType.Json,
        (int)SaveExtensionType.Binary => SaveExtensionType.Binary,
        _ => throw new Exception("Dropdown was out of bounds"),
      };

      Debug.Log(SaveScript.SaveExtensionType);
    }

    public void Save() {
      _gameManager.SaveScript.Save();
    }

    public void Load() {
      _gameManager.SaveScript.Load();
    }
  }
}