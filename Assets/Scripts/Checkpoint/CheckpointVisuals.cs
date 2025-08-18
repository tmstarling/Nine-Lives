using UnityEngine;

public class CheckpointVisuals : MonoBehaviour
{
    [SerializeField] Renderer[] x, z;

    [ContextMenu("FixScale")]
    public void FixScale()
    {
        foreach (Renderer renderer in x)
            if (renderer != null)
                renderer.material.mainTextureScale = new Vector2(1, transform.localScale.x / transform.localScale.y);
        foreach (Renderer renderer in z)
            if (renderer != null)
                renderer.material.mainTextureScale = new Vector2(1, transform.localScale.z / transform.localScale.y);
    }

    void Start() => FixScale();
}
