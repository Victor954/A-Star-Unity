using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoint {


    public float speed = 5;
    public Transform player;
    //public List<Node> path;

    private bool globalResult;
    public int pathStep;

    //Направление относительно маркера
    float GetAsyc(float i)
    {
        float result = 1;
        if (i > 0)
            result = -1;
        if (i == 0)
            result = 0;

        return result;
    }

    //Выполнение движения 
    public bool GoToPoint(Transform player, Vector2 marker)
    {
        //Узнаем расстояние до точки
        Vector3 longX = new Vector3(player.position.x - marker.x, 0);
        Vector3 longY = new Vector3(0, player.position.y - marker.y);
        Vector3 LogR = longX + longY;

        bool result = false;

        //Пака позиция по x и y не равна 0 - двигаем объект
        if (longY != Vector3.zero)
            player.Translate(new Vector3(0, GetAsyc(longY.y)) * speed * Time.deltaTime);
        else
            player.position = new Vector3(player.position.x, marker.y);

        if (longX != Vector3.zero)
            player.Translate(new Vector3(GetAsyc(longX.x), 0) * speed * Time.deltaTime);
        else
            player.position = new Vector3(marker.x, player.position.y);

        if (LogR == Vector3.zero)
            result = true;

        globalResult = result;

        return result;
    }
    
    public bool GetResult()
    {
        return globalResult;
    }

    public void Axis(int PathCount , out bool MoveCome)
    {
        MoveCome = true;

        if (pathStep != PathCount)
            pathStep++;
        else
            MoveCome = false;
    }

    public void MoveToPointByStep(Transform player,List<Node> path , out bool MoveCome,int Length)
    {
        MoveCome = !GoToPoint(player, path[pathStep].pos);

        if (!MoveCome)
        {
            Axis(Length, out MoveCome);
        }
    }

}
