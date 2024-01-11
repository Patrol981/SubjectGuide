using System.Collections.Generic;

namespace SubjectGuide.Map {
  public class MapSave {
    public List<MapObstacleData> MapObstacles;
    public List<SubjectSaveData> SubjectsInfo;

    public MapSave(
      List<MapObstacleData> mapObstacles,
      List<SubjectSaveData> subjectsInfo
    ) {
      MapObstacles = mapObstacles;
      SubjectsInfo = subjectsInfo;
    }
  }
}