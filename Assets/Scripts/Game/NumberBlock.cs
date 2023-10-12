using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberBlock : MonoBehaviour
{
    public Rigidbody NumberBlockBody;

    [SerializeField] private BoxCollider BlockCollider;
    [SerializeField] private MeshRenderer BlockRenderer;

    [SerializeField]
    private List<TextMeshProUGUI> _blockCountNumbersTexts;

    private BlockSpawnController _blockSpawnController;
    private BlockData _blockData;

    public bool holdBlock;
    public bool isAiming;

    public void OnBlockSpawned(BlockData blockData, BlockSpawnController blockSpawnController)
    {
        _blockData = blockData;
        _blockSpawnController = blockSpawnController;

        _blockCountNumbersTexts.ForEach(x => x.text = blockData.BlockName);
        BlockRenderer.material = _blockData.BlockMaterial;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            NumberBlock block = contact.otherCollider.gameObject.GetComponent<NumberBlock>();

            if (block == null || holdBlock) return;

            if (block._blockData == _blockData)
            {
                block.holdBlock = true;
                holdBlock = true;

                _blockSpawnController.OnMerge(block, this);

                return;
            }
        }      
    }

    public void ChangeRigidbodyState(bool enabled)
    {
        NumberBlockBody.isKinematic = !enabled;
        BlockCollider.enabled = enabled;
    }

    public BlockData GetBlockData()
    {
        return _blockData;
    }
}
