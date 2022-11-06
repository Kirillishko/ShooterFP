using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class bot : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Vector3 _startPos;
    private Vector3 _endPos;
    private bool _isMoving;
    private bool _canGo;
    private NavMeshPath _path;

    void Start()
    {
        _startPos = transform.position;
        _path = new NavMeshPath();
        _agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        if (!_isMoving)
        {
            _endPos = new Vector3(_startPos.x + Random.Range(-5.0f, 5.0f),
                _startPos.y - 1, _startPos.z + Random.Range(-5.0f, 5.0f));

            if (_agent.CalculatePath(_endPos, _path))
            {
                Collider[] hitColliders = Physics.OverlapSphere(_endPos, 1f, 7);
                if (hitColliders.Length == 0)
                {
                    _canGo = true;
                }

                if (_canGo)
                {
                    _agent.SetDestination(_endPos);
                    _isMoving = true;
                }
            }
        }
        else
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction);

            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.transform.CompareTag("Door"))
                {
                    hit.transform.GetComponent<Door>().Open();
                }
            }

            if ((_endPos - transform.position).magnitude <= 2f) _isMoving = false;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_endPos, 2);
    }


}
