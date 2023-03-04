using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    public int id = 0;
    public void DestroyScript()
    {
        Destroy(this);
    }
}
