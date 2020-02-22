using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public Vector2 pos;
    public bool isWall;

    public int indexX;
    public int indexY;

    public Node parent;

    public float Hcost;
    public float Gcost;
    public float Fcost { get { return Hcost + Gcost; } }

    public Node(bool iswall,Vector2 pos, int indexX, int indexY) : this (iswall,pos.x,pos.y,indexX,indexY) { }
    public Node(bool isWall, float x , float y,int indexX,int indexY)
    {
        pos = new Vector2(x, y);
        this.isWall = isWall;
    }
}
