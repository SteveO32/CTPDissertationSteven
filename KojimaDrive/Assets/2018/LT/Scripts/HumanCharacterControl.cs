using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Gualtiero Vercellotti 
// Purpose:		Main controller for Human Physics Based characters. 
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public enum PickUpSide
    {
        Left,
        Right
    }

    public class HumanCharacterControl : MonoBehaviour
    {
        public int playerId;

        public float speedMultiplier = 1.0f;


        //--- Animator and Input ---
        //HumanCharacterInput input;
        HumanCharacterAnimator anim;

        //--- Animated Character --- 

        //--- Ragdoll Character ---
        [SerializeField]
        Transform leftHand;
        [SerializeField]
        Transform rightHand;


        Transform holdingLeft;
        Transform holdingRight;

        //--- AnimFollow Links ---
        AnimFollow.RagdollControl_AF ragdollControl;
        AnimFollow.PlayerMovement_AF animFollowPlayerMovement;

        public bool inhibitMove = false; // Set from RagdollControl
        [HideInInspector] public bool inhibitRun = false; // Set from RagdollControl

        [SerializeField]
        SkinnedMeshRenderer mesh;
        [SerializeField]
        SkinnedMeshRenderer meshRB;

        [SerializeField]
        Transform animRoot;

        void Awake()
        {
            //input = GetComponent<HumanCharacterInput>();
            anim = GetComponent<HumanCharacterAnimator>();
            ragdollControl = GetComponentInChildren<AnimFollow.RagdollControl_AF>();
            //Set Hash ids to ragdoll control AnimFollow
            ragdollControl.hash = anim.hashIDs;
            animFollowPlayerMovement = GetComponentInChildren<AnimFollow.PlayerMovement_AF>();
        }

        void Start()
        {

        }

        void Update()
        {
            transform.position += animFollowPlayerMovement.transformMovement;
            anim.updateSpeedMultiplier(speedMultiplier);

            //if (ragdollControl.transform.localPosition.magnitude > 0.1f)
            //{
            //    Vector3 pos = ragdollControl.transform.localPosition;
             
            //    transform.position += pos;

            //    ragdollControl.transform.localPosition = Vector3.zero;
            //}

            if (Vector3.Distance(ragdollControl.ragdollRootBone.transform.position, animRoot.position) > 0.1f)
            {
                Vector3 pos = animRoot.position - ragdollControl.ragdollRootBone.transform.position;

                pos.y = 0;

                transform.position -= pos;

                ragdollControl.ragdollRootBone.transform.position += pos;
            }

            //Make sure the parent doesn't fall behind when the characher ragdolls
            //if (animFollowPlayerMovement.transform.localPosition.magnitude > 0.01f)
            //{
            //    Vector3 pos = animFollowPlayerMovement.transform.localPosition;
            //    pos.x = 0;
            //    pos.z = 0;
            //    pos.y = 0;

            //    transform.position += pos;


            //    animFollowPlayerMovement.transform.localPosition = pos;

            //}
        }

        private void OnAnimatorMove()
        {
            
        }

        public void UpdateCharacter(Mesh newMesh, Material newMat)
        {
            mesh.sharedMesh = newMesh;
            mesh.material = newMat;

            meshRB.sharedMesh = newMesh;
            meshRB.material = newMat;
        }

        public void PickUp(PickUpSide side, GameObject obj)
        {
            if (side == PickUpSide.Right)
            {
                anim.PickUpRight();

                obj.transform.parent = rightHand.transform;

                DisableCharacterCollisionsWithObject(obj);

                holdingRight = obj.transform;
                holdingRight.position = rightHand.position;
                holdingRight.localRotation = Quaternion.identity;
                holdingRight.GetComponent<Rigidbody>().isKinematic = true;
                obj.GetComponent<CollectibleHandWeapon>().Collect(this.gameObject);
            }
            else if (side == PickUpSide.Left)
            {
                //anim.PickUpRight();
                obj.transform.parent = leftHand;
                holdingRight = obj.transform;
            }
        }

        public void DisableCharacterCollisionsWithObject(GameObject coll)
        {
            foreach (Collider c in GetComponentsInChildren<Collider>())
            {
                foreach (Collider c2 in coll.GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(c2, c, true);
                }
            }
        }

        public void EnableCharacterCollisionsWithObject(GameObject coll)
        {
            foreach (Collider c in GetComponentsInChildren<Collider>())
            {
                foreach (Collider c2 in coll.GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(c2, c, false);
                }
            }
        }

        public void Drop(PickUpSide side)
        {
            if (side == PickUpSide.Left)
            {
                holdingLeft.GetComponent<CollectibleHandWeapon>().Drop();
                EnableCharacterCollisionsWithObject(holdingLeft.gameObject);
                holdingLeft.transform.parent = null;
                holdingLeft = null;
            }
            else
            {
                holdingRight.GetComponent<CollectibleHandWeapon>().Drop();
                EnableCharacterCollisionsWithObject(holdingRight.gameObject);

                holdingRight.transform.parent = null;
                holdingRight = null;
            }
        }

        //Move the character in direction
        public void Move(Vector2 dir, InputMode mode, GameObject camera = null)
        {
            switch (mode)
            {
                case InputMode.CharacterRelative:
                    animFollowPlayerMovement.transform.Rotate(0f, dir.x * 100 * Time.fixedDeltaTime, 0f);

                    if (dir.y >= .1f)
                    {
                        anim.Walk(5.5f * speedMultiplier);
                    }
                    else if (dir.y <= -.1f)
                    {
                        anim.Walk(-3f * speedMultiplier);
                    }
                    else
                        anim.StopWalking();
                    break;
                case InputMode.CameraRelative:
                    Vector3 cameraRelativeDirectionMovement = ((camera.transform.forward * dir.y) + (camera.transform.right * dir.x));
                    cameraRelativeDirectionMovement.y = 0;
                    cameraRelativeDirectionMovement.Normalize();
                    animFollowPlayerMovement.transform.LookAt(animFollowPlayerMovement.transform.position + cameraRelativeDirectionMovement);
                    if (dir.magnitude >= .1f)
                    {
                        anim.Walk(5.5f * speedMultiplier);
                    }
                    else
                    {
                        anim.StopWalking();
                    }
                    break;
                case InputMode.WorldRelative:
                    animFollowPlayerMovement.transform.LookAt(new Vector3(animFollowPlayerMovement.transform.position.x + dir.x, animFollowPlayerMovement.transform.position.y, animFollowPlayerMovement.transform.position.z + dir.y));
                    if (dir.magnitude >= .1f)
                    {
                        anim.Walk(5.5f * speedMultiplier);
                    }
                    else
                    {
                        anim.StopWalking();
                    }
                    break;
            }
            
            
        }

        public void Punch(bool left)
        {

        }

        public void Swing()
        {
            anim.Swing();
        }

        public void Pirouette()
        {
            anim.Pirouette();
        }

        public void GetUp(bool button)
        {
            if (ragdollControl.gettingUp)
            {
                if (gameObject.GetComponentInChildren<UIManager>() != null)
                    gameObject.GetComponentInChildren<UIManager>().warningText_reference.ActivateWarning();
                ragdollControl.getUpComand = button;
            }
            else
            {
                if (gameObject.GetComponentInChildren<UIManager>() != null)
                    gameObject.GetComponentInChildren<UIManager>().warningText_reference.DeactivateWarning();
            }
        }

        public void Emote(int emoteNum)
        {
            switch (emoteNum)

            {
                case 1:
                    anim.Pirouette();
                    break;
                case 2:
                    anim.Jump();
                    break;

            }

        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<CollectibleHandWeapon>() != null)
            {
                if (holdingRight == null && !other.GetComponent<CollectibleHandWeapon>().taken)
                { 
                    PickUp(PickUpSide.Right, other.gameObject);
                }
                else if (!other.GetComponent<CollectibleHandWeapon>().taken)
                {
                    Drop(PickUpSide.Right);
                    PickUp(PickUpSide.Right, other.gameObject);
                }
            }
        }
    }
}
