using System.Collections.Generic;
using SubjectGuide.Utils;

namespace SubjectGuide.Map {
  public class MapSave {
    public List<MapObstacleData> MapObstacles;
    public List<SubjectSaveData> SubjectsInfo;
    public SVector2 MapDimensions;

    public MapSave(
      List<MapObstacleData> mapObstacles,
      List<SubjectSaveData> subjectsInfo,
      SVector2 mapDimensions
    ) {
      MapObstacles = mapObstacles;
      SubjectsInfo = subjectsInfo;
      MapDimensions = mapDimensions;
    }
  }
}