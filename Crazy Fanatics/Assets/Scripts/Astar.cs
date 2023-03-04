using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Astar
{

    public List<Node> nodes = new List<Node>();
    public List<Node> openList = new List<Node>();
    public List<Node> closeList = new List<Node>();
    public bool found;
    //for debug
    public Node from;
    public Node to;

    public static Astar instance;

    public static Astar getInstance()
    {
        if (instance == null) {
            instance = new Astar();
        }
        return instance;
    }

    public Astar()
    {

        loadNodeList();

    }
    public Node GetClosestNode(Vector3 v3)
    {
        float closeDist = float.MaxValue;
        Node closest = null;
        foreach (var node in nodes)
        {
            float newDist = (v3 - node.transform.position).sqrMagnitude;
            if (newDist < closeDist)
            {
                closeDist = newDist;
                closest = node;
            }
        }
        return closest;
    }
       
		public List<Node> getPath (Node from, Node to)
		{
				
				found = false;
				openList.Clear ();
				closeList.Clear ();
				from.init (from, to);
				openList.Add (from);
				exploreAStar (to);
				if (found) {

						return asamblePath (from, to);
				}
				//Debug.Log ("path not found");
				return null;
		}

		public void exploreAStar (Node to)
		{
				if (openList.Count > 0) {
						Node current = getOpenNode ();
						if (current != to) {
								for (int i = 0; i < current.neighbours.Count; i++) {


										if (!openList.Contains (current.neighbours [i]) && !closeList.Contains (current.neighbours [i])) {
												current.neighbours [i].parent = current;
												current.neighbours [i].fitness = current.neighbours [i].getHeuristic (to) + current.neighbourCost [i] + current.cost;
												current.neighbours [i].cost = current.cost + current.neighbourCost [i];
												openList.Add (current.neighbours [i]);
												current.neighbours [i].colorDebug = Color.grey;
										} else if (openList.Contains (current.neighbours [i])) {
												float aux = current.neighbourCost [i] + current.cost;
												if (current.neighbours [i].cost > aux) {
														current.neighbours [i].parent = current;
														current.neighbours [i].cost = aux;
												}
										}



								}
								openList.Remove (current);
								closeList.Add (current);
								exploreAStar (to);
						} else {

								found = true;
						}
				}

				
		}

		private List<Node> asamblePath (Node from, Node to)
		{
				List<Node> l = new List<Node> ();
		
				Node aux = to;
				while (aux!=aux.parent) {
						l.Insert (0, aux);
						aux.colorDebug = Color.cyan;
						aux = aux.parent;
				}
				l.Insert (0, from);
				return l;
		}

		private Node getOpenNode ()
		{
				Node node = openList [0];
		
				for (int i = 1; i < openList.Count; i++) {
						if (openList [i].fitness < node.fitness) {
								node = openList [i];
						}
				}
				return node;
		}

		private void loadNodeList ()
		{
				GameObject[] go = GameObject.FindGameObjectsWithTag ("node");
				for (int i = 0; i < go.Length; i++) {
						nodes.Add (go [i].GetComponent<Node> ());

				}
		}
        public void LoadNodesNeighbours(float distance)
    {
        
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].getNeighbours(distance);
        }
    }

}
