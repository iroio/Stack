using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeSpawnManager : MonoBehaviour
{
    CubeMovement _currentCube;

    [SerializeField] Transform _target;
    [SerializeField] GameObject _cubePrefab;

    [SerializeField] Transform[] _spawnPoint;

    GameObjectPool<CubeMovement> _cubePools;

    Vector3 _dir;
    Vector3 _height;
    Vector3 _pos;

    float _targetY;

    bool _isX = true;

    public void SpawnCube()
    {
        var cube = _cubePools.Get();
        cube.gameObject.SetActive(true);

        _height.y += 1.4f;

        if (_isX )
            cube.transform.position = _spawnPoint[0].position + _height;
        else 
            cube.transform.position = _spawnPoint[1].position + _height;

        _dir = _isX ? Vector3.left : Vector3.back;
        _isX = !_isX;

        _currentCube = cube;

        cube.CubeMove(_dir, 5f);
        _targetY += 1.4f;
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

        _targetY = _target.position.y - 1;
        SpawnCube();
    }

    void Update()
    {
        _pos = _target.position;
        _pos.y = Mathf.Lerp(_pos.y, _targetY, Time.deltaTime);
        _target.position = _pos;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_currentCube != null)
            {
                _currentCube.StopCube();
            }

            SpawnCube();
        }
    }
}
