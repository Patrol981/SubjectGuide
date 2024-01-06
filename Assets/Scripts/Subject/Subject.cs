using System;
using SubjectGuide.Utils;
using UnityEngine;

namespace SubjectGuide {
  public class Subject : MonoBehaviour, ISubject {
    private Guid _subjectId = Guid.NewGuid();

    private double _speed = 0.0f;
    private double _agility = 0.0f;
    private double _constitution = 0.0f;

    public void GenerateAttributes() {
      _speed = RandomNumberGenerator.RandomDoubleValue(1, 5);
      _agility = RandomNumberGenerator.RandomDoubleValue(1, 10);
      _constitution = RandomNumberGenerator.RandomDoubleValue(1, 20);
    }

    public Guid SubjectId => _subjectId;
    public double Speed => _speed;
    public double Agility => _agility;
    public double Constitution => _constitution;
  }
}