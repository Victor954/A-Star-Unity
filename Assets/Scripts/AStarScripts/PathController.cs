using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PathController
{
    public GameObject step;
    List<GameObject> bootList = new List<GameObject>();

    GameObject player;

    Vector2 overlapPos;
    IEnumerable<GameObject> objectsFind;

    public PathController(GameObject player)
    {
        this.player = player;
    }

    private void GetOverlap()
    {
        fillList(player);

        List<Node> pathPlayer = player.GetComponent<Path>().path;

        var cur = AnyPlayer(0, pathPlayer);

        int i = 0;

        do
        {
            cur = AnyPlayer(i, pathPlayer);

            if (i < pathPlayer.Count() - 1 && cur.Count() == 0)
                i++;
            else
                break;


        }
        while (cur.Count() == 0);

        if (cur.Count() != 0)
        {
            overlapPos = pathPlayer[i].pos;
            objectsFind = cur;
        }
    }

    IEnumerable<GameObject> AnyPlayer(int i , List<Node> pathPlayer)
    {
        return bootList.Where(s => (s.GetComponent<Path>().path).Any(g => g.pos == pathPlayer[i].pos));
    }

    public void GetValue(IEnumerable<GameObject> boots , Vector2 pos)
    {
        if (boots != null)
        {
            Debug.Log(pos);
        }
    }

    public void GetOverlap(out Vector2 overlapPos, out IEnumerable<GameObject> objectsFind)
    {
        GetOverlap();

        overlapPos = this.overlapPos;
        objectsFind = this.objectsFind;
    }

    void fillList(GameObject player)
    {
        List<GameObject> objBoot = new List<GameObject>();
        Transform pi = GameObject.FindGameObjectWithTag("Count").transform;

        for (int i = 0; i <= pi.childCount - 1; i++)
        {
            if (pi.GetChild(i).gameObject != player)
            {
                objBoot.Add(pi.GetChild(i).gameObject);
            }
        }

        bootList.Clear();
        bootList.AddRange(objBoot);
    }
}

