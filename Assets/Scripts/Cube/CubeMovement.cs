using UnityEngine;
using UnityEngine.InputSystem;

public class CubeMovement : MonoBehaviour
{
    CubeSpawnManager _spawnManager;
    Rigidbody _rb;

    // 이동 ###############################################
    Transform _startPoint;
    Transform _endPoint;

    float _moveSpeed;
    float _moveTime;

    // 상태 ###############################################
    bool _isMoving;
    bool _isFalling;
    bool _isMoveOnX;

    // 큐브 초기화 ###########################################
    public void InitCube(CubeSpawnManager manager)
    {
        _spawnManager = manager;
    }

    // 이동 시작 ############################################
    public void CubeMove(Transform start, Transform end, float speed)
    {
        _startPoint = start;
        _endPoint = end;
        _moveSpeed = speed;

        _isMoving = true;
        _isFalling = false;

        _isMoveOnX = Mathf.Abs(start.position.x - end.position.x) > 0;

        _moveTime = 0f;

        if (_rb != null)
            _rb.isKinematic = true;
    }

    // 큐브 낙하 ############################################
    public void CubeFall()
    {
        _isMoving = false;
        _isFalling = true;

        _startPoint = null;
        _endPoint = null;

        if (_rb == null)
            _rb = gameObject.AddComponent<Rigidbody>();

        _rb.isKinematic = false;

        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        _rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
    }

    // 큐브 정지 ############################################
    public void StopCube()
    {
        _isMoving = false;
    }

    // 큐브 초기화 ###########################################
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

    // 화면 밖 처리 ##########################################
    public void OnBecameInvisible()
    {
        if (_spawnManager == null) return;

        if (_isFalling)
        {
            _spawnManager.RemoveCube(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 방어 코드
        if(!_isMoving) return;
        if (_isFalling) return;
        if (_startPoint == null || _endPoint == null) return;

        _moveTime += Time.deltaTime;

        float distance = Vector3.Distance(_startPoint.position, _endPoint.position);

        float t = Mathf.PingPong(_moveTime * _moveSpeed / distance, 1f);

        Vector3 pos = transform.position;

        if(_isMoveOnX)
            pos.x = Mathf.Lerp(_startPoint.position.x, _endPoint.position.x, t);
        else
            pos.z = Mathf.Lerp(_startPoint.position.z, _endPoint.position.z, t);

        transform.position = pos;
    }
}
