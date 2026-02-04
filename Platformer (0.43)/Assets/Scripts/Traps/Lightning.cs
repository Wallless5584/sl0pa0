using UnityEngine;

public class Lightning : MonoBehaviour
{

    [SerializeField] Material material;
    [SerializeField] float scrollSpeed;
    [SerializeField] bool invert;
    [SerializeField] float invertPeriod;

    [Header("References")]
    [SerializeField] Transform target;
    [SerializeField] LineRenderer lineRenderer;

    float invertTimer = 0f;
    int invertMod = 1;
    float offsetX = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        OffsetTexture();

        lineRenderer.SetPosition(1, target.transform.localPosition);
    }

    private void OnValidate()
    {
        lineRenderer.SetPosition(1, target.transform.localPosition);
    }

    void OffsetTexture()
    {
        offsetX += scrollSpeed * Time.deltaTime * invertMod;

        if (invert)
        {
            invertTimer -= Time.deltaTime;
            if (invertTimer < 0f)
            {
                invertMod *= -1;
                offsetX = -offsetX;
                invertTimer = invertPeriod;
            }
        }
        material.mainTextureOffset = new Vector2(offsetX, 0);
    }

    public void SetEndPosition(Vector2 localPosition)
    {
        float horizontalCoefficient = Vector3.Dot(transform.right, Vector3.right);
        float verticalCoefficient = Vector3.Dot(transform.right, Vector3.up);
        Vector2 offsetPos = new Vector2(
            x: localPosition.x * horizontalCoefficient + localPosition.y * verticalCoefficient,
            y: localPosition.y * horizontalCoefficient + localPosition.x * verticalCoefficient
            //x: localPosition.x * horizontalCoefficient - c_rightOffset * horizontalCoefficient + localPosition.y * verticalCoefficient,
            //y: localPosition.y * verticalCoefficient - c_rightOffset * verticalCoefficient - localPosition.x * horizontalCoefficient
            );
        Debug.Log($"SetEndPosition | localPosition {localPosition}, horCoeff {horizontalCoefficient}, verCoef {verticalCoefficient}, offsetPos {offsetPos}");
        int linePointIndex = lineRenderer.positionCount - 1;
        lineRenderer.SetPosition(linePointIndex, offsetPos);
    }
}
