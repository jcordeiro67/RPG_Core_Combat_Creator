using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {

	public class Fighter : MonoBehaviour, IAction {

		[SerializeField] float weaponRange = 2f;
		[SerializeField] float timeBetweenAttacks = 1f;
		[SerializeField] float weaponDamage = 5f;

		float timeSinceLastAttack = Mathf.Infinity;

		Health target;
		Mover mover;
		Animator myAnim;

		private void Start ()
		{
			mover = GetComponent<Mover> ();
			myAnim = GetComponent<Animator> ();
		}

		void Update ()
		{
			timeSinceLastAttack += Time.deltaTime;

			if (target == null) {
				return;
			}
			if (target.IsDead ()) {
				return;
			}

			if (!GetIsInRange ()) {
				mover.MoveTo (target.transform.position);

			} else {
				mover.Cancel ();
				AttackBehaviour ();
			}

		}

		private bool GetIsInRange ()
		{
			return Vector3.Distance (transform.position, target.transform.position) < weaponRange;
		}

		public bool CanAttack (GameObject combatTarget)
		{
			if (combatTarget == null) {
				return false;
			}

			Health targetToTest = combatTarget.GetComponent<Health> ();
			return targetToTest != null && !targetToTest.IsDead ();
		}

		private void AttackBehaviour ()
		{
			if (timeSinceLastAttack > timeBetweenAttacks) {
				StartAttack ();
				timeSinceLastAttack = 0;

			}

		}

		private void StartAttack ()
		{
			myAnim.ResetTrigger ("stopAttack");
			myAnim.SetTrigger ("attack");
		}

		//Called from PlayerController and AIController
		public void Attack (GameObject combatTarget)
		{
			GetComponent<ActionScheduler> ().StartAction (this);
			target = combatTarget.GetComponent<Health> ();

			//Look At Enemy or Player when attacking
			transform.LookAt (target.transform);
		}

		// Animation Event on attack animations of fighter
		void Hit ()
		{
			if (target != null) {

				target.TakeDamage (weaponDamage);
			}
		}

		public void Cancel ()
		{
			StopAttacking ();
			target = null;
		}

		private void StopAttacking ()
		{
			myAnim.ResetTrigger ("attack");
			myAnim.SetTrigger ("stopAttack");
		}
	}
}
