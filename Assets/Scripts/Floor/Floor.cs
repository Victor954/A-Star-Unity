using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {
     
    public GameObject player;
    public GameObject marker;
    public LayerMask layer;

    public delegate void Test(List<Node> list);
    public event Test gg;

    private Astar path;

    // Use this for initialization
    void Start() {

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {

            //GameObject.FindWithTag("Finish").transform.position = transform.position;
            MovePlayer playerMove = player.GetComponent<MovePlayer>();
            Vector3 movePoint = DownClick(layer);

            if (movePoint != new Vector3(0.1f, 0.1f))
            {
                float x = Mathf.Round(player.transform.position.x);
                float y = Mathf.Round(player.transform.position.y);

                SetPlayerParams(x, y, playerMove, movePoint);

                //Событие для нарисования Gizmos
                gg(playerMove.path);
            }

        }
    }

    void SetPlayerParams(float x , float y, MovePlayer playerMove , Vector3 movePoint)
    {
          path = new Astar(new Vector3(x, y), movePoint);

        player.GetComponent<Path>().path = path.GetPath();
          playerMove.path = path.GetPath();
          marker.transform.position = movePoint;
          playerMove.index = 1;

    }

    //Объект клика
    Vector3 DownClick(LayerMask layer)
    {
        Vector3 WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 Origin = new Vector2(WorldPosition.x, WorldPosition.y);

        Vector3 result = new Vector3(0.1f, 0.1f);

        RaycastHit2D Hit = Physics2D.Raycast(Origin, Vector2.zero, 0f, layer.value);

        if (Hit.collider != null)
            result = Hit.collider.gameObject.transform.position;

        return result;
    }

}
