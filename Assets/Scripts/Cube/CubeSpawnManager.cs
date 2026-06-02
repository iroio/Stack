using UnityEngine;
using UnityEngine.InputSystem;

public class CubeSpawnManager : MonoBehaviour
{
    GameManager _gameManager;
    CubeMovement _currentCube;  // 현재 움직이는 큐브
    CubeMovement _lastCube; // 마지막 큐브
    MaterialPropertyBlock _mpb; // 머터리얼 공유

    // =========================================================
    // 색상 관련 연결
    // =========================================================
    [SerializeField] CubeColorGenerator _cubeGen;
    [SerializeField] BGColorGenerator _bgGen;

    [SerializeField] CubeColorManager _cubeColorManager;
    [SerializeField] BGColorManager _bgColorManager;

    [SerializeField] Renderer _gradientCubeRenderer;

    // =========================================================
    // 파티클 관련 연결
    // =========================================================
    [SerializeField] ParticleSystem _particleSystem;

    // =========================================================
    // 설정
    // =========================================================
    [SerializeField] Transform _cameraTarget;   // 카메라가 따라갈 타켓
    [SerializeField] Transform _BG;                  // 배경
    [SerializeField] GameObject _cubePrefab;
    [SerializeField] GameObject _previousCube;
    [SerializeField] Transform _parentTransform;

    [SerializeField] Transform[] _spawnPoint;

