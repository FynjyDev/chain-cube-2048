using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "Data/BlockData")]
public class BlockData : ScriptableObject
{
    public BlocksDataManager.BlockTypes blockType;

    public int BlockCount;
    public string BlockName;
    public Material BlockMaterial;
}
