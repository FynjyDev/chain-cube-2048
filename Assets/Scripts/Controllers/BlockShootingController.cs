using UnityEngine;

public class BlockShootingController : MonoBehaviour
{
    private NumberBlock _currentBlock;

    [SerializeField]
    private BlockSpawnController _blockSpawnController;

    [SerializeField]
    private float _aimMoveSpeed = 0.2f;

    [Space(25)]

    [SerializeField] 
    private Vector3 _aimLimit = new Vector3(1.5f, 0);

    [SerializeField]
    private Vector3 _shootForce = new Vector3(0, 0, 100f);

    private bool _isAlreadyAim;

    public void FixedUpdate()
    {
        if (!_currentBlock) return;

        if (Input.GetMouseButton(0)) Aim();
        else Shoot();
    }

    public void SetNumberBlock(NumberBlock block)
    {
        _currentBlock = block;
    }

    private void Aim()
    {
        Vector3 mousePosition = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
        float offsetX = mousePosition.x * _aimMoveSpeed;

        Vector3 pos = new Vector3(Mathf.Clamp(_currentBlock.transform.position.x + offsetX, -_aimLimit.x, _aimLimit.x), 0, 0);

        _currentBlock.transform.position = pos;
        _isAlreadyAim = true;
    }

    private void Shoot()
    {
        if (!_isAlreadyAim) return;

        _currentBlock.ChangeRigidbodyState(true);

        _currentBlock.NumberBlockBody.AddForce(_shootForce);
        _currentBlock = null;

        _isAlreadyAim = false;

        _blockSpawnController.SpawnNewBlock();
    }
}
