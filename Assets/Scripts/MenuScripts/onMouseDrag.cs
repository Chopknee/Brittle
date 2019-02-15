using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onMouseDrag : MonoBehaviour {

	Vector3 distance;

	void OnMouseDown()
	{
		distance = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,Camera.main.WorldToScreenPoint(transform.position).z)) - transform.position;
	}


	void OnMouseDrag ()
	{
		Vector3 distance_to_screen = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen.z ));
		transform.position = new Vector3( pos_move.x - distance.x , pos_move.y -distance.y , pos_move.z -distance.z );

	}
}
