using UnityEngine;

[ExecuteAlways]
public class MinimapCalibrator : MonoBehaviour
{
    [SerializeField] private RectTransform arrowRect;

    [Header("References (3 anchors required)")]
    public Transform[] worldAnchors = new Transform[3];      // world points (use X = world.x, Y = world.z)
    public RectTransform[] uiAnchors = new RectTransform[3]; // corresponding UI anchoredPositions (anchored to minimap parent)
    
    [Header("Minimap & Arrow")]
    public RectTransform minimapBG;  // Minimap background RectTransform (pivot = 0.5,0.5)
    public RectTransform arrow;      // Player arrow UI RectTransform (child of minimapBG)
    public Transform player;         // Player root transform (use position.x and position.z)

    // Affine coefficients: uiX = a*x + b*y + tx ; uiY = c*x + d*y + ty
    private float a,b,tx,c,d,ty;
    private bool calibrated = false;

    // Editor / manual call
    [ContextMenu("Calibrate")]
    public void Calibrate()
    {
        if (worldAnchors == null || uiAnchors == null || worldAnchors.Length < 3 || uiAnchors.Length < 3)
        {
            Debug.LogError("Assign exactly 3 anchors in the inspector.");
            calibrated = false;
            return;
        }

        // Build A matrix (3x3) from world anchors using (x, y, 1) where y = world.z
        float[,] A = new float[3,3];
        float[] u = new float[3];
        float[] v = new float[3];

        for (int i = 0; i < 3; i++)
        {
            Vector3 w = worldAnchors[i].position;
            Vector2 uiPos = uiAnchors[i].anchoredPosition; // anchoredPosition relative to minimapBG
            A[i,0] = w.x;
            A[i,1] = w.z;
            A[i,2] = 1f;
            u[i] = uiPos.x;
            v[i] = uiPos.y;
        }

        float[,] invA;
        if (!Inverse3x3(A, out invA))
        {
            Debug.LogError("Anchor world points are degenerate (collinear) — choose non-collinear anchors.");
            calibrated = false;
            return;
        }

        // Solve [a b tx]^T = invA * u  and [c d ty]^T = invA * v
        float[] solU = MultiplyMatrixVector(invA, u);
        float[] solV = MultiplyMatrixVector(invA, v);

        a = solU[0]; b = solU[1]; tx = solU[2];
        c = solV[0]; d = solV[1]; ty = solV[2];

        calibrated = true;
        Debug.Log("Minimap calibrated successfully.");
    }

    void Update()
    {
        // Optionally auto-calibrate in editor when anchors change:
#if UNITY_EDITOR
        if (!Application.isPlaying && worldAnchors.Length >= 3 && uiAnchors.Length >= 3 && !calibrated)
        {
            // Attempt calibrate once (useful while editing)
            Calibrate();
        }
#endif
        if (!calibrated) return;
        if (player == null || arrow == null) return;

        // Map player world (x,z) to ui anchored position
        Vector3 p = player.position;
        float uiX = a * p.x + b * p.z + tx;
        float uiY = c * p.x + d * p.z + ty;

        // Apply position and rotation
        arrow.anchoredPosition = new Vector2(uiX, uiY);

        // Rotate arrow: match player's yaw (negative because UI y-axis is up)
        float yaw = player.eulerAngles.y;
        arrowRect.localEulerAngles = new Vector3(0, 0, -player.eulerAngles.y);
    }

    // Helper: proper rotation assignment
    private void SetArrowRotation(float yaw)
    {
        if (arrow != null)
            arrow.localEulerAngles = new Vector3(0f, 0f, -yaw);
    }

    // Multiply 3x3 matrix by 3x1 vector
    private static float[] MultiplyMatrixVector(float[,] M, float[] v)
    {
        float[] r = new float[3];
        for (int i = 0; i < 3; i++)
            r[i] = M[i,0]*v[0] + M[i,1]*v[1] + M[i,2]*v[2];
        return r;
    }

    // Inverse of 3x3 matrix. Returns false if determinant ~ 0.
    private static bool Inverse3x3(float[,] m, out float[,] inv)
    {
        inv = new float[3,3];
        float m00 = m[0,0], m01 = m[0,1], m02 = m[0,2];
        float m10 = m[1,0], m11 = m[1,1], m12 = m[1,2];
        float m20 = m[2,0], m21 = m[2,1], m22 = m[2,2];

        float det = m00*(m11*m22 - m12*m21) - m01*(m10*m22 - m12*m20) + m02*(m10*m21 - m11*m20);
        if (Mathf.Abs(det) < 1e-6f) return false;

        // Cofactor matrix (not transposed yet)
        float c00 =  (m11*m22 - m12*m21);
        float c01 = -(m10*m22 - m12*m20);
        float c02 =  (m10*m21 - m11*m20);

        float c10 = -(m01*m22 - m02*m21);
        float c11 =  (m00*m22 - m02*m20);
        float c12 = -(m00*m21 - m01*m20);

        float c20 =  (m01*m12 - m02*m11);
        float c21 = -(m00*m12 - m02*m10);
        float c22 =  (m00*m11 - m01*m10);

        // adjugate = cofactor^T, then divide by det
        inv[0,0] = c00 / det;
        inv[0,1] = c10 / det;
        inv[0,2] = c20 / det;

        inv[1,0] = c01 / det;
        inv[1,1] = c11 / det;
        inv[1,2] = c21 / det;

        inv[2,0] = c02 / det;
        inv[2,1] = c12 / det;
        inv[2,2] = c22 / det;

        return true;
    }

    // ensure compile by setting rotation via this call
    private void LateUpdate()
    {
        if (!calibrated || player == null || arrow == null) return;
        SetArrowRotation(player.eulerAngles.y);
    }
}
