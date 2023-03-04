using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIconSetter : MonoBehaviour
{
    public Sprite[] img;
    // Start is called before the first frame update
    void Awake()
    {
        int code = AttributesValues.enemyGender;
        if (code == 2) {
            code = Random.Range(0, 2);
        }
        if(code<img.Length)
        GetComponent<Image>().sprite = img[code];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
