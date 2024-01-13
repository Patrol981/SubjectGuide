using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SubjectGuide.Utils;
using UnityEngine;

namespace SubjectGuide.Managers {
  public class SubjectManager : MonoBehaviour {
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private ISubject[] _subjects = new ISubject[0];
    private ISubject _guide = null;
    [SerializeField] private GameObject _subjectPrefab;
    [SerializeField] private Transform _subjectParent;

    private void Update() {
#if DEBUG
      if (Input.GetKeyDown(KeyCode.P)) {
        PrintArray();
      }
#endif
    }

    public Task Init() {
      var rand = RandomNumberGenerator.RandomInt(3, _gameManager.MapScript.MapData.SubjectsMaxRange);
      for (short i = 0; i < rand; i++) {
        var sub = CreateSubject(new(i, 0, 0), Vector3.zero);
        AddSubject(sub);
      }

#if DEBUG
      PrintArray();
#endif
      return Task.CompletedTask;
    }

    public void AddSubject(ISubject subject) {
      var tmp = _subjects;
      _subjects = new ISubject[tmp.Length + 1];
      Array.Copy(tmp, 0, _subjects, 0, tmp.Length);
      _subjects[tmp.Length] = subject;
    }

    public void ClearSubjects() {
      for (short i = 0; i < _subjects.Length; i++) {
        Destroy(_subjects[i].Transform.gameObject);
      }
      _subjects = new ISubject[0];
    }

    public Transform[] GatherSubjects() {
      var subjects = new List<Transform> {
        _guide.Transform
      };
      var exludeGuide = _subjects
        .Where(x => x.SubjectId != _guide.SubjectId)
        .Select(x => x.Transform)
        .ToArray();
      subjects.AddRange(exludeGuide);
      return subjects.ToArray();
    }

    public Task LoadSubjectData(ReadOnlySpan<SubjectSaveData> subsData) {
      for (short i = 0; i < subsData.Length; i++) {
        var sub = CreateSubject(
          SVector3.ToVector3(subsData[i].Position),
          SVector3.ToVector3(subsData[i].Rotation)
        );
        sub.OverrideData(subsData[i].Speed, subsData[i].Agility, subsData[i].Constitution);
        AddSubject(sub);
      }
      return Task.CompletedTask;
    }

    public ISubject CreateSubject(Vector3 vec3, Vector3 rot3) {
      var go = Instantiate(_subjectPrefab, vec3, Quaternion.Euler(rot3), _subjectParent);
      return go.GetComponent<ISubject>();
    }

    public void SetGuide(ISubject subject) {
      _guide = subject;
    }

    public ISubject Guide => _guide;
    public ISubject[] Subjects => _subjects;

    public Transform SubjectParent => _subjectParent;

#if DEBUG
    private void PrintArray() {
      foreach (var sub in _subjects) {
        Debug.Log(sub.SubjectId);
      }
    }
#endif
  }
}