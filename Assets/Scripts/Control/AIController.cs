using RPG.Combat;
using RPG.Core;
using UnityEngine;
using RPG.Movement;

namespace RPG.Control {

	public class AIController : MonoBehaviour {

		[SerializeField] float chaseDistance = 5f;
		[SerializeField] float suspicisionTime = 3f;
		[SerializeField] PatrolPath patrolPath;
		[SerializeField] float waypointTollerance = 1f;
		[SerializeField] float waypointDwellTime = 2f;

		Fighter fighter;
		Health health;
		GameObject player;
		Mover mover;

		Vector3 guardPosition;
		float timeSinceLastSawPlayer = Mathf.Infinity;
		float timeSinceArriveAtWaypoint = Mathf.Infinity;
		int currentWaypointIndex = 0;

		private void OnDrawGizmosSelected ()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere (transform.position, chaseDistance);
		}

		private void Start ()
		{
			fighter = GetComponent<Fighter> ();
			player = GameObject.FindWithTag ("Player");
			health = GetComponent<Health> ();
			mover = GetComponent<Mover> ();

			guardPosition = transform.position;
		}

		// Update is called once per frame
		void Update ()
		{
			if (health.IsDead ()) {
				return;
			}

			if (InAttackRange () && fighter.CanAttack (player)) {

				AttackBehaviour ();

			} else if (timeSinceLastSawPlayer < suspicisionTime) {
				SuspicionBehaviour ();

			} else {
				PatrolBehaviour ();
			}

			UpdateTimers ();
		}

		private void UpdateTimers ()
		{
			timeSinceLastSawPlayer += Time.deltaTime;
			timeSinceArriveAtWaypoint += Time.deltaTime;
		}

		private void PatrolBehaviour ()
		{
			Vector3 nextPosition = guardPosition;
			if (patrolPath != null) {
				if (AtWaypoint ()) {

					timeSinceArriveAtWaypoint = 0f;
					CycleWaypoint ();
				}
				nextPosition = GetCurrentWaypoint ();
			}
			if (timeSinceArriveAtWaypoint > waypointDwellTime) {

				mover.StartMoveAction (nextPosition);
			}
		}

		private bool AtWaypoint ()
		{
			float distanceToWaypoint = Vector3.Distance (transform.position, GetCurrentWaypoint ());
			return distanceToWaypoint < waypointTollerance;
		}

		private void CycleWaypoint ()
		{
			currentWaypointIndex = patrolPath.GetNextIndex (currentWaypointIndex);
		}

		private Vector3 GetCurrentWaypoint ()
		{
			return patrolPath.GetWaypoint (currentWaypointIndex);
		}

		private void SuspicionBehaviour ()
		{
			GetComponent<ActionScheduler> ().CancelCurrentAction ();
		}

		private void AttackBehaviour ()
		{
			timeSinceLastSawPlayer = 0;
			fighter.Attack (player);
		}

		private bool InAttackRange ()
		{
			float distanceToPlayer = Vector3.Distance (player.transform.position, transform.position);
			return distanceToPlayer < chaseDistance;
		}
	}

}
