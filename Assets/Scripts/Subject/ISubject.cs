using System;

namespace SubjectGuide {
  public interface ISubject {
    Guid SubjectId { get; }
    double Speed { get; }
    double Agility { get; }
    double Constitution { get; }
  }
}