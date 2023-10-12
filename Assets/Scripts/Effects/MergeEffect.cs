using TMPro;
using UnityEngine;

public class MergeEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoresText;
    [SerializeField] private Animator _effectAnimator;
    [SerializeField] private ParticleSystem _mergeFx;

    public void SetMergeEffect(string mergeScoresName)
    {
        _scoresText.text = mergeScoresName;
        _mergeFx.Play();

        _effectAnimator.SetTrigger("Play");
    }

    public void DestroyAnimationEvent()
    {
        Destroy(gameObject);
    }
}
