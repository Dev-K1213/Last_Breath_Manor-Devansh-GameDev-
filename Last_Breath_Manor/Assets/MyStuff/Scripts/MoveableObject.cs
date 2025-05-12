using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public int objectNumber;

    public bool isLocked = false;

    // Use multiple required keys
    public List<string> requiredKeyTags = new List<string>();

    public void Unlock()
    {
        isLocked = false;
        Debug.Log($"{gameObject.name} unlocked.");
    }
}
