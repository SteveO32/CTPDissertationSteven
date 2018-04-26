using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KojimaParty
{

public abstract class GameMode : MonoBehaviour
{
    public int[] rankings { get; private set; }
    [HideInInspector] public UnityEvent onGameModeFinished;
    
    
    /// <summary>
    /// <para>The function that GameModeManager calls when it finds your GameMode.</para>
    /// 
    /// <para>All GameMode initialisation logic should happen inside this function.</para>
    /// </summary>
    public abstract void StartGameMode();


    /// <summary>
    /// <para>The function YOU call once your GameMode has finished.</para>
    /// 
    /// <para>This does not touch any of your GameMode's systems, so it is
    /// your responsibility to do any cleanup before you call this function.</para>
    /// 
    /// <para>The 'rankings' parameter represents which place each player
    /// was awarded after tallying up your GameMode's scores.</para>
    /// 
    /// <para>- Example usage 1: GameModeFinished(3, 1, 4, 2);</para>
    /// 
    /// <para>Players can also share an award, or be assigned 0 if they didn't participate.</para>
    /// 
    /// <para>- Example usage 2: GameModeFinished(2, 1, 2, 0)</para>
    /// </summary>
    public void GameModeFinished(int _p1rank, int _p2rank, int _p3rank, int _p4rank)
    {
        rankings = new int[] { _p1rank, _p2rank, _p3rank, _p4rank };
        onGameModeFinished.Invoke();
    }

}

} // namespace JB
