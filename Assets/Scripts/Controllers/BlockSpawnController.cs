using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnController : MonoBehaviour
{
    [SerializeField] private BlockShootingController _blockShootingController;
    [SerializeField] private NumberBlock _numberBlockPrefab;
    [SerializeField] private BlocksDataManager _blocksDataManager;
    [SerializeField] private Transform _blockSpawnPosition;

    [Space(15)]

    [SerializeField]
    private float _blockAnimationSpeed;

    [Space(15)]

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

        block.OnBlockSpawned(_blocksDataManager.GetRandomBlockData(), this);
        block.ChangeRigidbodyState(false);

        _blocks.Add(block);

        _currentBlockScaleAnimation = StartCoroutine(BlockScaleAnimatior(block, _blockAnimationSpeed));
    }

    public void SpawnNewBlock(BlockData blockData, Vector3 position)
    {
        NumberBlock block = Instantiate(_numberBlockPrefab, position, Quaternion.identity, _blockSpawnPosition);

        block.OnBlockSpawned(blockData, this);
        _blocks.Add(block);
    }

    public void OnMerge(NumberBlock block_1, NumberBlock block_2)
    {
        Vector3 middlePos = (block_1.transform.position + block_2.transform.position) / 2;
        BlockData blockData = _blocksDataManager.GetDataByCount(block_1.GetBlockData().BlockCount + block_2.GetBlockData().BlockCount);

        _blocks.Remove(block_1);
        _blocks.Remove(block_2);

        Destroy(block_1.gameObject);
        Destroy(block_2.gameObject);

        SpawnNewBlock(blockData, middlePos);
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
