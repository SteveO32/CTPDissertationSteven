using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralCanvas : MonoBehaviour
{
    public RectTransform scorePanel { get { return scorePanel_; } }
    public RectTransform reticulePanel { get { return reticulePanel_; } }

    [SerializeField] RectTransform scorePanel_;
    [SerializeField] RectTransform reticulePanel_;
}
