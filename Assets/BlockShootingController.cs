using UnityEngine;

public class BlockShootingController : MonoBehaviour
{
    private NumberBlock _currentNumberBlock;

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
        if (!_currentNumberBlock) return;

        if (Input.GetMouseButton(0)) Aim();
        else Shoot();
    }

    public void SetNumberBlock(NumberBlock block)
    {
        _currentNumberBlock = block;
    }

    private void Aim()
    {
        Vector3 mousePosition = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
        float offsetX = mousePosition.x * _aimMoveSpeed;

        Vector3 pos = new Vector3(Mathf.Clamp(_currentNumberBlock.transform.position.x + offsetX, -_aimLimit.x, _aimLimit.x), 0, 0);

        _currentNumberBlock.transform.position = pos;
        _isAlreadyAim = true;
    }

    private void Shoot()
    {
        if (!_isAlreadyAim) return;

        _currentNumberBlock.NumberBlockBody.AddForce(_shootForce);
        _currentNumberBlock = null;

        _isAlreadyAim = false;
    }
}
