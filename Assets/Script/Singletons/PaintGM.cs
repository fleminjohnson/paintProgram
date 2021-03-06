﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PaintGM : SingletonBehaviour<PaintGM>
{
    [SerializeField]
    private Transform board;

    private int gridSize = 5;
    private int totalPathCount = 5;
    private int initial = 0;
    private int actualValue = 0;
    private CellScript[,] cell;
    private Vector2Int prevIndices;
    private Stack<PathScript> pathStack;
    private Stack<CellScript> nodeStack;
    private Color penColor;
    private PathStatus pathStatus = PathStatus.Nonintersected;

    public SessionStatus GetSessionStatus { get; private set; } = SessionStatus.Idle;

    protected override void Awake()
    {
        base.Awake();

        cell = new CellScript[gridSize,gridSize];
        pathStack = new Stack<PathScript>(totalPathCount);
        nodeStack = new Stack<CellScript>(gridSize);
    }

    void Start()
    {
        FormGrid();
    }

    void Update()
    {
        switch (GetSessionStatus)
        {
            case SessionStatus.Started:
                TrackMidCells();
                break;
            case SessionStatus.Finished:
                FinishedRoutine();
                break;
            case SessionStatus.Suspended:
                SuspendedRoutine();
                break;
        }
    }

    public void ReportIntersection()
    {
        pathStatus = PathStatus.Intersected;
    }

    private void FinishedRoutine()
    {
        if(pathStatus == PathStatus.Intersected)
        {
            OneStepBack();
            for(; nodeStack.Peek().Designation != CellRole.EndNode; )
            {
                OneStepBack();
            }
            OneStepBack();
            pathStatus = PathStatus.Nonintersected;
        }
        GetSessionStatus = SessionStatus.Idle;
        EventServices.GenericInstance.InvokeOnFlowUpdate(pathStack.Count);
        WinCondition();
    }

    private void WinCondition()
    {
        if(pathStack.Count != totalPathCount )
        {
            return;
        }
        if(nodeStack.Count != board.childCount)
        {
            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    if(cell[i,j].Status == cellStatus.UnVisisted)
                    {
                        cell[i, j].ExposeUnVisited();
                    }
                }
            }
            EventServices.GenericInstance.InvokeOnGameOver();
        }
        else
        {
            EventServices.GenericInstance.InvokeOnWin();
        }
    }

    private void SuspendedRoutine()
    {
        if (pathStatus == PathStatus.Intersected)
        {
            GetSessionStatus = SessionStatus.Finished;
            return;
        }
        if (DistanceCheck(nodeStack.Peek()))
        {
            pathStack.Peek().ToNextNode();
            pathStack.Peek().IsReadytoFollow = true;
            GetSessionStatus = SessionStatus.Started;
        }
    }

    public void EndNodeRoutine(Vector2Int coordinates)
    {
        AccessCell(coordinates);
        initial = nodeStack.Count;
        GetSessionStatus = SessionStatus.Started;
        PathScript head = PathService.Instance.GeneratePath(cell[coordinates.x, coordinates.y].Pos);
        if (pathStack.Count != 0)
        {
            pathStack.Peek().enabled = false;
        }
        pathStack.Push(head);
        pathStack.Peek().Attach(cell[coordinates.x, coordinates.y]);
        pathStack.Peek().SetPathColor(penColor);
    }

    public void SetColor(Color color)
    {
        penColor = color;
    }

    public void OneStepBack()
    {
        if(nodeStack.Count < 1)
        {
            return;
        }
        if (nodeStack.Peek().Designation == CellRole.EndNode & pathStack.Peek().CurrentIndex == 1)
        {
            Destroy(pathStack.Pop().gameObject); 
        }
        else
        {
            pathStack.Peek().Dettach();
            GetSessionStatus = SessionStatus.Suspended;
        }
        nodeStack.Peek().ResetToDefault();
        nodeStack.Pop();
    }

    private void FormGrid()
    {
        int y = -1;
        for (int x = 0; x < board.childCount; x++)
        {
            if (x % gridSize == 0)
            {
                y++;
            }
            cell[y, x % gridSize] = board.GetChild(x).GetComponent<CellScript>();
            cell[y, x % gridSize].Coordinates = new Vector2Int(y, x % gridSize);
        }
    }

    private void TrackMidCells()
    {
        int criticalValue = 1;

        for (int i = -1; i <= criticalValue; i++)
        {
            for (int j = -1; j <= criticalValue; j++)
            {
                PathPossibleAt(i, j);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (pathStack.Peek().CurrentIndex > 2)
            {
                pathStack.Peek().Dettach();
                GetSessionStatus = SessionStatus.Suspended;
            }
            else
            {
                OneStepBack();
                pathStack.Pop().DestroyPath();
                GetSessionStatus = SessionStatus.Idle;
            }
        }

    }

    private void AccessCell(Vector2Int coordinates)
    {
        if(cell[coordinates.x, coordinates.y].GetComponent<Image>() == null)
        {
            print("Image inside is null");
        }
        cell[coordinates.x, coordinates.y].ChangeColor(penColor);
        nodeStack.Push(cell[coordinates.x, coordinates.y]);
        cell[coordinates.x, coordinates.y].PrevStatusAnalysis();
        prevIndices = coordinates;
    }

    private void PathPossibleAt(int xIndex , int yIndex)
    {
        Vector2Int currentIndices = new Vector2Int(prevIndices.x + xIndex, prevIndices.y + yIndex);
        currentIndices.x = math.clamp(currentIndices.x, 0, (gridSize - 1));
        currentIndices.y = math.clamp(currentIndices.y, 0, (gridSize - 1));

        if (NotDiagnolAt(ref currentIndices))
        {
            DistanceCheck(currentIndices);
        }
    }

    private bool NotDiagnolAt(ref Vector2Int currentIndices)
    {
        return math.abs(currentIndices.x - prevIndices.x) == 1 & math.abs(currentIndices.y - prevIndices.y) == 0 ||
                    math.abs(currentIndices.x - prevIndices.x) == 0 & math.abs(currentIndices.y - prevIndices.y) == 1;
    }

    private void DistanceCheck(Vector2Int currentIndices)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(cell[currentIndices.x, currentIndices.y].Pos, mousePos) < CellScript.Radius)
        {
            if(cell[currentIndices.x, currentIndices.y].Designation == CellRole.EndNode & cell[currentIndices.x, currentIndices.y].EndNodeColor != penColor)
            {
                return;
            }
            AccessCell(new Vector2Int(currentIndices.x, currentIndices.y));
            pathStack.Peek().Attach(cell[currentIndices.x, currentIndices.y]);
            prevIndices = currentIndices;
            if (cell[currentIndices.x, currentIndices.y].Designation == CellRole.EndNode)
            {
                GetSessionStatus = SessionStatus.Finished;
            }
            actualValue = nodeStack.Count - initial;
            EventServices.GenericInstance.InvokeOnMoveUpdate(actualValue);
        }
    }

    private bool DistanceCheck(CellScript cell)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) & Vector2.Distance(cell.Pos, mousePos) < CellScript.Radius)
        {
            return true;
        }
        return false;
    }
}
