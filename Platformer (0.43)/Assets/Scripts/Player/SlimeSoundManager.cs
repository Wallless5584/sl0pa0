using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    None,
    Bounce,
    Jump,
    Slide,
}

public class SlimeSoundManager : MonoBehaviour
{
    public static SlimeSoundManager Instance;

    AudioSource soundSource;
    [SerializeField] AudioClip bounce;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip slide;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        soundSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.None:
                soundSource.Stop();
                break;
            case SoundType.Bounce:
                soundSource.Stop();
                soundSource.PlayOneShot(bounce);
                break;
            case SoundType.Jump:
                soundSource.Stop();
                soundSource.PlayOneShot(jump);
                break;
            case SoundType.Slide:
                soundSource.clip = slide;
                soundSource.Play();
                soundSource.loop = true;
                break;
        }
    }
}
