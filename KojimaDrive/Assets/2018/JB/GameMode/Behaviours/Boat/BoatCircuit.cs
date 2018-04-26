using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JB
{

public class BoatCircuit : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] bool loop = false;

    private List<BoatCircuitNode> nodes = new List<BoatCircuitNode>();


    public BoatCircuitNode GetStartPoint()
    {
        if (nodes.Count == 0)
            UpdateNodeList();

        return nodes[0];
    }

    
    void Start()
    {
        UpdateNodeList();
    }


    void OnDrawGizmos()
    {
        UpdateNodeList();
        
        int i = 0;
        foreach (var node in nodes)
        {
            Gizmos.color = i == 0 ? Color.green : Color.red;
            Gizmos.DrawSphere(node.transform.position, i == 0 ? 2 : 1);

            if (node.nextNode == null)
                continue;

            Gizmos.color = Color.white;
            Gizmos.DrawLine(node.transform.position, node.nextNode.transform.position);

            ++i;
        }
    }


    void UpdateNodeList()
    {
        nodes = GetComponentsInChildren<BoatCircuitNode>().ToList();

        int i = 0;
        foreach (var node in nodes)
        {
            node.prevNode = i > 0 ? nodes[i - 1] : null;

            if (i + 1 < nodes.Count)
            {
                node.nextNode = nodes[i + 1];
            }
            else
            {
                node.nextNode = loop ? nodes[0] : null;
            }

            ++i;
        }
    }

}

} // namespace JB
