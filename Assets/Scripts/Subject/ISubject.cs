using System;
using UnityEngine;

namespace SubjectGuide {
  public interface ISubject {
    Guid SubjectId { get; }
    double Speed { get; }
    double Agility { get; }
    double Constitution { get; }
    Transform Transform { get; }

    void OverrideData(double speed, double agility, double constitution);
  }
}