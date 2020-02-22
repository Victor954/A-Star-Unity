using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour {

    public GameObject player;
    private int CurrectStep;

    private int step;
    private MovePlayer move;

    public delegate void IsStep();
    public event IsStep StepDone;

    // Use this for initialization
    void Start () {
        step = 0;

        move = player.GetComponent<MovePlayer>();
        CurrectStep = move.step;

        move.MoveDone += delegate () 
        {
            step++;
        };

    }
	
	// Update is called once per frame
	void Update () {
        //Как только количество пройденных клеток равна значению шага - возращаем событие
        if (step == CurrectStep)
        {
            StepDone();
            step = 0;
        }
    }
}
