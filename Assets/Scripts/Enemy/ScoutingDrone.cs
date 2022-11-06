using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ScoutingDrone : MonoBehaviour
{
    [Header("Shooting")]
    public static int AllEnemies;
    public static int EnemiesLeft;
    public int Damage;
    public float AmmoSpeed;
    public float RateOfFire;
    public GameObject Ammo;
    public Transform PointSpawnAmmo;

    private bool _corIsWorking = false;
    private bool _isShooting = false;

    [Header("EyeColor")]
    [ColorUsage(true, true)] public Color CalmColor;
    [ColorUsage(true, true)] public Color AgressiveColor;

    private Material _material;
    private float _colorTransition = 0;
    private float _colorTransitionStep = 0.01f;

    [Header ("NavMesh")]
    public float RadiusOfDetection;
    public LayerMask LayerMask;

    private Transform _player;
    private NavMeshAgent _navMeshAgent;
    private bool _isMoving = false;
    private Vector3 _startPos;
    private Vector3 _endPos;
    private Vector3 _prevPos;
    private RaycastHit _raycastHit;
    private bool _canGo;
    private NavMeshPath _path;

    private Ray ray;
    private RaycastHit hit;

    void Start()
    {
        EnemiesLeft++;
        AllEnemies++;
        //Debug.Log($"{EnemiesLeft} / {AllEnemies}");
        _path = new NavMeshPath();
        _startPos = transform.position;
        StartCoroutine(SetPlayer());
        _material = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (_player != null)
        {
            if (Vector3.Distance(transform.position, _player.position) <= RadiusOfDetection)
            {
                ChangeEmissionColor(true);
                _navMeshAgent.SetDestination(_player.position);
                if (Vector3.Distance(transform.position, _player.position) <= _navMeshAgent.stoppingDistance)
                {
                    transform.LookAt(_player.position);
                    if (!_corIsWorking) Shooting();
                }
                else if (_corIsWorking) _corIsWorking = false;
            }
            else
            {
                ChangeEmissionColor(false);
                if (!_isMoving)
                {
                    _endPos = new Vector3(_startPos.x + Random.Range(-5.0f, 5.0f),
                        _startPos.y - 1, _startPos.z + Random.Range(-5.0f, 5.0f));

                    if (_navMeshAgent.CalculatePath(_endPos, _path))
                    {
                        Collider[] hitColliders = Physics.OverlapSphere(_endPos, 1f, 7);
                        if (hitColliders.Length == 0)
                        {
                            _canGo = true;
                        }

                        if (_canGo)
                        {
                            _navMeshAgent.SetDestination(_endPos);
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
        }
    }

    public void SetDestination()
    {
        _navMeshAgent.SetDestination(_player.position);
    }

    private void ChangeEmissionColor(bool toAgressive)
    {
        if (toAgressive)
        {
            if (_colorTransition < 1)
            {
                _colorTransition += _colorTransitionStep;
                //Debug.Log($"ColorTransition {_colorTransition}");
            }
        }
        else
        {
            if (_colorTransition > 0)
            {
                _colorTransition -= _colorTransitionStep;
                //Debug.Log($"ColorTransition {_colorTransition}");
            }
        }

        _material.SetColor("_EmissionColor", Color.Lerp(CalmColor, AgressiveColor, _colorTransition));
    }

    IEnumerator SetPlayer()
    {
        yield return new WaitForSeconds(2);
        _player = GameObject.Find("Player").GetComponent<Transform>();
    }

    IEnumerator Shoot()
    {
        _corIsWorking = true;
        while (_corIsWorking)
        {
            if (!_isShooting)
            {
                Vector3 direction = _player.position - PointSpawnAmmo.position;

                GameObject createAmmo = Instantiate(Ammo, PointSpawnAmmo.position, transform.rotation);

                createAmmo.transform.Rotate(transform.rotation.x + 90, transform.rotation.y, transform.rotation.z);
                createAmmo.GetComponent<Rigidbody>().AddForce(direction.normalized * AmmoSpeed, ForceMode.Impulse);
                createAmmo.GetComponent<Bullet>().SetDamage(Damage);
                StartCoroutine(ShootDelay());
                yield return new WaitForSeconds(0);
            }
        }
    }

    private void Shooting()
    {
        if (!_isShooting)
        {
            Vector3 direction = _player.position - PointSpawnAmmo.position;

            GameObject createAmmo = Instantiate(Ammo, PointSpawnAmmo.position, transform.rotation);

            createAmmo.transform.Rotate(transform.rotation.x + 90, transform.rotation.y, transform.rotation.z);
            createAmmo.GetComponent<Rigidbody>().AddForce(direction.normalized * AmmoSpeed, ForceMode.Impulse);
            createAmmo.GetComponent<Bullet>().SetDamage(Damage);
            StartCoroutine(ShootDelay());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_endPos, 2);
    }

    IEnumerator ShootDelay()
    {
        _isShooting = true;
        yield return new WaitForSeconds(RateOfFire);
        _isShooting = false;
    }

    //IEnumerator IsMove()
    //{
    //    _prevPos = transform.position;
    //    yield return new WaitForSeconds(2);
    //    if (_prevPos == transform.position) _isMoving = false;
    //    //else Debug.Log(1);
    //}

}
