using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlocksDataManager", menuName = "Data/BlocksDataManager")]
public class BlocksDataManager : ScriptableObject
{
    public enum BlockTypes {number, rainbow , bomb  }

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
}
