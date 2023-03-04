using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCreator : MonoBehaviour {

    List<Node> nodeList;
    public Node nodePrefab;
    public float nodeDist = 1f;
    
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void CreateNodes (GameObject bound)
    {
       
        int ii = (int)(bound.transform.localScale.x / nodeDist);
        int jj = (int)(bound.transform.localScale.y / nodeDist);
       
        for (int i = 0; i < ii; i++)
        {
            for (int j = 0; j < jj; j++)
            {
                Vector3 position =  Vector3.right* i*nodeDist + Vector3.forward * j * nodeDist;
               
               if( !Physics.Raycast(position + Vector3.up * 5, Vector3.down,6f,1<<LayerMask.NameToLayer("wall")))
                {
                    Instantiate<Node>(nodePrefab, position, Quaternion.identity);
                }

               
                
            }
        }
        Astar.getInstance().LoadNodesNeighbours(nodeDist+0.01f);

    }
}
