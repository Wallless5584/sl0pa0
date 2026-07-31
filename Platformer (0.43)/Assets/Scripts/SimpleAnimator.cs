using UnityEngine;

[SelectionBase]
public class SimpleAnimator : MonoBehaviour
{
    [SerializeField] float frameTime;
    [SerializeField] Sprite[] sprites;
    [SerializeField] SpriteRenderer spriteRenderer;

    float frameTimer = 0f;
    byte currentFrame = 0;



    void Start()
    {
        SetFrame(0);
    }

    void Update()
    {
        Animate();
    }


    private void OnDrawGizmosSelected()
    {
        Animate();
    }

    private void Animate()
    {
        frameTimer += Time.deltaTime;
        if (frameTimer > frameTime)
        {
            SetFrame(++currentFrame);
            frameTimer = 0f;
        }
    }

    void SetFrame(byte newFrame)
    {
        if (newFrame < sprites.Length)
        {
            currentFrame = newFrame;
        }
        else
        {
            currentFrame = 0;
        }
        spriteRenderer.sprite = sprites[currentFrame];
    }
}
