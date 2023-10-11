using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnController : MonoBehaviour
{
    [SerializeField]
    private BlockShootingController _blockShootingController;

    [SerializeField]
    private NumberBlock _numberBlockPrefab;

    [SerializeField]
    private Transform _blockSpawnPosition;

    [SerializeField]
    private float _blockAnimationSpeed;

    [SerializeField]
    private List<NumberBlock> _blocks;

    private Coroutine _currentBlockScaleAnimation;

    private void OnEnable()
    {
        GameStateController.OnGameStart += SpawnNewBlock;
    }

    public void SpawnNewBlock()
    {
        NumberBlock block = Instantiate(_numberBlockPrefab, _blockSpawnPosition.position, _blockSpawnPosition.rotation, _blockSpawnPosition);

        _blocks.Add(block);

        _currentBlockScaleAnimation = StartCoroutine(BlockScaleAnimatior(block, _blockAnimationSpeed));
    }

    private IEnumerator BlockScaleAnimatior(NumberBlock block, float animtionSpeed)
    {
        float scaleTime = 0f;
        Transform blockTr = block.transform;
        Vector3 targetScale = blockTr.localScale;

        while (scaleTime < animtionSpeed)
        {
            blockTr.localScale = Vector3.zero;

            scaleTime += Time.deltaTime;
            float lerpValue = scaleTime / animtionSpeed;

            blockTr.localScale = Vector3.Lerp(Vector3.zero, targetScale, lerpValue);
            yield return new WaitForFixedUpdate();
        }

        _blockShootingController.SetNumberBlock(block);
        StopCoroutine(_currentBlockScaleAnimation);
    }

    private void OnDisable()
    {
        GameStateController.OnGameStart -= SpawnNewBlock;
    }
}
