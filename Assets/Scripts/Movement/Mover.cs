using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Control;
using RPG.Combat;

namespace RPG.Movement {

	public class Mover : MonoBehaviour, IAction {

		private NavMeshAgent myNavAgent;
		private Vector3 velocity;
		private Animator myAnimator;
		private Health health;

		void Start ()
		{
			myNavAgent = GetComponent<NavMeshAgent> ();
			myAnimator = GetComponent<Animator> ();
			health = GetComponent<Health> ();
		}

		void Update ()
		{
			if (health.IsDead ()) {
				myNavAgent.enabled = false;
			}
			UpdateAnimator ();
		}

		public void StartMoveAction (Vector3 destination)
		{
			GetComponent<ActionScheduler> ().StartAction (this);
			MoveTo (destination);
		}

		public void MoveTo (Vector3 destination)
		{
			if (!myNavAgent.enabled) {
				return;
			}
			myNavAgent.destination = destination;
			myNavAgent.isStopped = false;
		}

		public void Cancel ()
		{
			if (!myNavAgent.enabled) {
				return;
			}
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
