using System.Collections.Generic;


[System.Serializable]
public class CurrentLevel 
{
    public List<TeamFormation> Nodes = new List<TeamFormation>();
}

public class LevelInfo
{
    public List<CurrentLevel> Levels = new List<CurrentLevel>();
}
