using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {
    public List<Item> itemDatabase = new List<Item>();

    void Start() {
        itemDatabase.Add(new Item("dotory", 1, "맛있는 도토리"));
    }
}
