﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JB
{

public class CustomEvents
{
    [System.Serializable]
    public class IntEvent : UnityEvent<int> {}

    [System.Serializable]
    public class FloatEvent : UnityEvent<float> {}

    [System.Serializable]
    public class BooleanEvent : UnityEvent<bool> {}

    [System.Serializable]
    public class Vector3Event : UnityEvent<Vector3> {}

    [System.Serializable]
    public class CollisionEvent : UnityEvent<Collision> {}

    [System.Serializable]
    public class ColliderEvent : UnityEvent<Collider> {}

    [System.Serializable]
    public class Collision2DEvent : UnityEvent<Collision2D> {}

    [System.Serializable]
    public class Collider2DEvent : UnityEvent<Collider2D> {}

    [System.Serializable]
    public class RaycastHitEvent : UnityEvent<RaycastHit> {}

    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> {}

    [System.Serializable]
    public class BoatEvent : UnityEvent<DuckHuntBoat> {}

}

} // namespace JB
