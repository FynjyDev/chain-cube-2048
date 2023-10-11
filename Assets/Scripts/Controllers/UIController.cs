using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    public void UpdateScoresCount(int scores)
    {
        _scoreText.text = scores.ToString();
    }
}
