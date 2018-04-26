using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace JB
{

public class SplinePoint
{
    public int pointIndex { get; private set; }
    public float splineProgress { get; private set; }
    public Vector3 worldPosition { get; private set; }

    public SplinePoint(int _pointIndex, float _splineProgress, Vector3 _worldPosition)
    {
        pointIndex = _pointIndex;
        splineProgress = _splineProgress;
        worldPosition = _worldPosition;
    }
}

public interface IBoardManager
{
    float GetPlayerProgress(int _playerIndex);
}

public class BoardManager : MonoBehaviour, IBoardManager
{
    public UnityEvent boardFinishedSync;

    [Header("Parameters")]
    [SerializeField] int numPoints;
    [SerializeField] GameObject playerTokenPrefab;
    [SerializeField] GameObject checkpointTokenPrefab;

    [Header("References")]
    [SerializeField] Bird.BezierSpline spline;

    private static List<SplinePoint> lastPlayerPoints;

    private List<SplinePoint> points = new List<SplinePoint>();

    private List<PlayerBoardToken> playerTokens = new List<PlayerBoardToken>();
    private bool initialised;

    private const int MAX_PLAYERS = 4;


    public void Synchronise(int[] _rankings)
    {
        if (!initialised)
            Start();

        StopAllCoroutines();
        StartCoroutine(SynchroniseRoutine(_rankings));
    }


    public void MovePlayerToNextPoint(int _playerIndex)
    {
        if (!JHelper.ValidIndex(_playerIndex, playerTokens.Count))
            return;

        var token = playerTokens[_playerIndex];
        if (token.targetPoint.pointIndex + 1 < points.Count)
        {
            token.SetTargetPoint(points[token.targetPoint.pointIndex + 1]);
        }
        else
        {
            token.SetTargetPoint(points[0]);
        }
    }


    public float GetPlayerProgress(int _playerIndex)
    {
        if (!JHelper.ValidIndex(_playerIndex, MAX_PLAYERS))
            return 0;

        return playerTokens[_playerIndex].GetProgress();
    }


    void Start()
    {
        if (initialised)
            return;

        InitBoardPoints();
        InitPlayers();

        initialised = true;
    }


    IEnumerator SynchroniseRoutine(int[] _rankings)
    {
        Debug.Log("Board Started Synchronising");

        for (int i = 0; i < MAX_PLAYERS; ++i)
        {
            var token = playerTokens[i];
            token.enabled = true;

            int prev_score = token.targetPoint.pointIndex;
            int score_adjustment = RankingToScore(_rankings[i]);

            int sum = prev_score + score_adjustment;
            token.SetTargetPoint(points[sum >= numPoints ? 0 : sum]);
        }

        yield return new WaitUntil(() => playerTokens.TrueForAll(token => !token.IsMoving()));

        boardFinishedSync.Invoke();
        UpdateLastPoints();

        Debug.Log("Board Finished Synchronising");
    }


    void InitBoardPoints()
    {
        GeneratePoints();

        if (lastPlayerPoints == null)
        {
            InitLastPlayerPoints();
        }
    }


    void GeneratePoints()
    {
        if (spline == null)
            return;

        points.Clear();

        for (int i = 0; i < numPoints; ++i)
        {
            float progress = (float)i / numPoints;
            Vector3 position = spline.GetPoint(progress);

            points.Add(new SplinePoint(i, progress, position));

            var clone = Instantiate(checkpointTokenPrefab, this.transform);
            clone.transform.position = position;
        }
    }


    /// <summary>
    /// Initialise the static LastPlayerPoints list.
    /// 
    /// This is used to correctly position each token whenever the board
    /// scene is loaded, taking into account its previous progress.
    /// </summary>
    void InitLastPlayerPoints()
    {
        lastPlayerPoints = new List<SplinePoint>();

        for (int i = 0; i < MAX_PLAYERS; ++i)
        {
            lastPlayerPoints.Add(points[0]);
        }
    }


    void InitPlayers()
    {
        for (int i = 0; i < MAX_PLAYERS; ++i)
        {
            var start_point = lastPlayerPoints[i];

            var clone = Instantiate(playerTokenPrefab, Vector3.zero, Quaternion.identity);
            var token = clone.GetComponent<PlayerBoardToken>();

            token.SetID(i, 4);
            token.AddSplineFollower(spline);
            token.SetStartPoint(start_point);

            playerTokens.Add(token);
        }
    }


    int RankingToScore(int _ranking)
    {
        switch (_ranking)
        {
            case 1: return 4;
            case 2: return 3;
            case 3: return 2;
            case 4: return 1;
        }

        return 0;
    }



    void OnDrawGizmos()
    {
        if (points == null)
            return;

        int i = 0;
        foreach (var point in points)
        {
            bool first = i == 0;

            Gizmos.color = first ? Color.blue : Color.red;
            Gizmos.DrawWireSphere(points[i].worldPosition, first ? 20 : 10);

            ++i;
        }
    }


    void UpdateLastPoints()
    {
        for (int i = 0; i < MAX_PLAYERS; ++i)
        {
            lastPlayerPoints[i] = points[playerTokens[i].targetPoint.pointIndex];
        }
    }

}

} // namespace JB
