using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeService : SingletonBehaviour<NodeService>
{
    [SerializeField]
    private CellScript middleCell;
    [SerializeField]
    private CellScript endCell;
    [SerializeField]
    private Transform board;
    [SerializeField]
    private Image endNodeSymbol;

    private int gridSize = 25;
    private float scalingSize = 30000;
    private Team[] gridArray = new Team[25];

    void Start()
    {
        CreateLevel();
    }


    public void CreateLevel()
    {
        for (int i = 0; i < gridArray.Length; i++)
        {
            gridArray[i] = Team.Default;
        }
        JsonReader.Instance.AccessNodeList(ref gridArray, LevelOverController.Instance.CurrentLevel);
        CreateGrid();
    }

    private void CreateGrid()
    {
        CellScript currentObj;
        for (int i = 0; i < gridSize; i++)
        {
            switch (gridArray[i])
            {
                case Team.Blue:
                    CreateTeam(out currentObj, Color.blue);
                    break;
                case Team.Green:
                    CreateTeam(out currentObj, Color.green);
                    break;
                case Team.Cyan:
                    CreateTeam(out currentObj, Color.cyan);
                    break;
                case Team.Red:
                    CreateTeam(out currentObj, Color.red);
                    break;
                case Team.Yellow:
                    CreateTeam(out currentObj, Color.yellow);
                    break;
                default:
                    currentObj = GameObject.Instantiate<CellScript>(middleCell);
                    break;
            }
            currentObj.transform.SetParent(board.transform);
            currentObj.transform.localScale = currentObj.transform.localScale * Screen.width / scalingSize;
        }
    }

    private void CreateTeam(out CellScript currentObj, Color nodeColor)
    {
        Image endNodeS;
        currentObj = Instantiate(endCell);
        currentObj.EndNodeColor = nodeColor;
        endNodeS = Instantiate(endNodeSymbol, currentObj.transform);
        endNodeS.color = nodeColor;
        endNodeS.transform.SetParent(currentObj.transform);
    }

}
