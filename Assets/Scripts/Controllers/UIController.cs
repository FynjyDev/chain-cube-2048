using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    public void UpdateScoresCount(int scores)
    {
        string format = "#,###,###";
        _scoreText.text = scores.ToString(format);
    }
}
