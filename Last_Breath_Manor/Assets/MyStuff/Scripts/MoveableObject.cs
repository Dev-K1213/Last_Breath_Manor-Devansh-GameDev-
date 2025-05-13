using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public int objectNumber;

    public bool isLocked = false;

    // Use multiple required keys
    public List<string> requiredKeyTags = new List<string>();

    [Tooltip("Message shown when the drawer is locked and the player doesn’t have the key(s).")]
    public string lockedMessage = "It's locked. I need to find the key.";

    public void Unlock()
    {
        isLocked = false;
        Debug.Log($"{gameObject.name} unlocked.");
    }
}
