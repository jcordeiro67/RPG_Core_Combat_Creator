using UnityEngine;

namespace RPG.Core {

	public class CharacterName : MonoBehaviour {

		[SerializeField] string characterName;

		void Start ()
		{
			if (characterName != null) {
				transform.name = characterName;
			}
		}
	}
}
