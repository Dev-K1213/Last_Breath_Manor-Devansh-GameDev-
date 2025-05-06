using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private HashSet<string> collectedItems = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectItem(string itemTag)
    {
        if (!collectedItems.Contains(itemTag))
        {
            collectedItems.Add(itemTag);
            Debug.Log($"Collected item: {itemTag}");
        }
    }

    public bool HasItem(string itemTag)
    {
        return collectedItems.Contains(itemTag);
    }
}
