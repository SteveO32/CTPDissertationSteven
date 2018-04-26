using UnityEngine;
using System.Collections;

namespace JB
{

public class CameraShakeZiggy : MonoBehaviour
{
    public Transform camTransform;
    public float shakeDuration = 0f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    [SerializeField]
    private int hitAmount = 0;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
        if (hitAmount >= 10)
        {
            BreakCamera();
            hitAmount = 0;
        }
    }

    public void CameraHit()
    {
        hitAmount++;
    }

    private void BreakCamera()
    {
        Debug.Log("Starting to break glass");
        GameObject.Find("GameRoomScripts").GetComponent<GameRoomUI>().BreakGlass();
    }
}

} // namespace JB
