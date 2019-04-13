using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgBuildings : MonoBehaviour {

    //The horizontal start and end positions
    public Vector3 bgPositionStart;
    public Vector3 bgPositionEnd;

    public Vector3 worldPosParalaxStart;
    public Vector3 worldPosParalaxEnd;

    public bool drawGizmos = false;

    //private Vector3 startPos;
    private GameObject mainCamera;
    private Vector3 worldRange;

    private static Color wdStartCol = new Color(1, 0, 1);
    private static Color wdEndCol = new Color(1, 1, 0);
    private static Color bgStartCol = new Color(1, 0, 0);
    private static Color bgEndCol = new Color(0, 1, 1);

    public Vector3 through;

    //private GameObject backdrop;

    private Vector3 originalbgPositionStart;
    private Vector3 originalbgPositionEnd;
    // Use this for initialization
    void Start () {
        through = new Vector3();
        //startPos = transform.position;

        mainCamera = LevelControl.Instance.MainCamera;
        worldRange = worldPosParalaxStart - worldPosParalaxEnd;
        bgPositionStart = bgPositionStart + transform.localPosition;
        bgPositionEnd = bgPositionEnd + transform.localPosition;

        //backdrop = transform.parent.gameObject;

        originalbgPositionStart = bgPositionStart;
        originalbgPositionEnd = bgPositionEnd;
    }
	
	// Update is called once per frame
	void Update () {
        bgPositionEnd = Vector3.Scale(originalbgPositionEnd, transform.parent.localScale);
        bgPositionStart = Vector3.Scale(originalbgPositionStart, transform.parent.localScale);

        through = mainCamera.transform.position - worldPosParalaxEnd;
        //through.x = through.x / worldRange.x;
        through.x = ( worldRange.x == 0 ) ? 0 : through.x / worldRange.x;
        through.y = ( worldRange.y == 0 ) ? 0 : through.y / worldRange.y;
        through.z = ( worldRange.z == 0 ) ? 0 : through.z / worldRange.z;
        
        
        transform.localPosition = Lerp3(bgPositionStart, bgPositionEnd, through);
    }


    private Vector3 Lerp3(Vector3 start, Vector3 end, Vector3 t) {
        Vector3 res = new Vector3();
        res.x = Mathf.Lerp(start.x, end.x, t.x);
        res.y = Mathf.Lerp(start.y, end.y, t.y);
        res.z = Mathf.Lerp(start.z, end.z, t.z);
        return res;
    }

    private Vector3 Abs3(Vector3 vect) {
        return new Vector3(Mathf.Abs(vect.x), Mathf.Abs(vect.y), Mathf.Abs(vect.z));
    }

    public void OnDrawGizmos () {
        if (drawGizmos) {
            Gizmos.color = wdStartCol;
            Gizmos.DrawSphere(worldPosParalaxStart, 1);
            Gizmos.color = wdEndCol;
            Gizmos.DrawSphere(worldPosParalaxEnd, 1);
            Gizmos.color = bgStartCol;
            Gizmos.DrawSphere(transform.position + bgPositionStart, 1);
            Gizmos.color = bgEndCol;
            Gizmos.DrawSphere(transform.position + bgPositionEnd, 1);
        }
    }
}
