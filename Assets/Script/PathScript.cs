﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour
{
    private LineRenderer path;
    private int currentNode = 0;
    private bool isReadyToFollow = false;
      
    void Awake()
    {
        path = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (isReadyToFollow & PaintGM.Instance.GetSessionStatus)
        {
            Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            path.SetPosition(currentNode, Pos);
        }
    }

    public void Stick(CellScript cell)
    {
        path.SetPosition(currentNode, cell.Pos);

        if (cell.Designation == CellRole.EndNode)
        {
            isReadyToFollow = !isReadyToFollow;
            if(isReadyToFollow)
            {
                ToNextNode();
                path.SetPosition(currentNode, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
        else
        {
            ToNextNode();
        }
    }

    private void ToNextNode()
    {
        path.positionCount++;
        currentNode++;
    }
}