using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeSpawnManager : MonoBehaviour
{
    CubeMovement _currentCube;

    // 카메라 타겟
    [SerializeField] Transform _target;
    [SerializeField] GameObject _cubePrefab;
    [SerializeField] GameObject _baseCube;

    [SerializeField] float _speedIncrease = 0.2f;
    [SerializeField] float _maxSpeed = 12f;

    [SerializeField] float _speed = 7f;
    [SerializeField] float _overlapOffset = 0.05f;

    [SerializeField] Transform[] _spawnPoint;

    Transform _start;
    Transform _end;

    GameObjectPool<CubeMovement> _cubePools;

    Vector3 _dir;
    Vector3 _pos;

    Vector3 _basePos;
    Vector3 _newPos;

    Vector3 _curScale;
    Vector3 _curCubePos;

    Vector3 _spawnPos;

    Vector3 _cutScale;
    Vector3 _cutPos;

    // 카메라 타겟 높이 조절
    float _targetY;
    float _cubeHeight;

    float _offset;
    float _absOffset;

    float _baseSize;
    float _overlapSize;

    float _center;
    float _perfactOffset;

    float _cutSize;

    //int _stackPoint = 0;

    bool _tempX;
    bool _isX = true;

    public float _CurrentSpeed => _speed;

    public void SpawnCube()
    {
        // ObjectPool 에서 큐브 하나 꺼내옴
        var cube = _cubePools.Get();
        cube.gameObject.SetActive(true);

        // 기준 위치 지정
        _basePos = _baseCube.transform.position;

        if (_isX)
        {
            _start = _spawnPoint[0];
            _end = _spawnPoint[1];

            // X축 스폰위치 지정
            _spawnPos = new Vector3(_spawnPoint[0].position.x, _basePos.y + _cubeHeight, _basePos.z);
        }
        else
        {
            _start = _spawnPoint[2];
            _end = _spawnPoint[3];

            // Z축 스폰위치 지정
            _spawnPos = new Vector3(_basePos.x, _basePos.y + _cubeHeight, _spawnPoint[2].position.z);
        }

        // 큐브의 위치와 크기 지정
        cube.transform.position = _spawnPos;
        cube.transform.localScale = _baseCube.transform.localScale;

        // 이동 방향 지정
        _dir = _isX ? Vector3.left : Vector3.back;

        // 이동 방향 저장
        _tempX = _isX;
        _isX = !_isX;

        _currentCube = cube;

        // 이동
        cube.CubeMove(_dir);

        // 큐브 높이만큼 카메라 위치 변경
        _targetY += _cubeHeight;

        // 속도 증가
        _speed += _speedIncrease;
        _speed = Mathf.Min(_speed, _maxSpeed);
    }

    public void SpawnCutCube()
    {
        var cutCube = _cubePools.Get();
        cutCube.gameObject.SetActive(true);

        _cutSize = _absOffset;

        _cutScale = _currentCube.transform.localScale;
        _cutPos = _currentCube.transform.position;

        float dir = Mathf.Sign(_offset);

        if (_tempX)
            _cutScale.x = _cutSize;
        else
            _cutScale.z = _cutSize;

        cutCube.transform.localScale = _cutScale;

        if (_tempX)
            _cutPos.x += dir * (_overlapSize / 2 + _cutSize / 2);
        else
            _cutPos.z += dir * (_overlapSize / 2 + _cutSize / 2);

        cutCube.transform.position = _cutPos;

        cutCube.CubeFall();
    }

    // 겹쳐진 큐브상테 체크
    public void CheckStack()
    {
        // 기준이 될 큐브
        _basePos = _baseCube.transform.position;
        // _tempX 가 True 일 때 X 축, False 일떄 Z축
        _baseSize = _tempX ? _baseCube.transform.localScale.x : _baseCube.transform.localScale.z;
        // 현재 큐브 위치
        _newPos = _currentCube.transform.position;

        // _offset = 이전 큐브와 현재 큐브 사이의 거리
        if (_tempX)
            _offset = _newPos.x - _basePos.x;
        else
            _offset = _newPos.z - _basePos.z;

        // 절대값으로 거리만
        _absOffset = Mathf.Abs(_offset);

        // 겹친 부분이 없으면 Game Over
        if (_absOffset >= _baseSize)
        {
            Debug.Log("Game Over");
            _currentCube.StopCube();
            return;
        }

        // Perfact 보정
        _perfactOffset = _baseSize * _overlapOffset;
        if (_absOffset <= _perfactOffset)
        {
            _offset = 0;
            _absOffset = 0;

            if (_tempX)
                _newPos.x = _basePos.x;
            else
                _newPos.z = _basePos.z;

            _currentCube.transform.position = _newPos;

            _speed -= _speedIncrease;
            _speed = Mathf.Min(_speed, _maxSpeed);
        }

        // 겹친 부분 크기 계산
        _overlapSize = Mathf.Max(0, _baseSize - _absOffset);
        _curScale = _currentCube.transform.localScale;

        // 퍼펙 이면 X
        if (_absOffset > _perfactOffset)
        {
            SpawnCutCube();
        }

        // 겹친부분만큼 큐브 크기 축소
        if (_tempX)
            _curScale.x = _overlapSize;
        else
            _curScale.z = _overlapSize;

        _currentCube.transform.localScale = _curScale;

        // 중심위치 보정
        _center = _offset / 2f;
        _curCubePos = _currentCube.transform.position;

        if (_tempX)
            _curCubePos.x -= _center;
        else
            _curCubePos.z -= _center;

        _currentCube.transform.position = _curCubePos;

        _baseCube = _currentCube.gameObject;
    }

    public void RemoveCube(CubeMovement cube)
    {
        cube.ResetCube();
        cube.gameObject.SetActive(false);
        _cubePools.Set(cube);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cubePools = new GameObjectPool<CubeMovement>(5, () =>
        {
            var obj = Instantiate(_cubePrefab, transform);
            obj.SetActive(false);
            var cube = obj.GetComponent<CubeMovement>();
            cube.InitCube(this);
            return cube;
        });

        _cubeHeight = _cubePrefab.transform.localScale.y;
        _targetY = _target.position.y - 1;
        SpawnCube();
    }

    void Update()
    {
        _pos = _target.position;
        _pos.y = Mathf.Lerp(_pos.y, _targetY, Time.deltaTime * 5f);
        _target.position = _pos;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_currentCube != null)
            {
                _currentCube.StopCube();
                CheckStack();
            }
            SpawnCube();
        }
    }
}