    // =========================================================
    // 속도
    // =========================================================
    [SerializeField] float _speedIncrease = 0.1f;
    [SerializeField] float _maxSpeed = 12f;
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] float _minSpeed = 10f;

    [SerializeField] float _perfectThresholdRatio = 0.035f;  // 퍼펙트 판정 허용 비율

    // =========================================================
    // 판정 값
    // =========================================================
    float _offset;             // 기준 큐브와 현재 큐브의 거리
    float _absOffset;        // 거리 절댓값
    float _overlapSize;     // 겹친 길이

    // =========================================================
    // 상태값
    // =========================================================
    bool _lastAxisIsX;
    bool _nextAxisIsX = true;

    bool _inputLocked;

    bool _changeColor = false; // 색이 바뀌었는가?

    float _targetY;     // 카메라 목표 Y 위치
    float _BGY;         // 배경 목표 Y 위치
    float _cubeHeight;

    // =========================================================
    // 풀링
    // =========================================================
    GameObjectPool<CubeMovement> _cubePools;

    // =========================================================
    public float _CurrentSpeed => _moveSpeed;

    // =========================================================
    // 큐브 생성
    // =========================================================
    public void SpawnCube()
    {
        var cube = _cubePools.Get();
        cube.transform.SetParent(_parentTransform);
        cube.gameObject.SetActive(true);

        Vector3 basePos = _previousCube.transform.position;

        // 이돟 범위 : 큐브 반복 이동용
        Transform startPoint;
        Transform endPoint;

        Vector3 spawnPos;
        Vector3 moveDir;

        if (_nextAxisIsX)
        {
            startPoint = _spawnPoint[0];
            endPoint = _spawnPoint[1];

            spawnPos = new Vector3(_spawnPoint[0].position.x, basePos.y + _cubeHeight, basePos.z);
            moveDir = Vector3.left;
        }
        else
        {
            startPoint = _spawnPoint[2];
            endPoint = _spawnPoint[3];

            spawnPos = new Vector3(basePos.x, basePos.y + _cubeHeight, _spawnPoint[2].position.z);
            moveDir = Vector3.back;
        }

        // 큐브의 위치와 크기 지정
        cube.transform.position = spawnPos;
        cube.transform.localScale = _previousCube.transform.localScale;

        // 이동 축 저장
        _lastAxisIsX = _nextAxisIsX;
        _nextAxisIsX = !_nextAxisIsX;

        _currentCube = cube;

        cube.CubeMove(startPoint, endPoint, _moveSpeed);

        // 색상 변경
        Color cubeColor = _cubeGen.GetNextColor();
        Color bgColor = _bgGen.GetNextColor();

        Renderer cubeRenderer = _currentCube.GetComponent<Renderer>();

        _cubeColorManager.ApplyColor(cubeRenderer, cubeColor);
        _bgColorManager.ApplyColor(bgColor);

        ColorState.cubeColor = cubeColor;
        ColorState.bgColor = bgColor;

        ColorState.cubeCount = _cubeGen.GetCount();
        ColorState.bgCount = _bgGen.GetCount();

        if(!_changeColor)
        {
            _cubeColorManager.ApplyColor(_gradientCubeRenderer, cubeColor);
            _changeColor = true;
        }

        // 큐브 높이만큼 카메라 위치 변경
        _targetY += _cubeHeight;
        _BGY += _cubeHeight;
    }

    // =========================================================
    // 잘린 큐브 생성
    // =========================================================
    public void SpawnCutCube()
    {
        var cutCube = _cubePools.Get();

        // 잘린 큐브 색상 변경
        Renderer r = cutCube.GetComponent<Renderer>();
        _currentCube.GetComponent<Renderer>().GetPropertyBlock(_mpb);
        r.SetPropertyBlock(_mpb);

        cutCube.gameObject.SetActive(true);

        float cutSize = _absOffset; // 잘린 길이

        Vector3 cutScale = _currentCube.transform.localScale; // 현재 큐브 크기
        Vector3 cutPos = _currentCube.transform.position; // 현재 위치

        float direction = (_offset == 0) ? 1f : Mathf.Sign(_offset); // 밀린 방향

        // 잘린 방향 축 크기 설정
        if (_lastAxisIsX)
            cutScale.x = cutSize;
        else
            cutScale.z = cutSize;

        cutCube.transform.localScale = cutScale;

        // 잘린 큐브 위치 이동
        if (_lastAxisIsX)
            cutPos.x += direction * (_overlapSize / 2 + cutSize / 2);
        else
            cutPos.z += direction * (_overlapSize / 2 + cutSize / 2);

        cutCube.transform.position = cutPos;

        cutCube.CubeFall();
    }

    // =========================================================
    // 큐브 상태 체크
    // =========================================================
    public void CheckStack()
    {
        Vector3 basePos = _previousCube.transform.position;
        float baseSize = _lastAxisIsX ? _previousCube.transform.localScale.x : _previousCube.transform.localScale.z;

        Vector3 currentPos = _currentCube.transform.position;

        if (_lastAxisIsX)
            _offset = currentPos.x - basePos.x;
        else
            _offset = currentPos.z - basePos.z;

        _absOffset = Mathf.Abs(_offset);

        // Game Over 체크
        if (_absOffset >= baseSize)
        {
            if (_gameManager.IsGameOver) return;

            _currentCube.StopCube();
            _currentCube.CubeFall();

            _gameManager.GameOver();

            _lastCube = _currentCube;

            return;
        }
        else
        {
            _gameManager.AddScore(1);
        }

        // 퍼펙트 허용 범위 계산
        float perfectOffset = baseSize * _perfectThresholdRatio;

        if (_absOffset <= perfectOffset)
        {
            _offset = 0;
            _absOffset = 0;

            if (_lastAxisIsX)
                currentPos.x = basePos.x;
            else
                currentPos.z = basePos.z;

            _currentCube.transform.position = currentPos;

            // 퍼펙트시 속도 감소
            _moveSpeed -= _speedIncrease;

            //퍼펙트시 이펙트 실행
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _particleSystem.transform.position = _currentCube.transform.position;
            _particleSystem.Play();
        }
        else
        {
            // 속도 증가
            _moveSpeed += _speedIncrease;
        }

        _moveSpeed = Mathf.Clamp(_moveSpeed, _minSpeed, _maxSpeed);

        // 벗어난 범위만큼 큐브 조정
        _overlapSize = Mathf.Max(0, baseSize - _absOffset);

        //  위치 보정
        Vector3 curScale = _currentCube.transform.localScale;
        if (_lastAxisIsX)
            curScale.x = _overlapSize;
        else
            curScale.z = _overlapSize;

        _currentCube.transform.localScale = curScale;

        float centerOffset = _offset / 2f;
        Vector3 _curCubePos = _currentCube.transform.position;

        if (_lastAxisIsX)
            _curCubePos.x -= centerOffset;
        else
            _curCubePos.z -= centerOffset;

        _currentCube.transform.position = _curCubePos;

        // 컷 큐브 생성
        if (_absOffset > perfectOffset)
        {
            SpawnCutCube();
        }

        _previousCube = _currentCube.gameObject;
    }

    // =========================================================
    // 큐브 제거
    // =========================================================
    public void RemoveCube(CubeMovement cube)
    {
        cube.ResetCube();
        cube.gameObject.SetActive(false);
        _cubePools.Set(cube);
    }

    // =========================================================
    // 색상 적용
    // =========================================================
    public void ApplyColor(CubeMovement cube, Color color)
    {
        Renderer r = cube.GetComponent<Renderer>();

        r.GetPropertyBlock(_mpb);
        _mpb.SetColor("_BaseColor", color);
        r.SetPropertyBlock(_mpb);
    }

    // =========================================================
    // Start
    // =========================================================
    void Start()
    {
        _gameManager = GameManager._GM;
        _mpb = new MaterialPropertyBlock();
        _lastCube = null;

        _cubePools = new GameObjectPool<CubeMovement>(10, () =>
        {
            var obj = Instantiate(_cubePrefab, transform);
            obj.SetActive(false);
            var cube = obj.GetComponent<CubeMovement>();
            cube.InitCube(this);
            return cube;
        });

        _cubeHeight = _cubePrefab.transform.localScale.y;
        _targetY = _cameraTarget.position.y - 1;
        _BGY = _BG.position.y;

        // 타이틀 씬에서 넘어온 색 적용
        _cubeGen.SetCount(ColorState.cubeCount);
        _bgGen.SetCount(ColorState.bgCount);

        Renderer baseRenderer = _previousCube.GetComponent<Renderer>();

        _cubeColorManager.ApplyColor(baseRenderer, ColorState.cubeColor);
        _bgColorManager.ApplyColor(ColorState.bgColor);

        SpawnCube();
    }

    // =========================================================
    // Update
    // =========================================================
    void Update()
    {
        if (_gameManager.IsGameOver) return;

        Vector3 pos = _cameraTarget.position;
        pos.y = Mathf.Lerp(pos.y, _targetY, Time.deltaTime * 5f);
        _cameraTarget.position = pos;

        Vector3 pos2 = _BG.position;
        pos2.y = Mathf.Lerp(pos2.y, _BGY, Time.deltaTime * 5f);
        _BG.position = pos2;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_inputLocked) return;
            _inputLocked = true;

            if (_currentCube != null)
            {
                _currentCube.StopCube();
                CheckStack();

                if (_lastCube != null)
                {
                    _inputLocked = false;
                    return;
                }
            }
            SpawnCube();

            _inputLocked = false;
        }
    }
}