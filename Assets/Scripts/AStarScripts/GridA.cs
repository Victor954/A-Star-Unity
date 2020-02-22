using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridA : MonoBehaviour
{

    public int WidthGrid = 24;
    public int HeightGrid = 18;
    private Vector2 cellSize = new Vector2(1, 1);
    public Color32 cellColor = new Color32(255, 255, 255, 100);
    public Color32 cellColorTrigger = new Color32(238, 28, 36, 100);
    public LayerMask Wall;

    public GameObject player;
    public GameObject camera;

    private float drop = 2f;

    public Node[,] nodes;
    private Node Player;
    private Floor test;

    private List<Node> path;

    int CutWidth, CutHeight;


    // Use this for initialization
    void Start()
    {
        test = camera.GetComponent<Floor>();

        test.gg += delegate (List<Node> list)
        {
            path = list;
        };

        nodes = new Node[WidthGrid + 1, HeightGrid + 1];

        for (int y = HeightGrid / 2 * -1; y <= HeightGrid / 2; y++)
        {

            for (int x = WidthGrid / 2 * -1; x <= WidthGrid / 2; x++)
            {
                int indexY = y + HeightGrid / 2;
                int indexX = x + WidthGrid / 2;

                Vector3 pos = transform.position + new Vector3(x, y);
                bool isWall = Physics2D.OverlapCircle(pos, 0.4f, Wall);

                nodes[indexX, indexY] = new Node(isWall, pos, indexX, indexY);

                //Set start point and end point
                if (pos == player.transform.position)
                {
                    Player = nodes[indexX, indexY];
                    nodes[indexX, indexY].Gcost = 0;
                }

                indexX++;

            }
        }

        //Make A* finding

        if (Player != null)
        {
            Astar.Nodes = nodes;
        }
    }

    void OnDrawGizmosSelected()
    {
        CutWidth = WidthGrid / 2;
        CutHeight = HeightGrid / 2;

        for (float y = CutHeight * -1; y <= CutHeight; y += 2f / drop)
        {
            for (float x = CutWidth * -1; x <= CutWidth; x += 2f / drop)
            {
                Gizmos.DrawWireCube(gameObject.transform.position + new Vector3(x, y), cellSize);
            }
        }
    }


    void SetColor(bool isWall)
    {
        if (isWall)
            Gizmos.color = cellColorTrigger;
        else
            Gizmos.color = cellColor;
    }

    void OnDrawGizmos()
    {
        if (nodes != null)
        {
            //Make Gizmos grid
            foreach (Node n in nodes)
            {
                SetColor(n.isWall);

                Gizmos.DrawCube(n.pos, cellSize);
            }
        }

        if (path != null)
        {
            foreach (Node p in path)
            {
                Gizmos.color = new Color32(255, 0, 0, 200);
                Gizmos.DrawCube(p.pos, cellSize);
            }
        }
    }
}
