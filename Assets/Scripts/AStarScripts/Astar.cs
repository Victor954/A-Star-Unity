using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Astar : IEnumerable {

    private Node Player, Marker;
    private List<Node> OpenList = new List<Node>();
    private List<Node> CloseList = new List<Node>();
    private List<Node> Path = new List<Node>();


    public static Node[,] Nodes;

    private Node parent;

    enum Radius : int
    {
        IsRoud = 1,
        IsSide = 2,
        IsAngle = 3
    }

    public Astar(Vector3 PlayerO, Vector3 MarkerO)
    {
        //родительская нода по умолчанию начинается с позиции игрока

        SetPath(PlayerO, MarkerO);

    }

    //Установление пути
    public void SetPath(Vector3 PlayerO, Vector3 MarkerO)
    {
        if (Nodes != null)
        {
            Player = FindPoint(PlayerO);
            Marker = FindPoint(MarkerO);

            parent = Player;

            FillCloseList(Nodes);

            FindPath(Nodes);

            SetPath();

        }
    }

    //Нахождение точек в нодах
    Node FindPoint(Vector3 obj)
    {
        Node result = null;

        foreach (Node n in Nodes)
            if (new Vector2(obj.x, obj.y) == n.pos)
                result = n;

        return result;
    }

    //Находим путь
    void FindPath(Node[,] nodes)
    {
        while (parent.pos != Marker.pos)
        {

            SelectOpenNode(nodes);

            if (GetParentNode() != null)
                parent = GetParentNode();
            else
                break;

            if (parent.pos == Marker.pos)
                SelectOpenNode(nodes);
        }
    }
    //Возращаем путь
    public IEnumerator GetEnumerator()
    {
        foreach (Node n in Path)
            yield return n;
    }

    public List<Node> GetPath()
    {
        return Path;
    }

    //Заполяем путь в лист
    void SetPath ()
    {
        if (GetFirstFcost() != null)
        {
            Node Start = CloseList.Find(start => start.pos == Marker.pos);

            Path.Add(Start);

            for (int i = 0; i <= Path.Count; i++)
            {
                if (Path[i].pos != Player.pos)
                {
                    Path.Add(Path[i].parent);
                }
                else
                    break;

            }
            Path.Reverse();
        }
        else
            Path = null;
    }

    //Заполнить закрытый лист нодами стен
    void FillCloseList(Node[,] nodes)
    {
        foreach (Node n in nodes)
        {
            if (n.isWall)
                CloseList.Add(n);
        }
    }

    //Выбор наименьшего Fcost среди нод
    Node GetParentNode()
    {
        Node Key = GetFirstFcost();

        foreach (Node n in OpenList)
        {
           if (Key.Fcost > n.Fcost)
              Key = n;
        }

        return Key;
    }

    Node GetFirstFcost()
    {
        return (OpenList.Count != 0) ? OpenList[0] : null;
    }

    void LengthFromParent(out float x, out float y , Node start , Node end)
    {
        x = Mathf.Abs((start.pos - end.pos).x);
        y = Mathf.Abs((start.pos - end.pos).y);
    }

    Radius GetRadius(Node n)
    {
        float x, y;
        LengthFromParent(out x,out y,parent,n);

        Radius result = 0;

        if (x == 0 && y == 1 || x == 1 && y == 0)
            result = Radius.IsSide;
        if (x == 1 && y == 1)
            result = Radius.IsRoud;

        return result;
    }

    Radius GetRadius(Node n,Node n2)
    {
        float x, y;
        LengthFromParent(out x, out y, n, n2);

        return (x == 1 && y == 0 || x == 0 && y == 1) ? Radius.IsAngle : 0;
    }

    void SelectOpenNode(Node[,] nodes)
    {
        //Перебераем ноды и подбираем диапазон для них
        foreach (Node n in nodes)
        {
            //Добавление ключевой ноды в закрытый список
            AddCoseList(n);

            if (GetRadius(n) == Radius.IsSide)
                SetCost(n, 10);

            if (GetRadius(n) == Radius.IsRoud)
                SetCost(n, 14);
            
        }
    }
    //Выставить Gcost и родительскую
    void SetValueGandP(Node n, int Gcost)
    {
        n.Gcost = parent.Gcost + Gcost;
        n.parent = parent;
    }

    //Расчитываем диапозон
    void SetCost(Node n, int Gcost)
    {

        //Если ноды нет ни в одном из списокв то выставляем значения 
        if (!IsList(n, CloseList) && !IsList(n, OpenList) && !DetCut(n, Gcost))
        {

            SetValueGandP(n,Gcost);
            n.Hcost = (Mathf.Abs((n.pos - Marker.pos).x) + Mathf.Abs(n.pos.y - Marker.pos.y)) * 10;

            OpenList.Add(n);
        }
        //если есть в открытом списке то проверяем значения
        else if (IsList(n, OpenList))
        {
            float CurrectG = parent.Gcost + GetStep(n);
            float NewG = n.Gcost;

            if (NewG > CurrectG)
                SetValueGandP(n, Gcost);
        }

    }

    float GetStep(Node n)
    {
        return (GetRadius(n) == Radius.IsSide) ? 10 : 14;
    }

    //Добавление ключевой ноды в закрытый список
    void AddCoseList(Node n)
    {
        OpenList.RemoveAll(s => s.pos == parent.pos);

        if (n.pos == parent.pos)
            CloseList.Add(n);
    }

    //Проверить значения ноды со значениями нод в списке
    bool IsList(Node n, List<Node> list)
    {
        return list.Any(s => s.pos == n.pos);
    }

    bool FindAngle(Node n , Node t)
    {
        return GetRadius(n, parent) == Radius.IsAngle && GetRadius(n, t) == Radius.IsAngle && n.isWall;
    }

   //Запретить подрезание
    bool DetCut(Node t, int Gcost)
    {
        return (Gcost == 14) ? CloseList.Any(s => FindAngle(s,t)) : false;
    }
}
