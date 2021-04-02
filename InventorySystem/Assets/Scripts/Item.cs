
using UnityEngine;
using System.Collections;

public enum ItemType {HEALTH, DAMAGE};

public class Item : MonoBehaviour {

	public ItemType type;

	public Sprite spriteNeutral;
	public Sprite spriteHighlighted;

	public int maxStackSize;


	public void Use() {
		switch (type) {
		case ItemType.HEALTH:
			Debug.Log ("Health potion used.");
			break;
		case ItemType.DAMAGE:
			Debug.Log ("Damage potion drank.");
			break;
		}
	}
}
