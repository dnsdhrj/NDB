using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    public int slotX;
    public int slotY;
    public List<Item> inventory = new List<Item>();
    public List<Item> slots = new List<Item>();
    public GUISkin skin;

    private ItemDatabase database;
    private int inventorySize;
    private bool showInventory = false;

    private bool showTooltip = false;
    private string tooltip;

    private bool isDragging = false;
    private Item clickedItem;
    private int clickedItemIndex;
    private bool isOneClicked = false;
    private double prevClickTime;
    private double doubleClickTime = 0.25f;

    void Start ()
    {
        inventorySize = slotX * slotY;

        for (int i = 0; i < inventorySize; i++)
        {
            slots.Add(new Item());
            inventory.Add(new Item());
        }

        database = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();

        // 테스트용, 나중에 삭제
        AddItem(0);
        AddItem(0);
	}

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            showInventory = !showInventory;
        }
    }

    void OnGUI()
    {

        GUI.skin = skin;
        tooltip = "";
        if (showInventory)
        {
            DrawInventory();

            if (showTooltip)
            {
                GUI.Box(new Rect(Event.current.mousePosition.x + 10f, Event.current.mousePosition.y, 200, 200), tooltip, skin.GetStyle("Tooltip"));
            }
        }

        if (isDragging)
        {
            GUI.DrawTexture(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 50, 50), clickedItem.itemImage);
        }


    }

    void DrawInventory()
    {
        int i = 0;

        for (int y = 0; y < slotY; y++)
        {
            for (int x = 0; x < slotX; x++)
            {
                Rect slotRect = new Rect(x * 60, y * 60, 50, 50);
                GUI.Box(slotRect, "", skin.GetStyle("Slots"));

                slots[i] = inventory[i];

                if (slots[i].itemName != null)
                {
                    GUI.DrawTexture(slotRect, slots[i].itemImage);

                    if (slotRect.Contains(Event.current.mousePosition))
                    {
                        if (!isDragging)
                        {
                            showTooltip = true;
                            tooltip = CreateTooltip(slots[i]);
                        }

                        if (Event.current.button == 0 /*leftMouseButton*/)
                        {


                            //아이템 드래그
                            if (Event.current.type == EventType.MouseDrag && !isDragging)
                            {
                                clickedItem = slots[i];
                                clickedItemIndex = i;
                                isDragging = true;
                                inventory[i] = new Item();
                                isOneClicked = false;
                            }

                            if (Input.GetMouseButtonUp(0) && !isOneClicked)
                            {
                                isOneClicked = true;
                                prevClickTime = Time.time;
                            }

                            //더블클릭(아이템 사용)
                            if (Input.GetMouseButtonDown(0) && isOneClicked)
                            {
                                clickedItem = slots[i];
                                clickedItemIndex = i;

                                if (Time.time - prevClickTime < doubleClickTime)
                                {
                                    Debug.Log("doubleClicked");

                                    database.useItem(clickedItem);
                                    RemoveItem(clickedItemIndex);
                                }

                                isOneClicked = false;
                            }

                        }

                        if (Event.current.type == EventType.MouseUp && isDragging)
                        {
                            inventory[clickedItemIndex] = inventory[i];
                            inventory[i] = clickedItem;
                            isDragging = false;
                            clickedItem = null;
                        }
                    }
                }
                else
                {
                    if (slotRect.Contains(Event.current.mousePosition))
                    {
                        if (Event.current.type == EventType.MouseUp && isDragging)
                        {
                            inventory[clickedItemIndex] = inventory[i];
                            inventory[i] = clickedItem;
                            isDragging = false;
                            clickedItem = null;
                        }
                    }
                }

                if (tooltip == "") showTooltip = false;
                i++;
            }
        }
    }

    string CreateTooltip(Item item)
    {
        string tooltip = "<color=#ffffff>" + item.itemName + "</color>\n\n";
        tooltip += "<color=#029919>" + item.itemDescription + "</color>\n\n";
        tooltip += "<color=#990282>" + "가격 : " + item.itemPrice + "</color>";
        return tooltip;
    }

    void AddItem(int id) {
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventory[i].itemName == null)
            {
                for (int j = 0; j < database.itemDatabase.Count; j++)
                {
                    if (database.itemDatabase[j].itemID == id)
                    {
                        inventory[i] = database.itemDatabase[j];
                        break;
                    }
                }
                return;
            }
        }

        //나중에 창 형태로 나오도록 고치자.
        Debug.Log("인벤토리가 꽉찼습니다.");
    }

    bool IsInventoryContains(int id)
    {
        bool res = false;

        for (int i = 0; i < inventorySize; i++)
        {
            res = (inventory[i].itemID == id);

            if (res)
                break;
        }

        return res;
    }

    void RemoveItem(int index)
    {
        inventory[index] = new Item();
    }
}
