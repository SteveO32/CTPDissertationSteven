using System;
using UnityEngine;
using UnityEngine.Events;


namespace JB
{
    [Serializable]
    public class FireMode : ScriptableObject
    {
        [SerializeField] protected float bulletDelay = 0.5f;
        [SerializeField] protected float burstDelay = 0;


        protected Action Fire = null;
        public Action FireAction
        {
            set { Fire = value; }
            get { return Fire; }
        }


        public virtual void UpdateFiring() { }
        public virtual void FireStart() { }
        public virtual void FireHeld() { }
        public virtual void FireEnd() { }

    }
}
