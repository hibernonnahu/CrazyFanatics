using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillIDs : MonoBehaviour {

	// Use this for initialization
	public int[] ids;
    public int count = 0;
    public TextMesh text;
    public GameObject reward;
    private void Start()
    {
        if(count<=0)
        {
            text.text = "";
        }
        else
        {
            text.text = count.ToString();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Human human = other.gameObject.GetComponent<Human>();
        if (human!=null)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                if(ids[i]==human.id)
                {
                    human.ModifyLife(-100000);
                    count--;
                    if (count == 0)
                    {
                        text.text = "";
                        GameObject go = Instantiate(reward);
                        go.transform.position = transform.position+Vector3.up*2;
                        go.GetComponent<Rigidbody>().velocity = Vector3.up * 3;
                    }
                    else if(count>0)
                    {
                        text.text = count.ToString();
                    }
                    break;
                }
            }
            
        }
    }
}
