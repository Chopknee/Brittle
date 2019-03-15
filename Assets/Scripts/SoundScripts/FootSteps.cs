using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour {

    KieselControl keiselController;
    //Keisel_AnimationController Keisel_AnimationController;
    AudioSource audioSource;
    Rigidbody2D rb;


	// Use this for initialization
	void Start () {
        keiselController = GetComponent<KieselControl>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        //Keisel_AnimationController = GetComponent<Keisel_AnimationController>();
	}
	
	// Update is called once per frame
	void Update () {

        if (keiselController.grounded == true && Mathf.Abs(rb.velocity.x) > 2 && audioSource.isPlaying == false && keiselController.running)
        {
            audioSource.volume = Random.Range(0.4f, 0.5f);
            audioSource.pitch = Random.Range(0.95f, 1.0f);
            audioSource.Play();
        }
	}
}
