using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement {

	public class Mover : MonoBehaviour, IAction {

		private NavMeshAgent myNavAgent;
		private Vector3 velocity;
		private Animator myAnimator;

		void Start ()
		{
			myNavAgent = GetComponent<NavMeshAgent> ();
			myAnimator = GetComponent<Animator> ();

		}

		void Update ()
		{
			UpdateAnimator ();
		}

		public void StartMoveAction (Vector3 destination)
		{
			GetComponent<ActionScheduler> ().StartAction (this);
			MoveTo (destination);
		}

		public void MoveTo (Vector3 destination)
		{
			myNavAgent.destination = destination;
			myNavAgent.isStopped = false;
		}

		public void Cancel ()
		{
			myNavAgent.isStopped = true;
		}

		private void UpdateAnimator ()
		{
			velocity = myNavAgent.velocity;
			Vector3 localVelocity = transform.InverseTransformDirection (velocity);
			float speed = localVelocity.z;
			myAnimator.SetFloat ("ForwardSpeed", speed);
		}

	}
}
