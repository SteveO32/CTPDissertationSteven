using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Josh Fenlon
// Purpose:		functions that splits up meshes/objects into a spatial partion
// Note: 		There are a few hard coded values in here
// Namespace:	GG
//
//============================================================================//

namespace GG
{

    public class DeformableMeshWithSpatialPartioning : MonoBehaviour
    {
		[SerializeField]
        private bool getChildren = false;
		[SerializeField]
		private List<GameObject> objectToAddToList = new List<GameObject>();
		[SerializeField]
		private List<buildingData> objectData = new List<buildingData>();
		[SerializeField]
		private List<List<Vector3>> verts = new List<List<Vector3>>();
		[SerializeField]
		private List<MeshCollider> meshCols = new List<MeshCollider>();
		[SerializeField]
		private List<MeshFilter> meshFilters = new List<MeshFilter>();

		[SerializeField]
		private int sectionsSize = 32;

		[SerializeField]
		private bool debugChunks = false;
		[SerializeField]
		private float debugVisualSize = 1;
		[SerializeField]
		private bool debugMeshes = false;

		[SerializeField]
		private bool expandNotContract = false;

		[SerializeField]
		private Vector3 minPos = Vector3.zero;
		[SerializeField]
		private Vector3 maxPos = Vector3.zero;
		[SerializeField]
		private Vector3 actualSize = Vector3.zero;
		[SerializeField]
		private vec2I sectionCount = vec2I.zero();
		[SerializeField]
		private List<destroyPointData> destroyPoint = new List<destroyPointData>();

		[SerializeField]
		private float checkRange = 10;
		[SerializeField]
		private float nextRangeUpgrade = 1.25f;
		[SerializeField]
		private float maxRange = 10;
        [Range(0, 100)]
		[SerializeField]
		private float buildingDrop = -10;
        [Range(0, 100)]
		[SerializeField]
		private float buildingDeform = 10;
		[SerializeField]
		private float islandDrop = -0.25f;

        IEnumerator worker = null;

		[SerializeField]
		private List<spatialSection> sections;

        [System.Serializable]
        public class spatialSection
        {
            public List<vertexData> vertexDataInSection = new List<vertexData>();
        }

        [System.Serializable]
        public class buildingData
        {
            public Vector3 center = Vector3.zero;
            public Vector3 bottom = Vector3.zero;
            public List<int> sectionsWithin = new List<int>();

            public buildingData(Vector3 _center, Vector3 _bottom)
            {
                center = _center;
                bottom = _bottom;
            }
        }

        [System.Serializable]
        public class vertexData
        {
            public int objectIndex = -1;
            public int vertexIndex = -1;

            public vertexData(int obj, int vert)
            {
                objectIndex = obj;
                vertexIndex = vert;
            }
        }

        [System.Serializable]
        public class destroyPointData
        {
            public Vector3 pos;
			public float explosionVelocity;
            public float range;
            public float maxRange;

			public destroyPointData(Vector3 _pos, float _range, float _maxRange, float _explosionVelocity = 0)
			{
				pos = _pos;
				range = _range;
				maxRange = _maxRange;
				explosionVelocity = _explosionVelocity;
			}
        }

