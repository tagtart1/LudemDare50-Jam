using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] AudioSource effectSource;

    private void Awake()
    {
        Instance = this;
    }
    private void PlayEffectSound(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }
    public static void PlayEffectSound_Static(AudioClip clip)
    {
        Instance.PlayEffectSound(clip);
    }

  }
