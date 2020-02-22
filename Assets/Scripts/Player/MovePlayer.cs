using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

    public Transform marker;
    public GameObject isClick;
    public GameObject StepObj;
    public int step = 1;

    public List<Node> path;
    public int index;

    public delegate void IsMove();
    public event IsMove MoveDone;

    public MoveToPoint movePlayer;

    bool MoveCome;
    PathController ph;

    // Use this for initialization
    void Start () {
        index = 1;

        movePlayer = new MoveToPoint();
        ph = new PathController(gameObject);

        gameObject.transform.GetComponentInParent<CheckPath>().OnTriggerObjects += delegate ()
        {
            Vector2 pos;
            IEnumerable<GameObject> boots;

            ph.GetOverlap(out pos, out boots);

            ph.GetValue(boots, pos);
        };

        isClick.GetComponent<Floor>().gg += delegate (List<Node> n)
        {
            MoveCome = true;
            movePlayer.pathStep = 1;
        };
    }

    //пропустить шаг 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponent<Path>().path = new List<Node>() { new Node(false, transform.position, 0, 0) };
            MoveDone();
        }
    }

    void FixedUpdate()
    {
        if (path != null && MoveCome)
        {
            movePlayer.MoveToPointByStep(transform, path, out MoveCome, path.Count - 1);

            if (movePlayer.GetResult())
            {
                MoveDone();
            }

        }

    }
}
