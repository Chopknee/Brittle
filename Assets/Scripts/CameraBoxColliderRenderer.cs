using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoxColliderRenderer : MonoBehaviour {
    //public static Color 
    public Color col = new Color(0, 0.5f, 1, 0.75f);
    public bool show = false;
    private void OnDrawGizmos () {
        if (show) {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = rotationMatrix;
            Gizmos.color = col;
            BoxCollider2D bc = GetComponent<BoxCollider2D>();
            Gizmos.DrawWireCube((Vector3) bc.offset, bc.size);
        }
    }
}
