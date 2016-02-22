using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    public int slotX;
    public int slotY;
    public List<Item> inventory = new List<Item>();
    public List<Item> slots = new List<Item>();
    public GUISkin skin;

    [HideInInspector] public bool inventoryButtonClicked = false;

    private ItemDatabase database;
    private int inventorySize;
    private bool showInventory = false;

    private float slotPosX = 15f;
    private float slotPosY = 15f;

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

        database = GameObject.Find("Database").GetComponent<ItemDatabase>();
	}

    void Update()
    {
        if ((Input.GetButtonDown("Inventory") && !GameManager.gm.isInterRound))
        {
            showInventory = !showInventory;
        }

        else if (GameManager.gm.isInterRound)
        {
            showInventory = inventoryButtonClicked;
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
        GUI.Box(new Rect(slotPosX/2 + 0f, slotPosY/2 + 0f, ((50+8) * slotX) + slotPosX, ((50+8) * slotY) + slotPosY), "", skin.GetStyle("InventoryBackground"));
        for (int y = 0; y < slotY; y++)
        {
            for (int x = 0; x < slotX; x++)
            {

                Rect slotRect = new Rect(slotPosX + x * 60, slotPosY + y * 60, 50, 50);
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
                            if (Input.GetMouseButtonDown(0) && isOneClicked && !GameManager.gm.isInterRound)
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

    public string CreateTooltip(Item item)
    {
        string tooltip = "<color=#ffffff>" + item.itemName + "</color>\n\n";
        tooltip += "<color=#029919>" + item.itemDescription + "</color>\n\n";
        tooltip += "<color=#990282>" + "가격 : " + item.itemPrice + "</color>";
        return tooltip;
    }

    public void AddItem(Item item)
    {
        if (GameManager.gm.Money() < item.itemPrice)
        {
            //나중에 창으로 나오도록 고치자
            Debug.Log("돈이 부족합니다.");
            return;
        }

        for (int i = 0; i < inventorySize; i++)
        {
            if (inventory[i].itemName == null)
            {
                for (int j = 0; j < database.itemDatabase.Count; j++)
                {
                    if (database.itemDatabase[j].itemID == item.itemID)
                    {
                        inventory[i] = database.itemDatabase[j];
                        GameManager.gm.ChangeMoneyInterRound(-item.itemPrice);
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
