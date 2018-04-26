using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Range
{
    public float m_min;
    public float m_max;

    /// <summary>
    /// Lerps between the range's min / max 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public float Lerp(float t)
    {
        return Mathf.Lerp(m_min, m_max, t);
    }

    /// <summary>
    /// Returns normalized distance from start (clamped)
    /// </summary>
    /// <returns></returns>
    public float Normalize(float value)
    {
        return Mathf.Clamp01((value - m_min) / (m_max - m_min));
    }
}

public class HDev_Arrow : MonoBehaviour
{
    [SerializeField] public GameObject m_target;
    [SerializeField] private float m_lerpSpeedRot;
    [SerializeField] private Range m_fadeOutDist;
    [SerializeField] private Renderer m_head;
    [SerializeField] private Renderer m_tail;
    // Use this for initialization


    void Start()
    {
        m_target = FH.FH_GameManager.CityLocation;
    }

    // Update is called once per frame
    void Update()
    {
        float visibility = m_fadeOutDist.Normalize(GetDistance(gameObject.transform.position, m_target.transform.position));
        SetAllMaterialAlpha(m_head, visibility);
        SetAllMaterialAlpha(m_tail, visibility);

        //Debug.Log(GetDistance() + "km");
        UpdateRotation();
    }

    ushort GetDistance(Vector3 t1, Vector3 t2)
    {
        return (ushort)(t1 - t2).magnitude;
    }

    void UpdateRotation()
    {
        Vector3 dir = (m_target.transform.position - transform.position).normalized;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(dir.x, dir.z);
        Quaternion target = Quaternion.AngleAxis(angle, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, target, m_lerpSpeedRot * Time.deltaTime);
    }

    void SetAllMaterialAlpha(Renderer r, float a)
    {
        foreach (var m in r.materials)
        {
            Color c = m.color;
            c.a = a;
            m.color = c;
        }
    }
}