		//find min max of objects positions
        void setupObjects()
        {

            //search all objects and find the minimum and maximum 
            foreach (GameObject obj in objectToAddToList)
            {
                if (!obj.GetComponent<MeshCollider>())
                {
                    obj.AddComponent<MeshCollider>();
                }
                if (obj.GetComponent<MeshCollider>())
                {
                    Vector3 minBounds = obj.GetComponent<MeshCollider>().bounds.min;
                    Vector3 maxBounds = obj.GetComponent<MeshCollider>().bounds.max;

                    meshCols.Add(obj.GetComponent<MeshCollider>());
                    meshFilters.Add(obj.GetComponent<MeshFilter>());

                    Vector3 center = meshCols[meshCols.Count - 1].bounds.center;
                    Vector3 bottom = meshCols[meshCols.Count - 1].bounds.min;
                    //bottom = obj.transform.InverseTransformPoint (bottom);

                    //objectData.Add (new buildingData (obj.transform.InverseTransformPoint(center), obj.transform.InverseTransformPoint(bottom)));
                    objectData.Add(new buildingData(center, bottom));

                    //check if any point is lower than current minPos
                    if (minBounds.x < minPos.x)
                    {
                        minPos.x = minBounds.x;
                    }

                    if (minBounds.y < minPos.y)
                    {
                        minPos.y = minBounds.y;
                    }

                    if (minBounds.z < minPos.z)
                    {
                        minPos.z = minBounds.z;
                    }


                    //check if any point is highest than current maxPos
                    if (maxBounds.x > maxPos.x)
                    {
                        maxPos.x = maxBounds.x;
                    }

                    if (maxBounds.y > maxPos.y)
                    {
                        maxPos.y = maxBounds.y;
                    }

                    if (maxBounds.z > maxPos.z)
                    {
                        maxPos.z = maxBounds.z;
                    }

                }
            }

            actualSize = maxPos - minPos;

            sectionCount.x = Mathf.CeilToInt(actualSize.x / sectionsSize);
            sectionCount.y = Mathf.CeilToInt(actualSize.z / sectionsSize);

        }

        // Use this for initialization
        void Start()
        {
            //Get all children of object
            if (getChildren)
            {
                foreach (Transform tr in gameObject.GetComponentsInChildren<Transform>())
                {
                    if (tr != transform)
                    {
						if (tr.GetComponent<MeshFilter> ())
						{
							objectToAddToList.Add (tr.gameObject);
						}
                    }
                }
            }

            //setup min, max position
            if (objectToAddToList.Count != 0)
            {
                bool delete = false;
                if (!objectToAddToList[0].GetComponent<MeshCollider>())
                {
                    objectToAddToList[0].AddComponent<MeshCollider>();
                    delete = true;
                }
                minPos = objectToAddToList[0].GetComponent<MeshCollider>().bounds.min;
                maxPos = objectToAddToList[0].GetComponent<MeshCollider>().bounds.max;
                if (delete)
                {
                    DestroyImmediate(objectToAddToList[0].GetComponent<MeshCollider>());
                }
            }

			//setup the grid
            setupObjects();
            verts = getMeshData();
            partitioning();
        }

		//setup the grid sections and partitions
        void partitioning()
        {
            sections.Clear();

			//add all sections needed to list
            for (int a = 0; a < sectionCount.x * sectionCount.y; a++)
            {
                sections.Add(new spatialSection());
            }

            //setup spatial partitioning for grid
            for (int a = 0; a < objectToAddToList.Count; a++)
            {
                partitionObject(a);
            }
        }

		//add object vertices to grid
        void partitionObject(int a)
        {
            if (objectToAddToList[a] != null)
            {
                MeshFilter temp = objectToAddToList[a].GetComponent<MeshFilter>();
                if (temp != null)
                {
					//check if mesh data is readable
                    if (temp.mesh.isReadable)
                    {
						//loop through all vertices within the mesh
                        for (int b = 0; b < verts[a].Count; b++)
                        {
							//transform vertex position into world position
                            Vector3 vertToWorld = objectToAddToList[a].transform.TransformPoint(verts[a][b]);
							//get index within section
                            int index = posToIndex(vertToWorld);
                            if (index >= 0)
                            {
								//check if object is already within section
                                if (!objectData[a].sectionsWithin.Contains(index))
                                {
                                    objectData[a].sectionsWithin.Add(index);
                                }
                                sections[index].vertexDataInSection.Add(new vertexData(a, b));
                            }
                        }
                    }
                }
            }
        }

