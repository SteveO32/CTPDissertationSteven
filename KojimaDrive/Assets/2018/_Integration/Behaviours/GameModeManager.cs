using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KojimaParty
{

public class GameModeManager : MonoBehaviour
{
    [SerializeField] List<GameModeInfo> gameModes;

    private static GameModeManager instance;

    private GameMode currentMode;
    private int[] lastRankings;

    private List<GameModeInfo> playlist;
    private int playlistIndex;


    public static GameModeInfo SelectNextMode()
    {
        if (instance.PlaylistFinished())
        {
            instance.GeneratePlaylist();
        }

        GameModeInfo next_mode = instance.playlist[instance.playlistIndex];
        ++instance.playlistIndex;

        return next_mode;
    }


    void Awake()
    {
        if (instance == null)
        {
            InitSingleton();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    void Update()
    {
        if (currentMode != null)
        {
            // Debug rank assignment.
            if (Input.GetKeyDown(KeyCode.Return))
            {
                currentMode.GameModeFinished(Random.Range(1, 5), Random.Range(1, 5),
                    Random.Range(1, 5), Random.Range(1, 5));
            }
        }
    }


    void InitSingleton()
    {
        instance = this;

        if (transform.parent == null)
            DontDestroyOnLoad(this.gameObject);

        lastRankings = new int[] { 0, 0, 0, 0 };
        SceneManager.sceneLoaded += SceneLoaded;
    }


    void GeneratePlaylist()
    {
        playlist = new List<GameModeInfo>();
        playlistIndex = 0;

        List<GameModeInfo> unshuffled = GenerateUnshuffledPlaylist();

        while (unshuffled.Count > 0)
        {
            int index = Random.Range(0, unshuffled.Count);
            GameModeInfo mode = unshuffled[index];

            playlist.Add(mode);
            unshuffled.RemoveAt(index);
        }
    }


    List<GameModeInfo> GenerateUnshuffledPlaylist()
    {
        List<GameModeInfo> list = new List<GameModeInfo>();

        foreach (var mode in gameModes)
        {
            list.Add(mode);
        }

        return list;
    }


    bool PlaylistFinished()
    {
        if (playlist == null)
            return true;

        if (playlistIndex >= playlist.Count)
            return true;

        return false;
    }


    void SceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if (instance != this)
            return;

        if (_scene.name.Contains("Board"))
        {
            // Inform the Board Scene of new rankings.
            var boardScene = GameObject.FindObjectOfType<JB.BoardSceneController>();

            if (boardScene == null)
                return;

            boardScene.UpdateLastRankings(lastRankings);
        }
        else if (_scene.name.Contains("GameMode"))
        {
            // Find the GameMode in the scene when its loaded.
            currentMode = GameObject.FindObjectOfType<GameMode>();

            if (currentMode == null)
                return;

            currentMode.onGameModeFinished.AddListener(GameModeFinished);
            currentMode.StartGameMode();
        }
    }


    void GameModeFinished()
    {
        lastRankings = currentMode.rankings;
        SceneManager.LoadScene("Board");
    }


}

} // namespace JB
