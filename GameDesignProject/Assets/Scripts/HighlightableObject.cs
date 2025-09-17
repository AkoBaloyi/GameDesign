using UnityEngine;

[RequireComponent(typeof(PickUpObject))]
[RequireComponent(typeof(Outline))]
public class HighlightableObject : MonoBehaviour
{
    private Outline outline;

    void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false; 
    }

    public void HighlightOn()
    {
        outline.enabled = true;
    }

    public void HighlightOff()
    {
        outline.enabled = false;
    }
}
