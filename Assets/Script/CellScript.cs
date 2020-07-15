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
    private Image childImage;
    private Vector2Int coordinates;
    private Color endNodeColor;

    void Awake()
    {
        image = GetComponent<Image>();
        if(transform.childCount > 0)
        {
            childImage = transform.GetChild(0).GetComponent<Image>();
        }
    }

    void Start()
    {
        if(role == CellRole.EndNode & childImage != null)
        {
            endNodeColor = childImage.color;
        }
    }

    public Vector3 Pos { get { return transform.position; } }

    public Vector2Int Coordinates { set { coordinates = value; } }

    public CellRole Designation { get{ return role; } }

    public void ChangeColor(Color cellColor)
    {
        image.color = cellColor;
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
    }
}
