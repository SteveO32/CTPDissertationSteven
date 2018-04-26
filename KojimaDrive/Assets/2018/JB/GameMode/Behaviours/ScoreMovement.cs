using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JB
{

public class ScoreMovement : MonoBehaviour
{

    [SerializeField]
    float movement_speed = 0.4f;
    [SerializeField]
    Text score_text;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, movement_speed, 0);
    }

    public void StartValues(Vector3 _pos, float _score, int _id)
    {
        transform.position = _pos;

        score_text.text = "+ " + _score.ToString();

        switch (_id)
        {
            case 0:
                score_text.color = Color.red;
                break;
            case 1:
                score_text.color = Color.blue;
                break;
            case 2:
                score_text.color = Color.green;
                break;
            case 3:
                score_text.color = Color.magenta;
                break;
            default:
                score_text.color = Color.red;
                break;
        }
    }
}

} // namespace JB
