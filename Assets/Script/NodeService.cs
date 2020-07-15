using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeService : SingletonBehaviour<NodeService>
{
    [SerializeField]
    private GameObject middleCell;
    [SerializeField]
    private GameObject endCell;
    [SerializeField]
    private Transform board;
    [SerializeField]
    private GameObject endNodeSymbol;

    private int gridSize = 25;
    private Team[] gridArray = { Team.Default, Team.Default, Team.Default, Team.Red, Team.Green,
        Team.Default, Team.Default, Team.Blue, Team.Green, Team.Default, Team.Red, 
        Team.Default, Team.Default, Team.Default, Team.Default, Team.Orange, Team.Blue, Team.Default, Team.Yellow, 
        Team.Orange, Team.Default, Team.Default, Team.Default, Team.Default,Team.Yellow };
    
    protected override void Awake()
    {
        base.Awake();

        GameObject currentObj = null;
        for(int i = 0; i < gridSize; i++)
        {
            //if(gridArray[i] == 0)
            //{
            //    currentObj = Instantiate(middleCell);
            //}
            //else
            //{
            //    currentObj = Instantiate(endCell);
            //    endNodeS = Instantiate(endNodeSymbol, currentObj.transform);
            //    endNodeS.transform.SetParent(currentObj.transform);
            //}

            switch (gridArray[i])
            {
                case Team.Blue:
                    CreateTeam(out currentObj);
                    break;
                case Team.Green:
                    CreateTeam(out currentObj);
                    break;
                case Team.Orange:
                    CreateTeam(out currentObj);
                    break;
                case Team.Red:
                    CreateTeam(out currentObj);
                    break;
                case Team.Yellow:
                    CreateTeam(out currentObj);
                    break;
                default:
                    currentObj = Instantiate(middleCell);
                    break;
            }
            currentObj.transform.SetParent(board.transform);
            currentObj.transform.localScale = currentObj.transform.localScale * 0.037f;
        }
    }

    private void CreateTeam(out GameObject currentObj)
    {
        GameObject endNodeS = null;
        currentObj = Instantiate(endCell);
        endNodeS = Instantiate(endNodeSymbol, currentObj.transform);
        endNodeS.transform.SetParent(currentObj.transform);
    }
}
