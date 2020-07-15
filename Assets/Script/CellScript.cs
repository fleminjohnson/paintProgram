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
    private Color endNodeColor;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        print(endNodeColor.ToString());
    }

    public Vector3 Pos { get { return transform.position; } }

    public Vector2Int Coordinates { set { coordinates = value; } }

    public CellRole Designation { get{ return role; } }

    public Color EndNodeColor { set { endNodeColor = value; } }

    public void ChangeColor(Color cellColor)
    {
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

    }

    public void ButtonClick()
    {
        PaintGM.Instance.EndNodeRoutine(coordinates);
        PaintGM.Instance.SetColor(endNodeColor);
        ChangeColor(endNodeColor);
    }
}
