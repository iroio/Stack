using UnityEngine;
using UnityEngine.InputSystem;

public class CubeMovement : MonoBehaviour
{
    CubeSpawnManager _manager;
    Rigidbody _rb;

    Transform _start;
    Transform _end;

    float _speed;
    float _time;

    bool _isMoving;
    bool _isFalling;
    bool _isX;

    public void InitCube(CubeSpawnManager manager)
    {
        _manager = manager;
    }

    public void CubeMove(Transform start, Transform end, float speed)
    {
        _start = start;
        _end = end;
        _speed = speed;

        _isMoving = true;
        _isFalling = false;

        _isX = Mathf.Abs(start.position.x - end.position.x) > 0;

        _time = 0f;

        if (_rb != null)
            _rb.isKinematic = true;
    }

    public void CubeFall()
    {
        _isMoving = false;
        _isFalling = true;

        _start = null;
        _end = null;

        if (_rb == null)
            _rb = gameObject.AddComponent<Rigidbody>();

        _rb.isKinematic = false;

        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        _rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
    }

    public void StopCube()
    {
        _isMoving = false;
    }

    public void ResetCube()
    {
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

        if (_isFalling)
        {
            _manager.RemoveCube(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 방어 코드
        if(!_isMoving) return;
        if (_isFalling) return;
        if (_start == null || _end == null) return;

        _time += Time.deltaTime;

        float distance = Vector3.Distance(_start.position, _end.position);

        float t = Mathf.PingPong(_time * _speed / distance, 1f);

        Vector3 pos = transform.position;

        if(_isX)
            pos.x = Mathf.Lerp(_start.position.x, _end.position.x, t);
        else
            pos.z = Mathf.Lerp(_start.position.z, _end.position.z, t);

        transform.position = pos;
    }
}
