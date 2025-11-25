using System;
using System.Collections.Generic;


[Serializable]
public class LevelProgress
{
    public int Level;
    public bool IsPassed;
}

[Serializable]
public class PlayerProgress
{
    public List<LevelProgress> Progress = new List<LevelProgress>();
}
