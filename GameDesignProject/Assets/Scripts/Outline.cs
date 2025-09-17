using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Outline : MonoBehaviour
{
    private Material originalMaterial;
    private Material outlineMaterial;

    void Start()
    {
        originalMaterial = GetComponent<Renderer>().material;

        
        outlineMaterial = new Material(Shader.Find("Outlined/Uniform"));
        outlineMaterial.SetColor("_OutlineColor", Color.yellow);
        outlineMaterial.SetFloat("_Outline", 0.05f);
    }

    public void EnableOutline()
    {
        GetComponent<Renderer>().material = outlineMaterial;
    }

    public void DisableOutline()
    {
        GetComponent<Renderer>().material = originalMaterial;
    }
}
