using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeWall : MonoBehaviour
{
    private Vector3 _direction;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector3 playerPos)
    {
        _direction = transform.position - playerPos;
        _direction.y = 0;
        if (_direction.x > _direction.z)
        {
            _direction.x = 2;
            _direction.z = 0;
        }
        else
        {
            _direction.x = 0;
            _direction.z = 2;
        }
        //Debug.Log($"direction = {_direction}");
        //while ()
        //transform.position = Vector3.Lerp(playerPos, _direction, 3);
        StartCoroutine(Moving());
        //for (int i = 20; i > 0; i--)
        //{
        //    transform.Translate(_direction/i);
        //}
    }

    IEnumerator Moving()
    {
        transform.position = Vector3.Lerp(transform.position, _direction, 3f);
        yield return null;
    }
}
