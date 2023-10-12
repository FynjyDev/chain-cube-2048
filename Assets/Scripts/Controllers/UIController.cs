using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    [SerializeField]
    private GameObject _losePanel;

    private void OnEnable()
    {
        GameStateController.OnGameEnd += ActivateLosePanel;
    }

    private void ActivateLosePanel()
    {
        _losePanel.SetActive(true);
    }

    public void UpdateScoresCount(int scores)
    {
        string format = "#,###,###";
        _scoreText.text = scores.ToString(format);
    }

    private void OnDisable()
    {
        GameStateController.OnGameEnd -= ActivateLosePanel;
    }
}
