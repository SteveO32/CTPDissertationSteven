using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Aero : MonoBehaviour
{
    [SerializeField]
    private Vector3 _position;
    [SerializeField]
    private readonly float[][] _tensor;

    [SerializeField] private Vector3 _windVector3;

    public Aero(Vector3 position, Vector3 windVector3, float[][] tensor)
    {
        this._position = position;
        this._windVector3 = windVector3;
        this._tensor = tensor;
    }

    // Use this for initialization
    void Start ()
    {
        //setup 3x3 matrix
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                _tensor[i][j] = 0.0f;
            }   
        }
        //size = GetComponent<MeshCollider>().sharedMesh.bounds.size;
    }
	
	// Update is called once per frame
	void Update ()
	{
    }

    void UpdateForce()
    {

    }
   

}
