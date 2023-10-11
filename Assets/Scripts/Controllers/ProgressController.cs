using UnityEngine;

public class ProgressController : MonoBehaviour
{
    [SerializeField]
    private MergeEffect _mergeEffect;

    [SerializeField]
    private UIController _uiController;

    [SerializeField]
    private int _scoresCount;

    public void OnMerge(int mergeScores, string name, Vector3 mergePos)
    {
        MergeEffect effect = Instantiate(_mergeEffect, mergePos, Quaternion.identity, transform);
        effect.SetMergeEffect(name);

        UpdateScoresCount(mergeScores);
    }

    private void UpdateScoresCount(int mergeScores)
    {
        _scoresCount += mergeScores;
        _uiController.UpdateScoresCount(_scoresCount);
    }
}
