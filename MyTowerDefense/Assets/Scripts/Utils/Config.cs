using System;


[Serializable]
public class Config
{
    public Game game;
}

[Serializable]
public class Game 
{
    public int lives;
    public int gold;
}