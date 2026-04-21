using UnityEngine;
using UnityEngine.InputSystem;

public class CubeMovement : MonoBehaviour
{
    CubeSpawnManager _manager;

    [SerializeField] float _speed = 1f;

    Vector3 _dir;

    bool _isMoving = true;

    public void InitCube(CubeSpawnManager manager)
    {
        _manager = manager;
    }

    public void CubeMove(Vector3 dir, float speed)
    {
        _dir = dir.normalized;
        _speed = speed;
        _isMoving = true;
    }

    public void StopCube()
    {
        _isMoving = false;
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
            transform.position += _dir * _speed * Time.deltaTime;
        }
    }
}
