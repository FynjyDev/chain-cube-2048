using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnController : MonoBehaviour
{
    [SerializeField] private Vector3 _minMergeForce = new Vector3(0, 25f, 0);
    [SerializeField] private Vector3 _maxMergeForce = new Vector3(25f, 50f, 25f);

    [Space(15)]

    [SerializeField] private float _blockAnimationSpeed;

    [Space(15)]

    [SerializeField]
    private List<NumberBlock> _blocks;

    private Coroutine _currentBlockScaleAnimation;
    private BlockShootingController _blockShootingController;
    private ProgressController _progressController;
    private BlocksDataManager _blocksDataManager;

    private void OnEnable()
    {
        GameStateController.OnGameEnd += ClearMap;
    }

    public void Init(List<BlockSaveData> blockSaveData, BlockShootingController shootingController, ProgressController progressController, BlocksDataManager dataManager)
    {
        _blockShootingController = shootingController;
        _progressController = progressController;
        _blocksDataManager = dataManager;

        foreach (BlockSaveData data in blockSaveData) CreateBlock(data.MapBlocksPosition, data.MapBlockData);            
        SpawnNewBlock();
    }

    private void ClearMap()
    {
        for (int i = 0; i < _blocks.Count; i++)
            Destroy(_blocks[i].gameObject);

        _blocks.Clear();
    }

    private NumberBlock CreateBlock(Vector3 pos = new Vector3(), BlockData blockData = null, bool rigidbodyState = true)
    {
        NumberBlock block = Instantiate(_blocksDataManager._blockPrefab, pos, Quaternion.identity, transform);

        block.OnBlockSpawned(blockData, this);
        block.ChangeRigidbodyState(rigidbodyState);

        _blocks.Add(block);

        return block;
    }

    public void SpawnNewBlock()
    {
        BlockData blockData = _blocksDataManager.GetDataByPlayerProgress(_blocks);

        NumberBlock block = CreateBlock(Vector3.zero, blockData, false);

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
        int blocksAmount = block_1.GetBlockData().BlockCount + block_2.GetBlockData().BlockCount;

        Vector3 middlePos = (block_1.transform.position + block_2.transform.position) / 2;
        BlockData blockData = _blocksDataManager.GetDataByCount(block_1.GetBlockData().BlockCount + block_2.GetBlockData().BlockCount);

        _blocks.Remove(block_1);
        _blocks.Remove(block_2);

        Destroy(block_1.gameObject);
        Destroy(block_2.gameObject);

        if (blockData == null)
        {
            _progressController.OnMerge(blocksAmount, "Max", middlePos);
            return;
        }

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

    public List<NumberBlock> GetMapBlocks()
    {
        return _blocks;
    }

    private void OnDisable()
    {
        GameStateController.OnGameEnd -= ClearMap;
    }
}
