using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour {
    public GameObject toClose;
	// Use this for initialization
	public void OnClick()
    {
        toClose.SetActive(false);
    }
}
