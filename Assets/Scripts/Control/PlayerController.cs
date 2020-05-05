using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control {

	public class PlayerController : MonoBehaviour {

		void Update ()
		{
			if (InteractWithCombat ()) return;
			if (InteractWithMovement ()) return;
			//TODO: add a interaction to let player know nothing to do here
		}

		private bool InteractWithCombat ()
		{
			RaycastHit [] hits = Physics.RaycastAll (GetMouseRay ());

			foreach (RaycastHit hit in hits) {
				CombatTarget target = hit.transform.GetComponent<CombatTarget> ();

				if (!GetComponent<Fighter> ().CanAttack (target)) {
					continue;
				}

				if (Input.GetMouseButtonDown (0)) {

					GetComponent<Fighter> ().Attack (target);

				}
				return true;
			}
			return false;
		}

		private bool InteractWithMovement ()
		{
			bool hasHit = Physics.Raycast (GetMouseRay (), out RaycastHit hit);

			if (hasHit) {
				if (Input.GetMouseButton (0)) {

					GetComponent<Mover> ().StartMoveAction (hit.point);
				}
				return true;
			}
			return false;
		}


		private static Ray GetMouseRay ()
		{
			return Camera.main.ScreenPointToRay (Input.mousePosition);
		}
	}
}
