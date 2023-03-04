using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxSoundManager : MonoBehaviour {
    public AudioSource[] audioSource;
    public AudioClip[] clips;
    public static FxSoundManager Instance;
    int counter = 0;
    // Use this for initialization
     void Awake()
    {
        Instance = this;
    }
    void Start () {
       
	}
	public void PlayFx(int code)
    {
        
        audioSource[counter].PlayOneShot(clips[code]);
        counter = (counter + 1) % audioSource.Length;
    }
    
	// Update is called once per frame
	void Update () {
		
	}
}
