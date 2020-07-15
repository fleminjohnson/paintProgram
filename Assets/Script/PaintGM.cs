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
    private KeyCode mouseLeft;
    private CellScript[,] cell;
    private Vector2Int prevIndices;
    private Stack<PathScript> pathStack;
    private Stack<CellScript> nodeSequence;
    private PathScript path;

    public bool GetSessionStatus { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();

        cell = new CellScript[gridSize,gridSize];
        pathStack = new Stack<PathScript>(totalPathCount);
        nodeSequence = new Stack<CellScript>(gridSize);
        mouseLeft = KeyCode.Mouse0;
    }

    void Start()
    {
        FormGrid();
    }

    void Update()
    {
        TrackMidCells();
    }

    public void EndNodeRoutine(Vector2Int coordinates)
    {
        AccessCell(coordinates);
        GetSessionStatus = !GetSessionStatus;
        if (GetSessionStatus)
        {
            PathScript head = PathService.Instance.GeneratePath(cell[coordinates.x, coordinates.y].Pos);
            if(pathStack.Count != 0)
            {
                pathStack.Peek().enabled = false;
            }
            pathStack.Push(head);
            pathStack.Peek().Stick(cell[coordinates.x, coordinates.y]);
        }
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
            print(cell[y, x % gridSize].Pos);
        }
    }

    private void TrackMidCells()
    {
        if (!GetSessionStatus)
        {
            return;
        }

        int criticalValue = 1;

        for (int i = -1; i <= criticalValue; i++)
        {
            for (int j = -1; j <= criticalValue; j++)
            {
                PathPossibleAt(i, j);
            }
        }
    }

    private void AccessCell(Vector2Int coordinates)
    {
        cell[coordinates.x, coordinates.y].ChangeColor(Color.black);
        nodeSequence.Push(cell[coordinates.x, coordinates.y]);
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
            AccessCell(new Vector2Int(currentIndices.x, currentIndices.y));
            pathStack.Peek().Stick(cell[currentIndices.x, currentIndices.y]);
            prevIndices = currentIndices;
            if (cell[currentIndices.x, currentIndices.y].Designation == CellRole.EndNode)
            {
                GetSessionStatus = !GetSessionStatus;
            }
        }
    }
}
