using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

	private GameObject target;


	// Use this for initialization
	void Start ()
	{
		target = GameObject.FindGameObjectWithTag ("Player");

	}

	void LateUpdate ()
	{

		// Exit if no target
		if (!target) {
			return;
		}

		transform.position = target.transform.position;

	}
}
