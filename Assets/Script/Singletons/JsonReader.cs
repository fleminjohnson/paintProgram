﻿using System.Collections.Generic;
using UnityEngine;

public class JsonReader : SingletonBehaviour<JsonReader>
{
    public LevelInfo NodeList = new LevelInfo();

    void Start()
    {
        TextAsset assets = Resources.Load("LevelData") as TextAsset;

        if(assets != null)
        {
            NodeList = JsonUtility.FromJson<LevelInfo>(assets.text);
        }
        else
        {
            print("Assets is null");
        }
    }

    public void AccessNodeList(ref Team[] gridArray, int currentLevel)
    {
        CurrentLevel level = NodeList.Levels[currentLevel];

        for (int i =0; i < NodeList.Levels[1].Nodes.Count; i++)
        {
            gridArray[level.Nodes[i].Value] = StringToEnum(level.Nodes[i].NodeName);
        }
    }

    private Team StringToEnum(string name)
    {
        switch (name)
        {
            case "Red":
                return Team.Red;
            case "Green":
                return Team.Green;
            case "Blue":
                return Team.Blue;
            case "Cyan":
                return Team.Cyan;
            case "Yellow":
                return Team.Yellow;
        }
        return Team.Default;
    }

}
