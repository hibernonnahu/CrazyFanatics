using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawn : MonoBehaviour {
    private List<GameObject> objects = new List<GameObject>();

    // Use this for initialization
    private void Start()
    {
        foreach (Transform item in transform.GetComponentInChildren<Transform>())
        {

            objects.Add(item.gameObject);
            
        }
        foreach (var item in objects)
        {
            item.gameObject.SetActive(false);
            Vector3 worldPos = item.transform.localPosition;
            item.transform.parent = null;
            item.transform.position = worldPos+transform.position;
        }
    }
    
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        foreach (var item in objects)
        {
            item.SetActive(true);
            Human human = item.GetComponent<Human>();
            if(human!=null)
            {
                if(other.gameObject.layer!= LayerMask.NameToLayer("victim"))
                {
                    GameManager.Instance.combatIAList.Add(human);
                }
                else
                {
                    GameManager.Instance.allyList.Add(human);

                }
            }
        }
        Destroy(this.gameObject);
    }
}
