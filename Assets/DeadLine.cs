using System.Collections;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider _timerSlider;
    [SerializeField] private float _deadTime;

    private NumberBlock _ignoreBlock;
    private NumberBlock _currentBlock;

    private GameStateController _gameStateController;

    public int collisionCount;
    public float currentTime;

    public void Init(GameStateController gameStateController)
    {
        _gameStateController = gameStateController;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        NumberBlock block = other.GetComponent<NumberBlock>();

        if (block && block != _ignoreBlock)
            collisionCount++;
    }

    public virtual void OnTriggerStay(Collider other)
    {
        NumberBlock block = other.GetComponent<NumberBlock>();

        if (block == null) return;
        if ( block == _ignoreBlock) return;

        currentTime += Time.deltaTime;
            float _lerpValue = currentTime / _deadTime;

            _timerSlider.value = Mathf.Lerp(0, 1, _lerpValue);

            if (currentTime >= _deadTime)
                _gameStateController.GameLose();
    }

    public virtual void OnTriggerExit(Collider other)
    {
        NumberBlock block = other.GetComponent<NumberBlock>();

        if (block == null) return;
        if (block == _ignoreBlock) return;

        if (collisionCount > 0)
            collisionCount--;

        if (collisionCount <= 0)
        {
            currentTime = 0;
            _timerSlider.value = 0;
        }
    }

    public void SetIgnoreBlock(NumberBlock block)
    {
        _ignoreBlock = block;
    }
}
