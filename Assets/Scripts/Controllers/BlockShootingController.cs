using UnityEngine;

public class BlockShootingController : MonoBehaviour
{
    [SerializeField] private Transform _aim;

    [SerializeField]
    private float _aimMoveSpeed = 0.2f;

    [Space(25)]

    [SerializeField] 
    private Vector3 _aimLimit = new Vector3(1.5f, 0);

    [SerializeField]
    private Vector3 _shootForce = new Vector3(0, 0, 100f);

    private BlockSpawnController _blockSpawnController;
    private DeadLine _deadLine;
    private NumberBlock _currentBlock;

    private bool _isAlreadyAim;

    private void OnEnable()
    {
        GameStateController.OnGameEnd += DisableAim;
    }

    public void Init(BlockSpawnController blockSpawnController, DeadLine deadLine)
    {
        _blockSpawnController = blockSpawnController;
        _deadLine = deadLine;
    }

    public void FixedUpdate()
    {
        if (!_currentBlock) return;

        _aim.gameObject.SetActive(Input.GetMouseButton(0));

        if (Input.GetMouseButton(0)) Aim();
        else Shoot();
    }

    public void SetNumberBlock(NumberBlock block)
    {
        _currentBlock = block;

        _deadLine.SetIgnoreBlock(_currentBlock);
        _currentBlock.isAiming = true;
    }

    private void Aim()
    {
        Vector3 mousePosition = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
        float offsetX = mousePosition.x * _aimMoveSpeed;

        Vector3 pos = new Vector3(Mathf.Clamp(_currentBlock.transform.position.x + offsetX, -_aimLimit.x, _aimLimit.x), 0, 0);

        _currentBlock.transform.position = pos;
        _aim.position = pos;

        _isAlreadyAim = true;
    }

    private void Shoot()
    {
        if (!_isAlreadyAim) return;

        _currentBlock.ChangeRigidbodyState(true);

        _currentBlock.NumberBlockBody.AddForce(_shootForce);
        _currentBlock.isAiming = false;
        _currentBlock = null;

        _isAlreadyAim = false;

        _blockSpawnController.SpawnNewBlock();
    }

    private void DisableAim()
    {
        _aim.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        GameStateController.OnGameEnd -= DisableAim;
    }
}
