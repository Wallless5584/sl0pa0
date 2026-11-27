using UnityEngine;

public class SlimeMarkManager : MonoBehaviour
{
    public static SlimeMarkManager instance;
    [SerializeField] SpriteRenderer markPrefab;
    [SerializeField] Sprite[] markSprites;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        
    }

    public void AddMark(Vector2 markPos)
    {
        SpriteRenderer newMark = Instantiate(markPrefab, position:markPos, rotation:Quaternion.identity);
        newMark.sprite = markSprites[Random.Range(0, markSprites.Length)];
    }
}
