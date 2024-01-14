using System;
using SubjectGuide.Globals;
using SubjectGuide.Managers;
using SubjectGuide.Utils;
using UnityEngine;

namespace SubjectGuide {
  public class Subject : MonoBehaviour, ISubject {
    private GameManager _gameManager;
    [SerializeField] private Animator _animator;

    private Guid _subjectId = Guid.NewGuid();
    [SerializeField] private double _speed = 0.0f;
    [SerializeField] private double _agility = 0.0f;
    [SerializeField] private double _constitution = 0.0f;

    private bool _isMoving = false;

    private void Awake() {
      _gameManager = FindObjectOfType<GameManager>();
      GenerateAttributes();
    }

    private void Start() {
      CheckAgility();
      CheckSpeed();
      CheckConstitution();
    }

    private void Update() {
      _isMoving = _gameManager.NavGrid.Busy;

      _animator.SetBool(GameConstants.IsMoving, _isMoving);
    }

    public void GenerateAttributes() {
      _speed = RandomNumberGenerator.RandomDoubleValue(1, 5);
      _agility = RandomNumberGenerator.RandomDoubleValue(1, 10);
      _constitution = RandomNumberGenerator.RandomDoubleValue(1, 20);
    }

    public void OverrideData(double speed, double agility, double constitution) {
      _speed = speed;
      _agility = agility;
      _constitution = constitution;
    }

    private void CheckAgility() {
      if (_agility >= _speed && _agility >= _constitution) {
        transform.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.cyan;
      }
    }

    private void CheckConstitution() {
      if (_constitution >= _speed && _constitution >= _agility) {
        var scale = transform.localScale * (float)(_constitution / 15);
        var max = Vector3.Max(scale, transform.localScale);
        transform.localScale = max;
      }
    }

    private void CheckSpeed() {
      if (_speed >= _constitution && _speed >= _agility) {
        transform.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.yellow;
      }
    }

    public Guid SubjectId => _subjectId;
    public double Speed => _speed;
    public double Agility => _agility;
    public double Constitution => _constitution;
    public Transform Transform => transform;
  }
}