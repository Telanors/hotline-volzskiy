using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadShitScript : MonoBehaviour
{
    private AudioSource audioSource;
    private Animator animator;
    private void Awake()
    {
        animator= GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        if (!audioSource.isPlaying)
        {
            AnimStart();
        }
    }
    public void AnimStart()
    {
        animator.SetTrigger("End");
    }
    public void StartMusic()
    {
        audioSource.Play();
        Invoke(nameof(AnimStart), audioSource.clip.length - 2f);
    }
}
