using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Gualtiero Vercellotti 
// Purpose:		Animator Manager for Human Physics Based characters. 
// Namespace:	LT
//
//===============================================================================//

namespace LT
{ 
    [RequireComponent(typeof(HumanCharacterControl))]
    public class HumanCharacterAnimator : MonoBehaviour
    {
        //HumanCharacterControl hcc;

        public Hash_IDs hashIDs;

        //public float animatorSpeed = 1.3f; // Read by RagdollControl

        [SerializeField]
        Animator anim;

        [SerializeField]
        float speedDampTime = .1f;   // The damping for the speed parameter


        void Awake()
        {
            //hcc = GetComponent<HumanCharacterControl>();
            //hashIDs = new Hash_IDs();
            hashIDs.Awake();
        }

        void Start()
        {
            
        }

        void Update()
        {
            
        }

        public void updateSpeedMultiplier(float speedMult)
        {
            anim.SetFloat(hashIDs.speedMultFloat, speedMult);
        }

        public void Walk(float speed)
        {
            anim.SetFloat(hashIDs.speedFloat, speed, speedDampTime, Time.fixedDeltaTime);
        }

        public void StopWalking()
        {
            anim.SetFloat(hashIDs.speedFloat, 0, 0, Time.fixedDeltaTime);
        }

        public void Swing()
        {
            anim.SetTrigger(hashIDs.swing);
        }

        public void PickUpRight()
        {
            anim.SetBool(hashIDs.holdingRight, true);
        }

        public void DropRight()
        {
            anim.SetBool(hashIDs.holdingRight, false);
        }

        public void Pirouette()
        {
            anim.SetTrigger(hashIDs.pirouette);
        }

        public void Jump()
        {
            anim.SetTrigger(hashIDs.jump);
        }

    }


    [System.Serializable]
    public class Hash_IDs
    {
        // Here we store the hash tags for various strings used in our animators.
        public int swing;
        public int holdingRight;

        public int speedMultFloat;
        public int pirouette;
        public int jump;

        public int dyingState;
        public int locomotionState;
        public int deadBool;
        public int speedFloat;

        public int frontTrigger;
        public int backTrigger;
        public int frontMirrorTrigger;
        public int backMirrorTrigger;

        public int idle;

        public int getupFront;
        public int getupBack;
        public int getupFrontMirror;
        public int getupBackMirror;

        public int anyStateToGetupFront;
        public int anyStateToGetupBack;
        public int anyStateToGetupFrontMirror;
        public int anyStateToGetupBackMirror;

        public void Awake()
        {
            swing = Animator.StringToHash("Swing");
            holdingRight = Animator.StringToHash("HoldingRight");

            jump = Animator.StringToHash("JumpEmote");

            speedMultFloat = Animator.StringToHash("SpeedMultiplier");
            pirouette = Animator.StringToHash("Pirouette");

            dyingState = Animator.StringToHash("Base Layer.Dying");
            locomotionState = Animator.StringToHash("Base Layer.Locomotion");
            deadBool = Animator.StringToHash("Dead");

            idle = Animator.StringToHash("Base Layer.Idle");

            // These are used by the RagdollControll script and must exist exactly as below
            speedFloat = Animator.StringToHash("Speed");

            frontTrigger = Animator.StringToHash("FrontTrigger");
            backTrigger = Animator.StringToHash("BackTrigger");
            frontMirrorTrigger = Animator.StringToHash("FrontMirrorTrigger");
            backMirrorTrigger = Animator.StringToHash("BackMirrorTrigger");

            getupFront = Animator.StringToHash("Base Layer.GetupFront");
            getupBack = Animator.StringToHash("Base Layer.GetupBack");
            getupFrontMirror = Animator.StringToHash("Base Layer.GetupFronMirror");
            getupBackMirror = Animator.StringToHash("Base Layer.GetupBackMirror");

            anyStateToGetupFront = Animator.StringToHash("Entry -> Base Layer.GetupFront");
            anyStateToGetupBack = Animator.StringToHash("Entry -> Base Layer.GetupBack");
            anyStateToGetupFrontMirror = Animator.StringToHash("Entry -> Base Layer.GetupFrontMirror");
            anyStateToGetupBackMirror = Animator.StringToHash("Entry -> Base Layer.GetupBackMirror");
        }
    }

}
