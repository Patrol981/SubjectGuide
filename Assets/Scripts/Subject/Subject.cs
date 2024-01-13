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
    private double _speed = 0.0f;
    private double _agility = 0.0f;
    private double _constitution = 0.0f;

    private bool _isMoving = false;

    private void Awake() {
      _gameManager = FindObjectOfType<GameManager>();
    }

    private void Start() {
      GenerateAttributes();
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

    public Guid SubjectId => _subjectId;
    public double Speed => _speed;
    public double Agility => _agility;
    public double Constitution => _constitution;
    public Transform Transform => transform;
  }
}