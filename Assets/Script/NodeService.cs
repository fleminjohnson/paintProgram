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
    private Team[] gridArray = { Team.Default, Team.Default, Team.Default, Team.Red, Team.Green,
        Team.Default, Team.Default, Team.Blue, Team.Green, Team.Default, Team.Red, 
        Team.Default, Team.Default, Team.Default, Team.Default, Team.Orange, Team.Blue, Team.Default, Team.Yellow, 
        Team.Orange, Team.Default, Team.Default, Team.Default, Team.Default,Team.Yellow };

    void Start()
    {
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
                case Team.Orange:
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
            currentObj.transform.localScale = currentObj.transform.localScale * 0.037f;
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
