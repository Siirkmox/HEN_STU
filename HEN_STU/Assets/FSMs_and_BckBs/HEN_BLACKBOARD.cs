using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEN_BLACKBOARD : MonoBehaviour
{

    public float wormDetectableRadius = 60; // within this radius worms are detected
    public float wormReachedRadius = 12;    // at this distace worm is eatable
    public float timeToEatWorm = 1.5f;      // it takes this time to eat a worm
    public float chickDetectionRadius = 100;   // within this radius chicks are detected
    public float chickFarEnoughRadius = 250;   // from this distance on chicks stop being an annoyance
    public GameObject attractor;     // hen wanders arounf this point
    public GameObject worm;          // worm to eat
    public AudioClip angrySound;
    public AudioClip eatingSound;
    public AudioClip cluckingSound;
    
    void Awake()
    { 
        if (attractor == null) {
			attractor = GameObject.Find ("Attractor");
			if (attractor == null) {
				Debug.LogError ("no ATTRACTOR object found in "+this);
			}
		}

        if (angrySound == null) {
			angrySound = Resources.Load<AudioClip>("AngryChicken");
			if (angrySound == null) {
				Debug.LogError ("no ANGRYSOUND audioClip found in "+this);
			}
		}

        if (eatingSound == null) {
			eatingSound = Resources.Load<AudioClip>("Chew");
			if (eatingSound == null) {
				Debug.LogError ("no EATINGSOUND audioClip found in "+this);
			}
		}

        if (cluckingSound == null) {
			cluckingSound = Resources.Load<AudioClip>("ChickenClucking");
			if (cluckingSound == null) {
				Debug.LogError ("no CLUCKINGSOUND audioClip found in "+this);
			}
		}
    }
}
