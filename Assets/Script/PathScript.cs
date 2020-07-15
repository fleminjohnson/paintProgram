using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour
{
    private LineRenderer path;
    private int currentNode = 0;

    public bool IsReadytoFollow { get; set; } = false;

    public int CurrentIndex { get => path.positionCount; }

    void Awake()
    {
        path = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (IsReadytoFollow & PaintGM.Instance.GetSessionStatus == SessionStatus.Started)
        {
            Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            path.SetPosition(currentNode, Pos);
        }
        else
        {
            IsReadytoFollow = false;
        }
    }

    public void Attach(CellScript cell)
    {
        path.SetPosition(currentNode, cell.Pos);

        if (cell.Designation == CellRole.EndNode)
        {
            IsReadytoFollow = !IsReadytoFollow;
            if(IsReadytoFollow)
            {
                currentNode++;
                ToNextNode();
            }
        }
        else
        {
            currentNode++;
            ToNextNode();
        }
    }

    public void DestroyPath()
    {
        Destroy(gameObject);
    }

    public void Dettach()
    {
        path.positionCount--;
    }

    public void SetPathColor(Color color)
    {
        path.startColor = color;
        path.endColor = color;
    }

    public void ToNextNode()
    {
        path.positionCount++;
        path.SetPosition(currentNode, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
