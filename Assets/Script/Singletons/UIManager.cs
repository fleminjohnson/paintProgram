using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : SingletonBehaviour<UIManager>
{
    [SerializeField]
    private TMP_Text flow;

    [SerializeField]
    private TMP_Text move;

    [SerializeField]
    private GameObject winScreen;

    [SerializeField]
    private GameObject finishScreen;

    [SerializeField]
    private GameObject failScreen;

    void Start()
    {
        EventServices.GenericInstance.OnWin += WinRoutine;
        EventServices.GenericInstance.OnGameOver += GameOverRoutine;
        EventServices.GenericInstance.OnFlowUpdate += FlowUpdateRoutine;
        EventServices.GenericInstance.OnMoveUpdate += MoveUpdateRoutine;
        if(winScreen != null)
        {
            winScreen.SetActive(false);
        }
        if (finishScreen != null)
        {
            finishScreen.SetActive(false);
        }
        if (failScreen != null)
        {
            failScreen.SetActive(false);
        }
    }
    
    private void MoveUpdateRoutine(int currentMove)
    {
        move.text = "Move : " + currentMove.ToString();
    }

    private void FlowUpdateRoutine(int currentFlow)
    {
        flow.text = "Flows : " + currentFlow.ToString() + "/5";
    }

    private void GameOverRoutine()
    {
        if (failScreen != null)
        {
            failScreen.SetActive(true);
        }
    }

    private void WinRoutine()
    {
        if (LevelOverController.Instance.CurrentLevel < LevelOverController.TotalLevels)
        {
            if(winScreen != null)
            {
                winScreen.SetActive(true);
            }
        }
        else
        {
            if (finishScreen != null)
            {
                finishScreen.SetActive(true);
            }
        }
    }

    public void NextFunction()
    {
        LevelOverController.Instance.Next();
    }

    public void ToMainMenu()
    {
        LevelOverController.Instance.MainMenu();
    }
}
