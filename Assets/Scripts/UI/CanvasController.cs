using UnityEngine;
using TMPro;
using SubjectGuide.SaveSystem;
using System;
using SubjectGuide.Managers;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace SubjectGuide.UI {
  public class CanvasController : MonoBehaviour {
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private TMP_InputField _saveInput;
    [SerializeField] private TMP_Dropdown _saveExtDropdown;

    [SerializeField] private Transform _hooksPanel;
    [SerializeField] private Sprite _portrait;

    private void Awake() {
      _gameManager = FindObjectOfType<GameManager>();
    }

    public Task CreateUIHooks(ISubject[] subjects) {
      foreach (var sub in subjects) {
        var go = new GameObject();
        go.name = sub.SubjectId.ToString();
        go.transform.parent = _hooksPanel;
        go.AddComponent<RectTransform>();
        go.AddComponent<CanvasRenderer>();
        go.AddComponent<Button>();
        go.AddComponent<Image>();
        go.GetComponent<Button>().onClick.AddListener(delegate { HandleClick(sub, go); });
        go.GetComponent<Image>().sprite = _portrait;
      }
      return Task.CompletedTask;
    }

    public void ClickButton(string id) {
      var targetButton = _hooksPanel.Find(id);
      ClearButtonsColor();
      targetButton.GetComponent<Image>().color = Color.red;
    }

    public void ClearHooks() {
      var btns = _hooksPanel.GetComponentsInChildren<Button>();
      foreach (var btn in btns) {
        Destroy(btn.gameObject);
      }
    }

    private void HandleClick(ISubject subject, GameObject button) {
      _gameManager.SubjectManager.SetGuide(subject);
      ClearButtonsColor();
      button.GetComponent<Image>().color = Color.red;
    }

    private void ClearButtonsColor() {
      var buttons = _hooksPanel.GetComponentsInChildren<Image>();
      foreach (var btn in buttons) {
        btn.color = Color.white;
      }
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
    }

    public void Save() {
      _gameManager.SaveScript.Save();
    }

    public void Load() {
      _gameManager.SaveScript.Load();
    }
  }
}