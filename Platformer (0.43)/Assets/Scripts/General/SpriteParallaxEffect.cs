using Unity.VisualScripting;
using UnityEngine;

public class SpriteParallaxEffect : MonoBehaviour
{
    [SerializeField] Vector3 RefPoint;
    [SerializeField] float ParallaxSpeed;
    [SerializeField] Vector3 HintOffset;

    // Start is called before the first frame update
    void Start()
    {
        HintOffset = gameObject.transform.localPosition;
        Disable();
        transform.parent.parent.GetComponent<RoomController>().AddHint(this);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = RefPoint + HintOffset + (new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0) - RefPoint) * ParallaxSpeed;
    }

    public void Setup()
    {
        gameObject.SetActive(true);
        RefPoint = gameObject.transform.parent.position;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = RefPoint + HintOffset;
    }
}
