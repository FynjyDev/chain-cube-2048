using System.Collections;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider _timerSlider;
    [SerializeField] private float _deadTime;

    private NumberBlock _ignoreBlock;

    private GameStateController _gameStateController;

    private int _collisionCount;
    private float _currentTime;

    public void Init(GameStateController gameStateController)
    {
        _gameStateController = gameStateController;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        NumberBlock block = other.GetComponent<NumberBlock>();

        if (block && block != _ignoreBlock)
            _collisionCount++;
    }

    public virtual void OnTriggerStay(Collider other)
    {
        NumberBlock block = other.GetComponent<NumberBlock>();

        if (block == null) return;
        if ( block == _ignoreBlock) return;

        _currentTime += Time.deltaTime;
            float _lerpValue = _currentTime / _deadTime;

            _timerSlider.value = Mathf.Lerp(0, 1, _lerpValue);

            if (_currentTime >= _deadTime)
                _gameStateController.GameLose();
    }

    public virtual void OnTriggerExit(Collider other)
    {
        NumberBlock block = other.GetComponent<NumberBlock>();

        if (block == null) return;
        if (block == _ignoreBlock) return;

        if (_collisionCount > 0)
            _collisionCount--;

        if (_collisionCount <= 0)
        {
            _currentTime = 0;
            _timerSlider.value = 0;
        }
    }

    public void SetIgnoreBlock(NumberBlock block)
    {
        _ignoreBlock = block;
    }
}
