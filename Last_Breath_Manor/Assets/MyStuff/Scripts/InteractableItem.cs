using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InteractableItem : MonoBehaviour
{
    public string itemName;
    public ItemType itemType;
    //public bool isSpecial = false;
}


public enum ItemType
{
    Key,
    Book,
    Medkit,
    Bottle,
    WindowCover,
    Potion,
    Computer
    
}

