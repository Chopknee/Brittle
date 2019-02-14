using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveEnemy : MonoBehaviour {
    public Transform target;
    public Transform hive;
    public float chaseRange;
    public float hiveRange;
    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        float distanceToHive = Vector3.Distance(transform.position, hive.position);
        if(distanceToTarget < chaseRange && distanceToHive < hiveRange)
        {

            Vector3 targetDir = target.position - transform.position;
            float angleOne = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg -90f;
            Quaternion r = Quaternion.AngleAxis(angleOne, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, r, 360);

            transform.Translate(Vector3.up * Time.deltaTime * speed);
        }

        else
        {
            Vector3 hiveLocationDir = hive.position - transform.position;
            float angle = Mathf.Atan2(hiveLocationDir.y, hiveLocationDir.x) * Mathf.Rad2Deg - 90f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 360);

            transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
    }
}
