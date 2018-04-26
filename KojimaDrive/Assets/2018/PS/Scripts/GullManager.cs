using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS
{
    public class GullManager : MonoBehaviour
    {
        public int GullAmount = 15;
        private GameObject[] gullVec;
        private int minX = -75;
        private int maxX = 75;
        private int minY = 0;
        private int maxY = 0;
        private int minZ = -75;
        private int maxZ = 75;
        public GameObject gull;
        private Vector3 vel;
        private Vector3 acc;
        private float MaxSpeed = 30.0f;
        private float speed = 10.0f;
        public float boxSize = 100.0f;

        // Use this for initialization
        void Start()
        {
            gullVec = new GameObject[GullAmount];
            for (int i = 0; i < GullAmount; i++)
            {
                GameObject n = Instantiate(gull, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ)), Quaternion.identity);
                gullVec[i] = n;
            }
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 v1 = Vector3.zero;
            Vector3 v2 = Vector3.zero;
            Vector3 v3 = Vector3.zero;
            Vector3 v4 = Vector3.zero;
            for (int z = 0; z < GullAmount; z++)
            {
                v1 = CentreMass(gullVec[z]);
                v2 = Seperation(gullVec[z]);
                //v3 = VelocityMatch(gullVec[z]);
                //v4 = velLimit(gullVec[z]);
                acc = v1 + v2;
                //vel += acc * Time.deltaTime;
                //gullVec[z].transform.position += vel;
                gullVec[z].GetComponent<GullMovement>().acc = v1 + v2;
                velLimit(z, gullVec[z].GetComponent<GullMovement>().acc);
                acc = Vector3.zero;
            }
            for (int z = 0; z < GullAmount; z++)
            {
                //BoundingBox(z);
            }
        }

        Vector3 CentreMass(GameObject b)
        {
            Vector3 newPos = Vector3.zero;
            for (int z = 0; z < GullAmount; z++)
            {
                if (b.transform.position != gullVec[z].transform.position)
                {
                    newPos += b.transform.position;
                    //print(newPos);
                }
                else
                {
                    RandomMovement();
                }
            }
            return (newPos - b.transform.position) / 100;
        }
        Vector3 Seperation(GameObject b)
        {
            Vector3 c = Vector3.zero;
            for (int z = 0; z < GullAmount; z++)
            {
                if (b.transform.position != gullVec[z].transform.position)
                {
                    if (Vector3.Distance(b.transform.position, gullVec[z].transform.position) < 5.0f)
                    {
                        c += (gullVec[z].transform.position - b.transform.position);
                    }
                }
            }
            return c;
        }
        Vector3 VelocityMatch(GameObject b)
        {
            Vector3 vel = Vector3.zero;
            for (int z = 0; z < GullAmount; z++)
            {
                if (b.transform.position != gullVec[z].transform.position)
                {
                    acc += gullVec[z].GetComponent<GullMovement>().acc;
                }
            }
            return acc / 8;
        }
        void RandomMovement()
        {

        }


        void BoundingBox(int z)
        {


            //if (gullVec[z].transform.position.z >= maxZ)
            //{
            //    gullVec[z].transform.position = new Vector3(gullVec[z].transform.position.x, gullVec[z].transform.position.y, minZ);
            //}
            //else if (gullVec[z].transform.position.z <= minZ)
            //{
            //    gullVec[z].transform.position = new Vector3(gullVec[z].transform.position.x, gullVec[z].transform.position.y, maxZ);
            //}


            //if (gullVec[z].transform.position.x >= maxX)
            //{
            //    gullVec[z].transform.position = new Vector3(minX, gullVec[z].transform.position.y, gullVec[z].transform.position.z);
            //}
            //else if (gullVec[z].transform.position.x <= minX)
            //{
            //    gullVec[z].transform.position = new Vector3(maxX, gullVec[z].transform.position.y, gullVec[z].transform.position.z);
            //}

            //if (gullVec[z].transform.position.y >= maxY)
            //{
            //    gullVec[z].transform.position = new Vector3(gullVec[z].transform.position.x, minY, gullVec[z].transform.position.z);
            //}
            //else if (gullVec[z].transform.position.y <= minY)
            //{
            //    gullVec[z].transform.position = new Vector3(gullVec[z].transform.position.x, maxY, gullVec[z].transform.position.z);
            //}
        }


        void velLimit(int z, Vector3 vel)
        {
            Vector3 blop = Vector3.zero;
            if (vel.z > MaxSpeed)
            {
                blop.z = speed;
            }
            if (vel.y > MaxSpeed)
            {
                blop.y = speed;
            }
            if (vel.x > MaxSpeed)
            {
                blop.x = speed;
            }
            if (blop != Vector3.zero)
            {
                print("woodle");
                gullVec[z].GetComponent<GullMovement>().acc = (acc / speed);
            }
        }
    }
}
