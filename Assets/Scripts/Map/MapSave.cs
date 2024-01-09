using System.Collections.Generic;

namespace SubjectGuide.Map {
  public class MapSave {
    public List<MapObstacleData> MapObstacles;

    public MapSave(
      List<MapObstacleData> mapObstacles
    ) {
      MapObstacles = mapObstacles;
    }
  }
}