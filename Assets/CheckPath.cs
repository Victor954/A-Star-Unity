using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckPath : MonoBehaviour {

    static public bool stop = false;

    public delegate void VisibleObjects();
    public event VisibleObjects OnTriggerObjects;

    // Update is called once per frame
    void Update () {

        if (!stop)
        {
            Transform pi = gameObject.transform;

            int i = 0;
            do
            {

                if (i == pi.childCount - 1 && pi.GetChild(i).gameObject.GetComponent<Path>().path.Count != 0)
                {
                    OnTriggerObjects();
                    stop = true;
                }

                i++;
            }
            while (i <= pi.childCount -1);
        }
    }
}
