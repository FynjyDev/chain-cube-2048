using UnityEngine;

public class ProgressController : MonoBehaviour
{
    [SerializeField]
    private MergeEffect _mergeEffect;

    [SerializeField]
    private UIController _uiController;

    [SerializeField]
    private int _scoresCount;

    private void OnEnable()
    {
        GameStateController.OnGameEnd += OnGameEnd;
    }

    public void Init(int scores)
    {
        UpdateScoresCount(scores);
    }

    private void OnGameEnd()
    {
        _scoresCount = 0;
    }

    public void OnMerge(int mergeScores, string name, Vector3 mergePos)
    {
        MergeEffect effect = Instantiate(_mergeEffect, mergePos, Quaternion.identity, transform);
        effect.SetMergeEffect(name);

        UpdateScoresCount(mergeScores);
    }

    private void UpdateScoresCount(int newScores)
    {
        _scoresCount += newScores;
        _uiController.UpdateScoresCount(_scoresCount);
    }

    public int GetScores()
    {
        return _scoresCount;
    }

    private void OnDisable()
    {
        GameStateController.OnGameEnd -= OnGameEnd;
    }

}
