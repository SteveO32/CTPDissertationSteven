using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KojimaParty
{

[CreateAssetMenu(menuName = "GameMode/Game Mode Info")]
public class GameModeInfo : ScriptableObject
{
    public string modeName              { get { return modeName_; } }
    public string sceneName             { get { return sceneName_; } }
    public string description           { get { return description_; } }
    public Sprite modeScreenshot        { get { return modeScreenshot_; } }
    public MovieTexture movieTexture    { get { return tutorialVideo_; } }

    [Header("Basic Information")]

    [Tooltip("The title of your Game Mode.")]
    [SerializeField] string modeName_;

    [Tooltip("The name of your Game Mode's scene.\nThis MUST start with 'GameMode_'.")]
    [SerializeField] string sceneName_;

    [Tooltip("A description of your Game Mode.\nTry to keep this succinct.")]
    [SerializeField] [TextArea] string description_;

    [Header("Visual Information")]

    [Tooltip("An image to identify your Game Mode within selection interfaces.")]
    [SerializeField] Sprite modeScreenshot_;

    [Tooltip("A short tutorial video to display before your Game Mode starts.")]
    [SerializeField] MovieTexture tutorialVideo_;

}

} // namespace Kojima
