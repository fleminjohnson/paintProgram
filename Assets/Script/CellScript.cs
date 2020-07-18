using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellScript : MonoBehaviour
{
    public static float Radius = 0.2f;

    [SerializeField]
    private CellRole role;

    private Image image;
    private Vector2Int coordinates;
    private Color defaultColor = Color.white;
    private Color endNodeColor;
    private Stack<Color> colorHistory = new Stack<Color>();

    void Awake()
    {
        image = GetComponent<Image>();
        if(image == null)
        {
            print("Image is null");
        }
        colorHistory.Push(defaultColor);
    }

    public Vector3 Pos { get { return transform.position; } }

    public Vector2Int Coordinates { set { coordinates = value; } }

    public CellRole Designation { get{ return role; } }

    public Color EndNodeColor {get{ return endNodeColor; } set { endNodeColor = value; }  }

    public cellStatus Status { get; private set; } = cellStatus.UnVisisted;

    public void ExposeUnVisited()
    {
        image.color = Color.green;
    }

    public void ChangeColor(Color cellColor)
    {
        colorHistory.Push(image.color);
        if(image == null)
        {
            print("Image is null");
        }
        image.color = cellColor;
        image.color /= 2;
    }

    public void PrevStatusAnalysis()
    {
        if (Status == cellStatus.UnVisisted)
        {
            Status = cellStatus.Visited;
            return;
        }
        Status = cellStatus.Intersection;
        PaintGM.Instance.ReportIntersection();
    }

    public void ButtonClick()
    {
        if(Status == cellStatus.UnVisisted & PaintGM.Instance.GetSessionStatus == SessionStatus.Idle)
        {
            PaintGM.Instance.SetColor(endNodeColor);
            PaintGM.Instance.EndNodeRoutine(coordinates);
        }
    }

    internal void ResetToDefault()
    {
        if(colorHistory.Count > 1)
        {
            image.color = colorHistory.Pop();
        }
        else
        {
            image.color = colorHistory.Peek();
        }

        if(Status == cellStatus.Intersection)
        {
            Status = cellStatus.Visited;
        }
        else
        {
            Status = cellStatus.UnVisisted;
        }
    }
}