		//completely reset all grid object data
        void resetObjects()
        {
            meshCols.Clear();
            meshFilters.Clear();
            verts.Clear();
            objectData.Clear();
        }

		//remove an object and its data from the grid 
        void removeObject(int a)
        {
			//loop through each section the object is within
            foreach (int i in objectData[a].sectionsWithin)
            {
				//loop through each index within vertice list
                for (int index = 0; index < sections[i].vertexDataInSection.Count;)
                {
					//check if the vertex is owned by the object supplied
                    if (sections[i].vertexDataInSection[index].objectIndex == a)
                    {
						//remove the vertex from the list
                        sections[i].vertexDataInSection.RemoveAt(index);
                    }
                    else
                    {
                        index++;
                    }
                }
            }
        }

        void Update()
        {
            if (worker == null)
            {
				//check if point needs to be destroyed
                if (destroyPoint.Count != 0)
                {
					//setup enumerator to destroy/deform mesh
					Debug.DrawLine(destroyPoint[0].pos, destroyPoint[0].pos + new Vector3(0,10,0), Color.red, 2);
					worker = DeformVerts(destroyPoint[0].pos, destroyPoint[0].range, destroyPoint[0].maxRange, destroyPoint[0].explosionVelocity);
                    StartCoroutine(worker);
                    destroyPoint.RemoveAt(0);
                }
            }
        }

        //Grab the closest vertex position
        public List<Vector3> getClosestVerPos(Vector3 hit, float range, int findCount = int.MaxValue)
        {

            List<Vector3> verticies = new List<Vector3>();

            foreach (vec2I vec2 in getClosestVert(hit, range, findCount))
            {
                verticies.Add(objectToAddToList[vec2.x].transform.TransformPoint(verts[vec2.x][vec2.y]));
            }

            return verticies;

        }

        //Get the position of vector3 into index of verticie array
        int posToIndex(Vector3 hit)
        {
            float actualX = hit.x - minPos.x;
            float actualZ = hit.z - minPos.z;

            if (actualX < 0 || actualZ < 0)
            {
                return -1;
            }

            int xIndex = (int)(actualX / sectionsSize);
            int zIndex = (int)(actualZ / sectionsSize);

            if (xIndex >= sectionCount.x || zIndex >= sectionCount.y)
            {
                return -1;
            }

            return (xIndex * sectionCount.y) + zIndex;
        }

		//add a default destroy points
		public void addDestroyPoint(Vector3 pos, float range, float _maxRange, float explosionVelocity = 0)
		{
			destroyPoint.Add (new destroyPointData (pos, range, _maxRange, explosionVelocity));
		}

        //Grab the closest vertex index
        List<vec2I> getClosestVert(Vector3 hit, float range, int findCount = int.MaxValue)
        {

            List<vec2I> withinRange = new List<vec2I>();
            List<int> indicies = new List<int>();

            //find all possible sections
            for (float a = -range; a < range; a += sectionsSize)
            {
                for (float b = -range; b < range; b += sectionsSize)
                {
                    int i = posToIndex(hit + new Vector3(a, 0, b));
                    if (i >= 0)
                    {
                        if (!indicies.Contains(i))
                        {
                            indicies.Add(i);
                        }
                    }
                }
            }

            //loop through all sections
            for (int a = 0; a < indicies.Count; a++)
            {
                int index = indicies[a];
                //check if section exists
                //loop through all sections verts
                foreach (vertexData vec in sections[index].vertexDataInSection)
                {
                    if (objectToAddToList[vec.objectIndex] != null)
                    {
                        if (Vector3.Distance(hit, objectToAddToList[vec.objectIndex].transform.TransformPoint(verts[vec.objectIndex][vec.vertexIndex])) < range)
                        {
                            withinRange.Add(new vec2I(vec.objectIndex, vec.vertexIndex));
                        }
                    }
                }
            }

            List<vec2I> returnVal = new List<vec2I>();

            bool sort = true;
            float dist1 = 0;
            float dist2 = 0;

            while (sort)
            {
                sort = false;
                for (int a = 0; a < withinRange.Count - 1; a++)
                {
                    dist1 = verts[withinRange[a].x][withinRange[a].y].sqrMagnitude;
                    dist2 = verts[withinRange[a + 1].x][withinRange[a + 1].y].sqrMagnitude;
                    if (dist1 > dist2)
                    {
                        sort = true;
                        vec2I temp = withinRange[a + 1];
                        withinRange[a + 1] = withinRange[a];
                        withinRange[a] = temp;
                    }
                }
            }

            for (int a = 0; a < findCount; a++)
            {
                if (a < withinRange.Count)
                {
                    returnVal.Add(withinRange[a]);
                }
                else
                {
                    break;
                }
            }

            return returnVal;

        }

