using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : MonoBehaviour {
    public float verticleCeiling = 100;
    public ParticleSystem[] blizzardEmitters;

	void Update () {
	    foreach (var particleSystem in blizzardEmitters)
	    {
            Ray ray = new Ray(particleSystem.transform.position + Vector3.up * verticleCeiling, Vector3.down);
            RaycastHit hit;
	        if (Physics.Raycast(ray, out hit, verticleCeiling))
	        {
                particleSystem.enableEmission = false;
	        }
	        else
	        {
                particleSystem.enableEmission = true;
	        }
	    }
	}
}
