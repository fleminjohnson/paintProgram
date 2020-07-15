﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellScript : MonoBehaviour
{
    public static float Radius = 0.2f;

    [SerializeField]
    private CellRole role;

    private cellStatus status = cellStatus.UnVisisted;
    private Image image;
    private Vector2Int coordinates;
    private Color defaultColor = Color.white;
    private Color endNodeColor;
    private Stack<Color> colorHistory = new Stack<Color>();

    void Awake()
    {
        image = GetComponent<Image>();
        colorHistory.Push(defaultColor);
    }

    public Vector3 Pos { get { return transform.position; } }

    public Vector2Int Coordinates { set { coordinates = value; } }

    public CellRole Designation { get{ return role; } }

    public Color EndNodeColor { set { endNodeColor = value; } }

    public void ChangeColor(Color cellColor)
    {
        colorHistory.Push(image.color);
        image.color = cellColor;
        image.color = image.color / 2;
    }

    public void PrevStatusAnalysis()
    {
        if (status == cellStatus.UnVisisted)
        {
            status = cellStatus.Visited;
            return;
        }
        status = cellStatus.Intersection;
        PaintGM.Instance.ReportIntersection();
    }

    public void ButtonClick()
    {
        PaintGM.Instance.SetColor(endNodeColor);
        PaintGM.Instance.EndNodeRoutine(coordinates);
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

        if(status == cellStatus.Intersection)
        {
            status = cellStatus.Visited;
            return;
        }
        else
        {
            status = cellStatus.UnVisisted;
        }
    }
}
