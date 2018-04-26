using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JB
{

public class BoardSceneController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float delayBeforeSync = 3;
    [SerializeField] float delayBeforeSelection =5;

    [Header("References")]
    [SerializeField] BoardManager board;
    [SerializeField] BoardSceneUI ui;

    private int[] lastRankings;


    public void UpdateLastRankings(int[] _lastRankings)
    {
        lastRankings = _lastRankings;
    }



    void Start()
    {
        StartCoroutine(TempRoutine());
    }


    IEnumerator TempRoutine()
    {
        ui.Init(board);

        yield return new WaitForSecondsRealtime(delayBeforeSync);

        SynchroniseBoard();

        yield return new WaitForSecondsRealtime(delayBeforeSelection);

        KojimaParty.GameModeInfo nextMode = KojimaParty.GameModeManager.SelectNextMode();
        ui.ShowModeSelection(nextMode);

        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));

        SceneManager.LoadScene(nextMode.sceneName);
    }


    void SynchroniseBoard()
    {
        if (lastRankings == null)
            lastRankings = new int[] { 0, 0, 0, 0 };

        board.Synchronise(lastRankings);
    }

}

} // namespace JB
