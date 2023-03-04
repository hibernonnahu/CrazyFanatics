using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    AudioSource audioSource;
    public AudioClip[] clips;
    public static SoundManager Instance;
    // Use this for initialization
    private void Awake()
    {
        Instance = this;
    }
    void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	public void PlayMusic(string name)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(clips[0]);
    }
    
	// Update is called once per frame
	void Update () {
		
	}
}
