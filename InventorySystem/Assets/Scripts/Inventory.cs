

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


using System.Collections.Generic;

public class Inventory : MonoBehaviour {


	private RectTransform inventoryRect;
	private float inventoryWidth, inventoryHeight;

	private List<GameObject> allSlots;
	
	
	private static int emptySlots;

	private static Slot from, to;
	
	private static GameObject hoverObject;

	private float hoverYOffset;


	public int slots;
	public int rows;
	public float slotPaddingLeft, slotPaddingTop;

	public float slotSize;

	public GameObject slotPrefab;

	public GameObject iconPrefab;

	public Canvas canvas;

	public EventSystem eventSystem;

	public static int EmptySlots {
		get { return emptySlots; }
		set { emptySlots = value; }
	}

	void Start () {
		CreateLayout();
	}
	
	void Update () {

		if (Input.GetMouseButtonUp (0)) {
		
			if(!eventSystem.IsPointerOverGameObject(-1) && from != null) {
				from.GetComponent<Image>().color = Color.white;
				from.ClearSlot();
				Destroy(GameObject.Find("Hover"));
				to = null;
				from = null;
				hoverObject = null;
			}		
		}

		if (hoverObject != null) {
			Vector2 position;

			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
		
			position.Set(position.x, position.y - hoverYOffset);

			hoverObject.transform.position = canvas.transform.TransformPoint(position);
		}
	}

	private void CreateLayout() {
		allSlots = new List<GameObject>();
		emptySlots = slots;

		hoverYOffset = slotSize * 0.01f;

		float inventoryWidth = ((slots/rows)*(slotSize + slotPaddingLeft)) + slotPaddingLeft;
		float inventoryHeight = (rows * (slotSize + slotPaddingTop)) + slotPaddingTop;

		inventoryRect = GetComponent<RectTransform>();
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, inventoryHeight);

		int columns = slots / rows;

		for (int y = 0; y < rows; y++) {
			for (int x = 0; x < columns; x++) {
				GameObject newSlot = (GameObject)Instantiate(slotPrefab);
				RectTransform slotRect = newSlot.GetComponent<RectTransform>();

				newSlot.name = "Slot";
				newSlot.transform.SetParent(this.transform.parent);

				slotRect.localPosition = inventoryRect.localPosition + (new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y)));
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);


				allSlots.Add(newSlot);

//				float xPosition = slotPaddingLeft * (x + 1) + (slotSize * x);
//				float yPosition = slotPaddingTop * (y + 1) + (slotSize * y);
			}
		}
	}


	//
	public bool AddItem(Item item) {
		if (item.maxStackSize == 1) {
			PlaceEmpty(item);
			return true;
		} 
		else {
			foreach(GameObject slot in allSlots) {
				Slot temp = slot.GetComponent<Slot>();
				if (!temp.IsEmpty) {
					if ((temp.CurrentItem.type == item.type) && temp.IsAvailable) {
						temp.AddItem(item);
						return true; 
					}
				}
			}
			if(emptySlots > 0) {
				PlaceEmpty(item);
			}
		}
		return false;
	}


	
	private bool PlaceEmpty(Item item) {
		if (emptySlots > 0) {
			foreach(GameObject slot in allSlots) {
				Slot temp = slot.GetComponent<Slot>();

				if(temp.IsEmpty) {
					temp.AddItem(item);
					emptySlots--;
					return true;
				}
			}
		}
		Debug.Log("No more empty slots");
		return false;
	}


	
	public void MoveItem(GameObject clicked) {

		if (from == null) {
			if (!clicked.GetComponent<Slot>().IsEmpty) {
				from = clicked.GetComponent<Slot>();
				from.GetComponent<Image>().color = Color.gray; 

				hoverObject = (GameObject)Instantiate(iconPrefab);
				hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
				hoverObject.name = "Hover";

				RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
				RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

				hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
				hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

				hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);

				hoverObject.transform.localScale = from.gameObject.transform.localScale;

			}	
		} else if (to == null) {
			to = clicked.GetComponent<Slot>();

			Destroy(GameObject.Find("Hover"));
		}

		if (to != null && from != null) {
			Stack<Item> tempTo = new Stack<Item>(to.Items);
			to.AddItems(from.Items);

			if (tempTo.Count == 0) {
				from.ClearSlot();
			} else {
				from.AddItems(tempTo);
			}

			from.GetComponent<Image>().color = Color.white;

			to = null;
			from = null;
			hoverObject = null;
		}
	}




} 