		//returns all of the mesh vertex positions in objects within grid
        List<List<Vector3>> getMeshData()
        {
			//setup new vertex list
            List<List<Vector3>> vertices = new List<List<Vector3>>();
            foreach (GameObject obj in objectToAddToList)
            {
				//check if the object contains a mesh filter
                if (!obj.GetComponent<MeshFilter>())
                {
                    Debug.LogError("Object added to deformable mesh does not have a mesh: " + obj.name);
                    break;
                }
                else
                {
					//set vertex list a last index to mesh vertices
                    vertices.Add(new List<Vector3>());
                    obj.GetComponent<MeshFilter>().mesh.GetVertices(vertices[vertices.Count - 1]);
                }
            }
            return vertices;
        }

		//detaches parts of buildings and creates a new object/mesh
		void FragmentMesh(Vector3 pos, float range, float _maxRange) 
		{

			List<int> indexes = new List<int>();

			//add current pos to check
			int i = posToIndex(pos);
			if (i >= 0)
			{
				if (!indexes.Contains(i))
				{
					indexes.Add(i);
				}
			}

			//find all possible sections within range
			for (float a = -range; a < range; a += sectionsSize)
			{
				for (float b = -range; b < range; b += sectionsSize)
				{
					float dist = Vector2.Distance(new Vector2(a, b), Vector2.zero);
					if (dist <= range)
					{
						//check if pos is within sections
						i = posToIndex(pos + new Vector3(a, 0, b));
						if (i >= 0)
						{
							if (!indexes.Contains(i))
							{
								indexes.Add(i);
							}
						}
					}
				}
			}

			//loop through all sections
			for (int a = 0; a < indexes.Count; a++) 
			{
				int index = indexes[a];
				//check if section exists
				//loop through all sections verts
				foreach (vertexData vec in sections[index].vertexDataInSection)
				{



				}
			}

		}

