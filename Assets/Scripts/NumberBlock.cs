using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberBlock : MonoBehaviour
{
    public Rigidbody NumberBlockBody;
    public MeshRenderer BlockRenderer;

    [SerializeField]
    private List<TextMeshProUGUI> _blockCountNumbersTexts;

    private BlockData _blockData;

    public void OnBlockSpawned(BlockData blockData)
    {
        _blockData = blockData;

        _blockCountNumbersTexts.ForEach(x => x.text = blockData.BlockName);
        BlockRenderer.material = _blockData.BlockMaterial;
    }
}
