using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BootControll : MonoBehaviour {

    public List<Node> path;
    public GameObject stepObj;

    private Step step;
    public Vector3 point;

    PathController ph;

    // Use this for initialization

   void Start()
    {
        step = stepObj.GetComponent<Step>();
        point = new Vector3(7, 2);

        ph = new PathController(gameObject);

        gameObject.transform.GetComponentInParent<CheckPath>().OnTriggerObjects += delegate ()
        {
            Vector2 pos;
            IEnumerable<GameObject> boots;

            ph.GetOverlap(out pos, out boots);

            ph.GetValue(boots,pos);
        };

        //При поступлении шага
        step.StepDone += delegate ()
        {

            CheckPath.stop = false;

            Astar star = new Astar(transform.position, point);
            path = star.GetPath();

            gameObject.GetComponent<Path>().path = path;
        };
    }

	// Update is called once per frame
	void FixedUpdate () {

    }
}