        //deforms verts at point
		IEnumerator DeformVerts(Vector3 pos, float range, float _maxRange, float explosionVelocity)
        {
			//add bool for each collider to check if mesh needs updating
            List<bool> updateCollider = new List<bool>();
            foreach (GameObject obj in objectToAddToList)
            {
                updateCollider.Add(false);
            }

            List<int> indexes = new List<int>();

            //add current pos to check
            int i = posToIndex(pos);
            if (i >= 0)
            {
                if (!indexes.Contains(i))
                {
                    indexes.Add(i);
                }
            }

            //find all possible sections within range
            for (float a = -range; a < range; a += sectionsSize)
            {
                for (float b = -range; b < range; b += sectionsSize)
                {
                    float dist = Vector2.Distance(new Vector2(a, b), Vector2.zero);
                    if (dist <= range)
                    {
						//check if pos is within sections
                        i = posToIndex(pos + new Vector3(a, 0, b));
                        if (i >= 0)
                        {
                            if (!indexes.Contains(i))
                            {
                                indexes.Add(i);
                            }
                        }
                    }
                }
            }

            bool destroyedOne = false;

            //loop through all sections
            for (int a = 0; a < indexes.Count; a++)
            {
                int index = indexes[a];
                //check if section exists
                //loop through all sections verts
                foreach (vertexData vec in sections[index].vertexDataInSection)
                {
                    if (objectToAddToList[vec.objectIndex] != null)
                    {
						//get vertex pos in world space
						Vector3 newPos = objectToAddToList[vec.objectIndex].transform.TransformPoint(verts[vec.objectIndex][vec.vertexIndex]);
                        float distance = Vector3.Distance(newPos, pos);
                        if (distance <= range)
                        {
                            //add a check to reset partition if different vertPos

							//check if deforming mesh toward center
							if (explosionVelocity == 0) {

								//check if object is a building
								if (objectToAddToList [vec.objectIndex].transform.tag == "Building") {

									//calculate change values
									//only apply horizontal movement based on building deform value
									Vector3 centDir = (objectData [vec.objectIndex].center - newPos);
									Vector3 fwdDir = Functions.vec3Times (Vector3.forward, centDir);
									Vector3 sideDir = Functions.vec3Times (Vector3.right, centDir);

									//only apply vertical movement based on building drop value
									Vector3 botDir = (objectData [vec.objectIndex].bottom - newPos);
									Vector3 botWorldDir = Functions.vec3Times (Vector3.up, botDir);

									if (expandNotContract) {
										newPos -= (fwdDir + sideDir) * (buildingDeform / 100) * ((range - distance) / range);
										newPos -= botWorldDir * (buildingDrop / 100) * ((range - distance) / range);

									} else {
										newPos += (fwdDir + sideDir) * (buildingDeform / 100) * ((range - distance) / range);
										newPos += botWorldDir * (buildingDrop / 100) * ((range - distance) / range);

									}
								} else {

									//only apply vertical movement based on island drop value
									newPos -= Vector3.down * islandDrop;

								}

							} else {

								//if hit object has a velocity move away from it
								//add some maths here to do that

							}

                            verts[vec.objectIndex][vec.vertexIndex] = objectToAddToList[vec.objectIndex].transform.InverseTransformPoint(newPos);
                            destroyedOne = true;
                            updateCollider[vec.objectIndex] = true;

                        }
                    }
                }
            }

            if (destroyedOne == false && range <= _maxRange)
            {
                destroyPoint.Add(new destroyPointData(pos, range * nextRangeUpgrade, _maxRange));
            }

            //update mesh and mesh collider
            for (int a = 0; a < objectToAddToList.Count; a++)
            {
                if (updateCollider[a])
                {
                    meshFilters[a].mesh.SetVertices(verts[a]);
                    //reacalc normals = 100x more expensive but is cheap
                    meshFilters[a].mesh.RecalculateNormals();
					meshCols [a].sharedMesh = null;
                    if (objectToAddToList[a].transform.TransformPoint(meshFilters[a].mesh.bounds.size).magnitude > 1)
                    {
						meshCols [a].sharedMesh = meshFilters[a].mesh;
                    }
                    else
                    {
                        clearSectionsOfVerts(a);
					}

					yield return null;
                }
            }

            worker = null;
        }

