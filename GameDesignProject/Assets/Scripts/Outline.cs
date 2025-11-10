using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Outline : MonoBehaviour
{
    [Header("Outline Settings")]
    public Color outlineColor = Color.yellow;
    public float outlineWidth = 0.05f;
    
    private Material[] originalMaterials;
    private Material[] outlineMaterials;
    private Renderer objectRenderer;
    private bool isOutlined = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterials = objectRenderer.materials;

        CreateOutlineMaterials();
    }

    void CreateOutlineMaterials()
    {

        Shader outlineShader = Shader.Find("Outlined/Uniform");
        
        if (outlineShader == null)
        {

            outlineShader = Shader.Find("Standard");
        }

        outlineMaterials = new Material[originalMaterials.Length];
        
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            outlineMaterials[i] = new Material(originalMaterials[i]);
            
            if (outlineShader.name == "Outlined/Uniform")
            {

                outlineMaterials[i].shader = outlineShader;
                outlineMaterials[i].SetColor("_OutlineColor", outlineColor);
                outlineMaterials[i].SetFloat("_Outline", outlineWidth);
            }
            else
            {

                outlineMaterials[i].EnableKeyword("_EMISSION");
                outlineMaterials[i].SetColor("_EmissionColor", outlineColor * 0.5f);
            }
        }
    }

    public void EnableOutline()
    {
        if (!isOutlined && outlineMaterials != null)
        {
            objectRenderer.materials = outlineMaterials;
            isOutlined = true;
        }
    }

    public void DisableOutline()
    {
        if (isOutlined && originalMaterials != null)
        {
            objectRenderer.materials = originalMaterials;
            isOutlined = false;
        }
    }
    
    void OnDestroy()
    {

        if (outlineMaterials != null)
        {
            for (int i = 0; i < outlineMaterials.Length; i++)
            {
                if (outlineMaterials[i] != null)
                {
                    DestroyImmediate(outlineMaterials[i]);
                }
            }
        }
    }
}