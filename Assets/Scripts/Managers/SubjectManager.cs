using System;
using System.Collections.Generic;
using UnityEngine;

namespace SubjectGuide.Managers {
  public class SubjectManager : MonoBehaviour {
    [SerializeField] private ISubject[] _subjects = new ISubject[0];

    private void Update() {
#if DEBUG
      if (Input.GetKeyDown(KeyCode.P)) {
        var go = new GameObject("test");
        go.AddComponent<Subject>();
        AddSubject(go.GetComponent<Subject>());
        PrintArray();
#endif
      }
    }

    public void AddSubject(ISubject subject) {
      var tmp = _subjects;
      _subjects = new ISubject[tmp.Length + 1];
      Array.Copy(tmp, 0, _subjects, 0, tmp.Length);
      _subjects[tmp.Length] = subject;
    }

#if DEBUG
    private void PrintArray() {
      foreach (var sub in _subjects) {
        Debug.Log(sub.SubjectId);
      }
    }
#endif
  }
}