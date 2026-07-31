using UnityEngine;

public class SubobjectDescaler : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    private void Start()
    {
        SetScale();
    }
    private void OnDrawGizmosSelected()
    {
        SetScale();
    }

    void SetScale()
    {
        spriteRenderer.transform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1);
        spriteRenderer.size = transform.localScale;
    }
}
