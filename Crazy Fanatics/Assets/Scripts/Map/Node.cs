using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
	private float neighbourRadius;
	public Node parent;
	public float fitness;
	public float cost;
	public float nodeHardness = 1f;
	public List<Node>  neighbours = new List<Node> ();
	public List<float> neighbourCost = new List<float> ();
	public Color colorDebug = Color.white;
	public bool showArea = false;
	public  int NODE_MASK ;
    public float debugSize = 3.3f;
	//public int onLayer=-99;
	// Use this for initialization
	

	public void getNeighbours (float distance)
	{
		neighbourRadius = distance;
		neighbourCost.Clear ();
		neighbours.Clear ();
		Collider[] col = Physics.OverlapSphere (transform.position, distance, NODE_MASK);
       
		
		for (int i = 0; i < col.Length; i++) {
			if (col [i].gameObject != this.gameObject) {
				RaycastHit rch;
				Physics.Raycast (this.transform.position, (col [i].gameObject.transform.position - this.transform.position).normalized, out rch);
				if (rch.collider == col [i]) {
					Node node = col [i].gameObject.GetComponent<Node> ();
					neighbours.Add (node);
					neighbourCost.Add ((col [i].gameObject.transform.position - transform.position).sqrMagnitude * node.nodeHardness);
				}
			}
		}
	}

	void Awake ()
	{
        NODE_MASK = 1 << LayerMask.NameToLayer("node");
        

    }

	public void init (Node _parent, Node finalNode)
	{
		fitness = 0;
		parent = _parent;
		//heuristic = (finalNode.transform.position - transform.position).sqrMagnitude;
	}

	public float getHeuristic (Node finalNode)
	{
		float heuristic = Mathf.Abs (transform.position.x - finalNode.transform.position.x);
		heuristic += Mathf.Abs (transform.position.z - finalNode.transform.position.z);
		heuristic += Mathf.Abs (transform.position.y - finalNode.transform.position.y);
		return heuristic;
	}

	void OnDrawGizmos ()
	{
		if (showArea) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere (transform.position, neighbourRadius);
		}
		Gizmos.color = colorDebug;
		Gizmos.DrawSphere (transform.position,debugSize);
	}

}
