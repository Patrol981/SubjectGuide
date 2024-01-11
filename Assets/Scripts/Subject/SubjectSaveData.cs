using SubjectGuide.Utils;

namespace SubjectGuide {
  public class SubjectSaveData {
    public SVector3 Position;
    public SVector3 Rotation;

    public SubjectSaveData(
      SVector3 position,
      SVector3 rotation
    ) {
      Position = position;
      Rotation = rotation;
    }
  }
}