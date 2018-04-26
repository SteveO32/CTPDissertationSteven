using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace HDev
{
    public class PackageManager : MonoBehaviour
    {
        public List<GameObject> packages            = new List<GameObject>();
        public Vector3 vehicleSpringJointOffset     = Vector3.zero;

        public Vector3 vehicleNorthContainerOffset  = Vector3.zero;
        public Vector3 vehicleSouthContainerOffset  = Vector3.zero;
        public Vector3 vehicleEastContainerOffset   = Vector3.zero;
        public Vector3 vehicleWestContainerOffset   = Vector3.zero;

        public float vehicleContainerHeight = 0.0f;

        [SerializeField]
        private float stackDebuff = 0.8f;    //the speed debuff applied by the packages

        public GameObject pack;

        private GameObject package;
        private SpringJoint spring;

        public Vector3 firstBoxOffset = new Vector3(0.0f, 1.45f, -0.5f);

        // Use this for initialization
        private void Start()
        {
            Initialize();
        }

        
        
        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                AddPackage(pack, false);
            else if (Input.GetKeyDown(KeyCode.L))
                DestroyPackage();
            else if (Input.GetKeyDown(KeyCode.I))
                AddPackage(pack, true);
            else if (Input.GetKeyDown(KeyCode.P))
                BankPackages();
        }



        //This initializes the car bounds and anchor points for the keeping the packages on the car
        private void Initialize()
        {
            Vector3 vehicleNorthContainerSize = Vector3.zero;
            Vector3 vehicleSouthContainerSize = Vector3.zero;
            Vector3 vehicleEastContainerSize = Vector3.zero;
            Vector3 vehicleWestContainerSize = Vector3.zero;

            vehicleNorthContainerSize.x = vehicleEastContainerOffset.x - vehicleWestContainerOffset.x;
            vehicleNorthContainerSize.y = vehicleContainerHeight;
            vehicleNorthContainerSize.z = 0.1f;

            vehicleSouthContainerSize.x = vehicleEastContainerOffset.x - vehicleWestContainerOffset.x;
            vehicleSouthContainerSize.y = vehicleContainerHeight;
            vehicleSouthContainerSize.z = 0.1f;

            vehicleEastContainerSize.x = 0.1f;
            vehicleEastContainerSize.y = vehicleContainerHeight;
            vehicleEastContainerSize.z = vehicleNorthContainerOffset.z - vehicleSouthContainerOffset.z;

            vehicleWestContainerSize.x = 0.1f;
            vehicleWestContainerSize.y = vehicleContainerHeight;
            vehicleWestContainerSize.z = vehicleNorthContainerOffset.z - vehicleSouthContainerOffset.z;

            for (int i = 0; i < 4; i++)
            {
                BoxCollider collider = this.gameObject.AddComponent<BoxCollider>();
                Vector3 containerSize = Vector3.zero;
                Vector3 containerOffset = Vector3.zero;

                switch (i)
                {
                    case 0:
                        containerSize = vehicleNorthContainerSize;
                        containerOffset = vehicleNorthContainerOffset;
                        containerOffset.y += (containerSize.y / 2) + 0.5f;
                        break;

                    case 1:
                        containerSize = vehicleSouthContainerSize;
                        containerOffset = vehicleSouthContainerOffset;
                        containerOffset.y += (containerSize.y / 2) + 0.5f;
                        break;

                    case 2:
                        containerSize = vehicleEastContainerSize;
                        containerOffset = vehicleEastContainerOffset;
                        containerOffset.y += (containerSize.y / 2) + 0.5f;
                        break;

                    case 3:
                        containerSize = vehicleWestContainerSize;
                        containerOffset = vehicleWestContainerOffset;
                        containerOffset.y += (containerSize.y / 2) + 0.5f;
                        break;

                }

                collider.center = containerOffset;
                collider.size = containerSize;
                collider.isTrigger = false;
            }
        }



        //Adds a package to the top of the car (the package is the gameobject passed through)
        public void AddPackage(GameObject go,bool isPackageStolen)
        {
 

            package = Instantiate(go, this.transform);


                
                Vector3 packagePosition = vehicleSpringJointOffset;
                packagePosition.y += 0.5f;
                package.transform.localPosition = packagePosition;

                Debug.Log(packagePosition);

                Vector3 packageAnchorPosition = package.transform.localPosition;
                packageAnchorPosition.y -= package.transform.localScale.y / 2;

                Rigidbody packageRB;

                if(!(packageRB = package.GetComponent<Rigidbody>()))
                {
                    packageRB = package.AddComponent<Rigidbody>();
                    packageRB.isKinematic = false;
                }

                spring = this.gameObject.AddComponent<SpringJoint>();
                spring.anchor = vehicleSpringJointOffset;
                spring.autoConfigureConnectedAnchor = false;
                spring.connectedAnchor = packageAnchorPosition;
                spring.spring = 100;
                spring.connectedBody = packageRB;
                spring.enableCollision = true;
            //}
            //else
            //{
            //    GameObject basePackage = packages[packages.Count - 1];

            //    package.transform.parent = basePackage.transform;

            //    Vector3 packagePosition = basePackage.transform.localPosition;
            //    packagePosition.y += basePackage.transform.localScale.y + 0.5f;

            //    Vector3 packageAnchorPosition = package.transform.localPosition;
            //    packageAnchorPosition.y -= package.transform.localScale.y;

            //    Vector3 baseAnchorPosition = basePackage.transform.localPosition;
            //    baseAnchorPosition.y += basePackage.transform.localScale.y;

            //    Rigidbody packageRB;

            //    if (!(packageRB = package.GetComponent<Rigidbody>()))
            //    {
            //        packageRB = package.AddComponent<Rigidbody>();
            //        packageRB.isKinematic = false;
            //    }

            //    SpringJoint spring = basePackage.AddComponent<SpringJoint>();
            //    spring.anchor = baseAnchorPosition;
            //    spring.autoConfigureConnectedAnchor = false;
            //    spring.connectedAnchor = packageAnchorPosition;
            //    spring.spring = 50;
            //    spring.connectedBody = packageRB;

            //}

            if (isPackageStolen == true)
            {
                package.GetComponent<HDev_Package>().SetStolen(true);
            }
            else if (isPackageStolen == false)
            {
                package.GetComponent<HDev_Package>().SetStolen(false);
            }


            packages.Add(package);
            //GetComponent<Kojima.CarScript>().m_baseCarInfo.m_maxSpeed *= stackDebuff;
            //GetComponent<Kojima.CarScript>().m_baseCarInfo.m_acceleration *= stackDebuff;
        }



        //public void AddStaticPackage(GameObject package)
        //{
        //    if(packages.Count <= 0)
        //    {
        //        package.transform.parent = this.transform;
        //        package.transform.localPosition = firstBoxOffset;
        //        packages.Add(package);
        //    }
            
        //}

        //used to bank the score for the packages delivered
        public void BankPackages()
        {
            int scoreCount = 0;
            int stackSize = packages.Count;
            Debug.Log("stack size");

            int numberStolen = 0;
            int numberNotStolen = 0;
            for (int i = 0; i < stackSize; i++)
            {
                if (packages[i].GetComponent<HDev_Package>().GetStolen() == true)
                {
                    numberStolen++;
                }
                else
                {
                    numberNotStolen++;
                }

            }

            //remove all packages from the car
            for(int i = 0; i < stackSize; i++)
            {
                DestroyPackage();
                scoreCount++;
            }
            //+1 point for each package removed
            //GetComponentInChildren<HDev_ScoreCalculation>().AddToScoreTotal(scoreCount);
            GetComponentInChildren<HDev_ScoreCalculation>().CalculateScore(numberStolen,numberNotStolen);
        }

        public void DestroyPackage()
        {
            spring.Destroy();
            GameObject temp = packages[0];
            packages.Remove(temp);
            temp.Destroy();
            //GetComponent<Kojima.CarScript>().m_baseCarInfo.m_maxSpeed /= stackDebuff;
            //GetComponent<Kojima.CarScript>().m_baseCarInfo.m_acceleration /= stackDebuff;
        }
    }
}