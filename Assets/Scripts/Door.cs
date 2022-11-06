using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Transform _playerPos;

    private bool _isEnd = false;
    private Animator _anim;
    private Collider _colider;

    void Start()
    {
        _colider = GetComponent<Collider>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (IsOpen && Vector3.Distance(transform.position, Player.gameObject.transform.position) <= 3 && !isEnd)
        //{
        //    anim.SetFloat("Speed", 1.0f);

        //}
        if (_playerPos != null && Vector3.Distance(transform.position, _playerPos.position) > 5 && _isEnd)
        {
            _anim.SetFloat("Speed", -1.5f);
        }

    }

    public void Open()
    {
        if (_playerPos == null) _playerPos = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.transform;
        _anim.SetFloat("Speed", 1.5f);
    }

    public void StopOnOpen()
    {
        if (_anim.GetFloat("Speed") == 1.5f) _anim.SetFloat("Speed", 0f);
        _isEnd = true;
    }

    public void StopOnClose()
    {
        if (_anim.GetFloat("Speed") == -1.5f)
        {
            _anim.SetFloat("Speed", 0f);
            if (_colider.enabled == false) _colider.enabled = true;
        }
        _isEnd = false;
    }



}