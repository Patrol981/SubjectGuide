using SubjectGuide.Utils;

namespace SubjectGuide {
  public class SubjectSaveData {
    public SVector3 Position;
    public SVector3 Rotation;
    public double Constitution;
    public double Agility;
    public double Speed;

    public SubjectSaveData(
      SVector3 position,
      SVector3 rotation,
      double constitution,
      double agility,
      double speed
    ) {
      Position = position;
      Rotation = rotation;
      Constitution = constitution;
      Agility = agility;
      Speed = speed;
    }
  }
}