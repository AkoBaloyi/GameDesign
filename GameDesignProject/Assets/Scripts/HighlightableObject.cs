using UnityEngine;

[RequireComponent(typeof(PickUpObject))]
public class HighlightableObject : MonoBehaviour
{
    [Header("Highlight Settings")]
    public Color highlightColor = Color.yellow;
    public float highlightIntensity = 2f;
    
    private Renderer objectRenderer;
    private Material[] originalMaterials;
    private Material[] highlightMaterials;
    private bool isHighlighted = false;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError($"HighlightableObject on {gameObject.name} needs a Renderer component!");
            return;
        }
        
        originalMaterials = objectRenderer.materials;
        CreateHighlightMaterials();
    }

    void CreateHighlightMaterials()
    {
        highlightMaterials = new Material[originalMaterials.Length];
        
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            highlightMaterials[i] = new Material(originalMaterials[i]);
            
            // Enable emission
            highlightMaterials[i].EnableKeyword("_EMISSION");
            highlightMaterials[i].SetColor("_EmissionColor", highlightColor * highlightIntensity);
            
            // Make it slightly brighter
            if (highlightMaterials[i].HasProperty("_Color"))
            {
                Color baseColor = highlightMaterials[i].GetColor("_Color");
                highlightMaterials[i].SetColor("_Color", baseColor * 1.2f);
            }
        }
    }

    public void HighlightOn()
    {
        if (!isHighlighted && highlightMaterials != null && objectRenderer != null)
        {
            objectRenderer.materials = highlightMaterials;
            isHighlighted = true;
            Debug.Log($"Highlighting {gameObject.name}");
        }
    }

    public void HighlightOff()
    {
        if (isHighlighted && originalMaterials != null && objectRenderer != null)
        {
            objectRenderer.materials = originalMaterials;
            isHighlighted = false;
            Debug.Log($"Un-highlighting {gameObject.name}");
        }
    }
    
    void OnDestroy()
    {
        // Clean up created materials
        if (highlightMaterials != null)
        {
            for (int i = 0; i < highlightMaterials.Length; i++)
            {
                if (highlightMaterials[i] != null)
                {
                    DestroyImmediate(highlightMaterials[i]);
                }
            }
        }
    }
}