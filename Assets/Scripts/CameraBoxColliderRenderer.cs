using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoxColliderRenderer : MonoBehaviour {
    //public static Color 
    private void OnDrawGizmos () {
        if (LevelControl.showCameraColliders) {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = rotationMatrix;
            Gizmos.color = LevelControl.cameraColliderColor;//col;
            BoxCollider2D bc = GetComponent<BoxCollider2D>();
            Gizmos.DrawWireCube((Vector3) bc.offset, bc.size);
        }
    }
}
