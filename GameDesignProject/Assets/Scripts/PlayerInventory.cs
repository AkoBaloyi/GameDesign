using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Keycards")]
    public List<string> keycards = new List<string>(); 

    
    public void CollectKeycard(string id)
    {
        if (!keycards.Contains(id))
        {
            keycards.Add(id);
            Debug.Log($"Collected Keycard: {id}");
        }
    }

   
    public bool HasKeycard(string id)
    {
        return keycards.Contains(id);
    }
}