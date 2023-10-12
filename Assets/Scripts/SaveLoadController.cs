using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadController : MonoBehaviour
{
    [SerializeField]
    private ProgressController _progressController;

    [SerializeField]
    private BlockSpawnController _blockSpawnController;

    private void OnEnable()
    {
        GameStateController.OnGameQuit += Save;
    }

    public void Save()
    {
        SaveData _data = new SaveData();

        List<NumberBlock> mapBlocks = _blockSpawnController.GetMapBlocks();
        List<NumberBlock> blocks = new List<NumberBlock>();

        for (int i = 0; i < mapBlocks.Count; i++)
            if (!mapBlocks[i].isAiming) blocks.Add(mapBlocks[i]);
        
        _data.CurrentScores = _progressController.GetScores();

        for (int i = 0; i < blocks.Count; i++)
        {
            BlockSaveData blockSaveData = new BlockSaveData(blocks[i].GetBlockData(),
                blocks[i].transform.position);

            _data.MapBlockData.Add(blockSaveData.MapBlockData);
            _data.MapBlocksPosition.Add(blockSaveData.MapBlocksPosition);
        }

        string _json = JsonUtility.ToJson(_data, false);

        Debug.Log(_json);

        File.WriteAllText(Application.persistentDataPath + "/Data.json", _json);
        Debug.Log(Application.persistentDataPath + "/Data.json");
    }

    public SaveDataStructure Load()
    {
        if (!Exist()) return null;

        string _json = File.ReadAllText(Application.persistentDataPath + "/Data.json");

        SaveData _data = JsonUtility.FromJson<SaveData>(_json);
        SaveDataStructure _dataStructure = new SaveDataStructure();

        Debug.Log(_json);

        _dataStructure.CurrentScores = _data.CurrentScores;

        for (int i = 0; i < _data.MapBlockData.Count; i++)
        {
            BlockSaveData blockSaveData = new BlockSaveData(_data.MapBlockData[i], _data.MapBlocksPosition[i]);
            _dataStructure.BlockSaveData.Add(blockSaveData);
        }

        return _dataStructure;
    }

    private bool Exist()
    {
        if (!File.Exists(Application.persistentDataPath + "/Data.json") ||
            File.ReadAllText(Application.persistentDataPath + "/Data.json").Length <= 0)
        {
            FileStream _fs = File.Create(Application.persistentDataPath + "/Data.json");
            _fs.Close();
            return false;
        }

        return true;
    }

    private void OnDisable()
    {
        GameStateController.OnGameQuit -= Save;
    }
}

public class SaveData
{
    public int CurrentScores = new int();
    public List<BlockData> MapBlockData = new List<BlockData>();
    public List<Vector3> MapBlocksPosition = new List<Vector3>();
}

public class SaveDataStructure
{
    public int CurrentScores = new int();
    public List<BlockSaveData> BlockSaveData = new List<BlockSaveData>();
}

public class BlockSaveData
{
    public BlockData MapBlockData = new BlockData();
    public Vector3 MapBlocksPosition = new Vector3();

    public BlockSaveData(BlockData blockData, Vector3 position)
    {
        this.MapBlockData = blockData;
        this.MapBlocksPosition = position;
    }
}

