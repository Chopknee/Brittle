using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPause : MonoBehaviour, IPausable {

    ParticleSystem ps;

    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPause() {
        //
        ps.Pause();
    }

    public void OnUnPause() {
        //
        ps.Play();
    }
}
