using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnController : MonoBehaviour
{
    [SerializeField] private BlockShootingController _blockShootingController;
    [SerializeField] private ProgressController _progressController;
    [SerializeField] private BlocksDataManager _blocksDataManager;

    [Space(15)]

    [SerializeField] private Vector3 _minMergeForce = new Vector3(0, 25f, 0);
    [SerializeField] private Vector3 _maxMergeForce = new Vector3(25f, 50f, 25f);

    [Space(15)]

    [SerializeField] private float _blockAnimationSpeed;

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
        NumberBlock block = Instantiate(_blocksDataManager._blockPrefab, Vector3.zero, Quaternion.identity, transform);
        BlockData blockData = _blocksDataManager.GetDataByPlayerProgress(_blocks);

        block.OnBlockSpawned(blockData, this);
        block.ChangeRigidbodyState(false);

        _blocks.Add(block);

        _currentBlockScaleAnimation = StartCoroutine(BlockScaleAnimatior(block, _blockAnimationSpeed));
    }

    public void SpawnMergeBlock(BlockData blockData, Vector3 position)
    {
        NumberBlock block = Instantiate(_blocksDataManager._blockPrefab, position, Quaternion.identity, transform);

        float xForce = Random.Range(_minMergeForce.x, _maxMergeForce.x); 
        float yForce = Random.Range(_minMergeForce.y, _maxMergeForce.y); 
        float zForce = Random.Range(_minMergeForce.z, _maxMergeForce.z); 

        block.NumberBlockBody.AddForce(new Vector3(xForce, yForce, zForce));
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

        _progressController.OnMerge(blockData.BlockCount, blockData.BlockName, middlePos);

        SpawnMergeBlock(blockData, middlePos);
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
