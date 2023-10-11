using UnityEngine;

public class ProgressController : MonoBehaviour
{
    [SerializeField]
    private UIController _uiController;

    [SerializeField]
    private int _scoresCount;

    public void OnMerge(int scores)
    {
        UpdateScoresCount(scores);
    }

    private void UpdateScoresCount(int scores)
    {
        _scoresCount += scores;
        _uiController.UpdateScoresCount(_scoresCount);
    }
}
