using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour {

	private Vector3 destination;
	private NavMeshAgent myNavAgent;
	private Vector3 velocity;
	private Animator myAnimator;

	void Start ()
	{
		myNavAgent = GetComponent<NavMeshAgent> ();
		destination = myNavAgent.destination;
		myAnimator = GetComponent<Animator> ();

	}

	void Update ()
	{

		if (Input.GetMouseButton (0)) {

			MoveToCursor ();
		}
		UpdateAnimator ();
	}

	private void MoveToCursor ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		bool hasHit = Physics.Raycast (ray, out RaycastHit hit);

		if (hasHit && Vector3.Distance (destination, hit.point) >= myNavAgent.stoppingDistance) {
			destination = hit.point;
			myNavAgent.destination = destination;
		}
	}

	private void UpdateAnimator ()
	{
		velocity = myNavAgent.velocity;
		Vector3 localVelocity = transform.InverseTransformDirection (velocity);
		float speed = localVelocity.z;
		myAnimator.SetFloat ("ForwardSpeed", speed);
	}
}
