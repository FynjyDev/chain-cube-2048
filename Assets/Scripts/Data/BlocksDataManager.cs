using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlocksDataManager", menuName = "Data/BlocksDataManager")]
public class BlocksDataManager : ScriptableObject
{
    public enum BlockTypes {number, rainbow , bomb  }

    public NumberBlock _blockPrefab;

    [SerializeField] private int _minStartIndex = 1;
    [SerializeField] private int _interval = 3;

    [SerializeField] private List<BlockData> _blockDatas;

    public List<BlockData> GetAllBlockDatas()
    {
        return _blockDatas;
    }

    public BlockData GetRandomBlockData()
    {
        return _blockDatas[Random.Range(0, _blockDatas.Count)];
    }

    public BlockData GetDataByCount(int count)
    {
        foreach (BlockData bD in _blockDatas)
            if (bD.BlockCount == count)
                return bD;

        return null;
    }

    public BlockData GetDataByPlayerProgress(List<NumberBlock> currentBlocks)
    {
        BlockData maxBlockData = GetMaxBlockData(currentBlocks);

        int maxExlusive = _minStartIndex;
        int maxBlockDataIndex = _blockDatas.IndexOf(maxBlockData);

        if (maxBlockDataIndex > _minStartIndex + _interval) 
            maxExlusive = maxBlockDataIndex - _interval;

        return _blockDatas[Random.Range(0, maxExlusive)];
    }

    public BlockData GetMaxBlockData(List<NumberBlock> currentBlocks)
    {
        List<BlockData> blockDatas = new List<BlockData>();
        currentBlocks.ForEach(x => blockDatas.Add(x.GetBlockData()));

         BlockData maxBlockData = currentBlocks.Count <= 0 ? _blockDatas[1] : blockDatas[0];

        foreach (BlockData b in blockDatas)
            if (b.BlockCount > maxBlockData.BlockCount)
                maxBlockData = b;

        return maxBlockData;
    }
}
