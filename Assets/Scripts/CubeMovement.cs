using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeMovement : MonoBehaviour
{
    CubeSpawnManager _manager;
    Rigidbody _rb;

    Vector3 _dir;

    Vector3 _start;
    Vector3 _end;

    bool _isMoving;
    bool _isFalling;

    public void InitCube(CubeSpawnManager manager)
    {
        _manager = manager;
    }

    public void CubeMove(Vector3 dir)
    {
        _dir = dir.normalized;

        _isMoving = true;
        _isFalling = false;

        if (_rb != null)
            _rb.isKinematic = true;
    }

    public void CubeFall()
    {
        _isMoving = true;
        _isFalling = true;

        if (_rb == null)
            _rb = gameObject.AddComponent<Rigidbody>();

        _rb.isKinematic = false;

        _rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
    }

    public void StopCube()
    {
        _isMoving = false;
    }

    public void ResetCube()
    {
        _dir = Vector3.zero;
        _isMoving = false;
        _isFalling = false;

        transform.rotation = Quaternion.identity;

        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }

    public void OnBecameInvisible()
    {
        if (_manager == null) return;

        if(_isFalling)
        {
            _manager.RemoveCube(this);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_isMoving)
        {
            float speed = _manager._CurrentSpeed;
            transform.position += _dir * speed * Time.deltaTime;
        }

        if (_isFalling && transform.position.y < -10f)
        {
            _manager.RemoveCube(this);
        }
    }
}
