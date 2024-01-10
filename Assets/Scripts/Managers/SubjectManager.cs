using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SubjectGuide.Managers {
  public class SubjectManager : MonoBehaviour {
    [SerializeField] private ISubject[] _subjects = new ISubject[0];
    private ISubject _guide;
    [SerializeField] private GameObject _subjectPrefab;

    private void Update() {
#if DEBUG
      if (Input.GetKeyDown(KeyCode.P)) {
        PrintArray();
      }
#endif
    }

    public Task Init() {
      // hardcoded for now 3 subjects
      for (short i = 0; i < 3; i++) {
        var sub = CreateSubject(new(i, 0, 0));
        AddSubject(sub);
      }

#if DEBUG
      PrintArray();
#endif

      _guide = _subjects[0];

      Debug.Log(_guide.SubjectId);
      Debug.Log(_guide.Agility);
      Debug.Log(_guide.Speed);
      Debug.Log(_guide.Constitution);

      return Task.CompletedTask;
    }

    public void AddSubject(ISubject subject) {
      var tmp = _subjects;
      _subjects = new ISubject[tmp.Length + 1];
      Array.Copy(tmp, 0, _subjects, 0, tmp.Length);
      _subjects[tmp.Length] = subject;
    }

    public ISubject CreateSubject(Vector3 vec3) {
      var go = Instantiate(_subjectPrefab, vec3, Quaternion.identity, null);
      return go.GetComponent<ISubject>();
    }

    public void SetGuide(ISubject subject) {
      _guide = subject;
    }

    public ISubject Guide => _guide;
    public ISubject[] Subjects => _subjects;

#if DEBUG
    private void PrintArray() {
      foreach (var sub in _subjects) {
        Debug.Log(sub.SubjectId);
      }
    }
#endif
  }
}