        void OnDrawGizmos()
        {
            if (debugChunks)
            {
                for (int a = 0; a < sectionCount.x; a++)
                {
                    for (int b = 0; b < sectionCount.y; b++)
                    {
                        if (sections[(int)(a * sectionCount.y) + b].vertexDataInSection.Count != 0)
                        {
                            Gizmos.color = Color.green;
                            Gizmos.DrawWireCube(minPos + new Vector3(a * sectionsSize + (sectionsSize / 2), 100, b * sectionsSize + (sectionsSize / 2)), new Vector3(sectionsSize, 300, sectionsSize));
                            Gizmos.color = Color.white;
                        }
                        else
                        {
                            Gizmos.DrawWireCube(minPos + new Vector3(a * sectionsSize + (sectionsSize / 2), 100, b * sectionsSize + (sectionsSize / 2)), new Vector3(sectionsSize, 300, sectionsSize));
                        }
                    }
                }
                foreach (GameObject obj in objectToAddToList)
                {
                    Gizmos.DrawSphere(obj.transform.position, debugVisualSize);
                }
                Gizmos.color = Color.red;
                Gizmos.DrawLine(minPos, maxPos);
                Gizmos.DrawSphere(minPos, debugVisualSize);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(maxPos, debugVisualSize);

            }

            if (debugMeshes)
            {

                for (int a = 0; a < meshFilters.Count; a++)
                {
                    for (int subMesh = 0; subMesh < meshFilters[a].mesh.subMeshCount; subMesh++)
                    {
                        int colorOffset = 0;
                        for (int subMeshOffset = 0; subMeshOffset < subMesh; subMeshOffset++)
                        {
                            colorOffset += meshFilters[a].mesh.GetTriangles(subMeshOffset).Length / 3;
                        }
						int[] tri = meshFilters[a].mesh.GetTriangles(subMesh);
						Gizmos.color = Color.white;
                        for (int b = 0; b < tri.Length; b += 3)
                        {
                            Gizmos.DrawLine(objectToAddToList[a].transform.TransformPoint(verts[a][tri[b]]), objectToAddToList[a].transform.TransformPoint(verts[a][tri[b + 1]]));
                            Gizmos.DrawLine(objectToAddToList[a].transform.TransformPoint(verts[a][tri[b + 1]]), objectToAddToList[a].transform.TransformPoint(verts[a][tri[b + 2]]));
                            Gizmos.DrawLine(objectToAddToList[a].transform.TransformPoint(verts[a][tri[b + 2]]), objectToAddToList[a].transform.TransformPoint(verts[a][tri[b]]));
                        }
                    }
                }

            }
        }

		//loop through all sections that object at objID and remove verts
		void clearSectionsOfVerts(int objectID)
		{
			//loop through each section
			for (int a = 0; a < sectionCount.x; a++)
			{
				for (int b = 0; b < sectionCount.y; b++)
				{
					//loop through all vertices within section
					for (int index = 0; index < sections[(a * sectionCount.y) + b].vertexDataInSection.Count;)
					{
						if (sections[(a * sectionCount.y) + b].vertexDataInSection[index].objectIndex == objectID)
						{
							//if vertex at index is owned by object delete it
							sections[(a * sectionCount.y) + b].vertexDataInSection.RemoveAt(index);
						}
						else
						{
							index++;
						}
					}
				}
			}
		}

		//loops through objects and then resets their vertice positions
		//within the spatial partion grid, also updates grid if needed
		public void resetObjectSections(List<GameObject> objects)
		{
			bool regeneratePartitons = false;
			//loop through each object
			foreach (GameObject obj in objects)
			{
				//check if object currently exists
				if (!objectToAddToList.Contains(obj))
				{
					//if not regenerate the grid
					objectToAddToList.Add(obj);
					regeneratePartitons = true;
				}
			}
			//check if grid needs to be regenerated
			if (regeneratePartitons)
			{
				//reset the grid
				resetObjects();
				//create the grid objects
				setupObjects();
				//create local verts variable
				verts = getMeshData();
				//create the grid
				partitioning();
			}
			else
			{
				//loop through each object
				foreach (GameObject obj in objects)
				{
					//loop through each object in grid
					for (int a = 0; a < objectToAddToList.Count; a++)
					{
						//check if current object at index is equal to obj
						if (objectToAddToList[a] == obj)
						{
							//reset the object
							removeObject(a);
							//clear the current objects local verts
							verts[a].Clear();
							//setup the current objects local verts
							meshFilters[a].mesh.GetVertices(verts[a]);
							//repartition the object
							partitionObject(a);
							break;
						}
					}
				}
			}
		}






    }

}