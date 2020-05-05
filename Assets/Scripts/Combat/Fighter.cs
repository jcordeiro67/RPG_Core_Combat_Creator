using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {

	public class Fighter : MonoBehaviour, IAction {

		[SerializeField] float weaponRange = 2f;
		[SerializeField] float timeBetweenAttacks = 1f;
		[SerializeField] float weaponDamage = 5f;

		float timeSinceLastAttack = 0f;

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
			//Fixed NullReference by moving bool inside first if and breaing
			//single if statement into nested if statements

			return Vector3.Distance (transform.position, target.transform.position) < weaponRange;
		}

		public bool CanAttack (CombatTarget combatTarget)
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
				myAnim.ResetTrigger ("stopAttack");
				myAnim.SetTrigger ("attack");
				timeSinceLastAttack = 0;

			}

		}

		//Called from PlayerController
		public void Attack (CombatTarget combatTarget)
		{
			GetComponent<ActionScheduler> ().StartAction (this);
			target = combatTarget.GetComponent<Health> ();

			//Look At Enemy or Player when attacking
			transform.LookAt (target.transform);

			string targetName = target.gameObject.name;
			print ("Attacking " + targetName);
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
			myAnim.SetTrigger ("stopAttack");
			target = null;
		}

	}
}
