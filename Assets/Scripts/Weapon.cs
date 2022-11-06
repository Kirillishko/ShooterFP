using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public enum AnimState
    {
        IsAmbient,
        IsFire,
        IsHide,
        IsMelle,
        IsReady,
        IsReloadFull,
        IsReloadMag
    }

    public GameObject Ammo;
    public Transform PointSpawnAmmo;
    public Transform PointHitMelle;

    public int Damage;
    public float SpeedForce;
    public float AimingSpeed;
    public int MaxAmmo = 35;
    public int MaxClipAmmo = 7;
    public float WeaponMoveSpeed;
    public float BackRecoil;
    public Vector2 ForceRecoil;

    public float HitRadius;
    public int HitDamage;

    public Vector3 NormalPos;
    public Vector3 AimingPos;
    private Vector3 currentWeaponPos;

    private int _leftAmmo;
    private int _leftClipAmmo;

    public Animator _animator;
    private Vector3 _prevPos;

    private Camera _camera;

    private Ray ray;
    private RaycastHit hit;

    private Vector3 targetPoint;
    private Vector3 direction;

    private PlayerCamera _playerCamera;
    private ParticleSystem _particleSystem;


    private GameObject createAmmo;
    private Image _image;

    public Color _colorActive;
    public Color _colorInactive;

    private void Awake()
    {
        if (gameObject.name == "Pistol") _image = GameObject.Find("PistolImage").GetComponent<Image>();
        else if (gameObject.name == "BattleRifle") _image = GameObject.Find("BattleRifleImage").GetComponent<Image>();
        _leftAmmo = MaxAmmo;
        _leftClipAmmo = MaxClipAmmo;
    }

    public void Start()
    {
        //_colorInactive = new Color(40, 40, 40, 1);
        //if (gameObject.name == "Pistol") _image = GameObject.Find("PistolImage").GetComponent<Image>();
        //else if (gameObject.name == "BattleRifle") _image = GameObject.Find("BattleRifleImage").GetComponent<Image>();
        _particleSystem = PointSpawnAmmo.GetChild(0).GetComponent<ParticleSystem>();
        _camera = Camera.main;
        _playerCamera = transform.parent.GetComponent<PlayerCamera>();
        //_leftAmmo = MaxAmmo;
        //_leftClipAmmo = MaxClipAmmo;
        WeaponManager.instance.SetAmmoText(_leftClipAmmo, _leftAmmo);
    }

    void Update()
    {
        if (_particleSystem.gameObject.activeInHierarchy)
        {
            _particleSystem.Stop();
            _particleSystem.gameObject.SetActive(false);
        }
        transform.localPosition += new Vector3(_playerCamera.GetMouseY(), _playerCamera.GetMouseX()) * WeaponMoveSpeed / 1000; 
        Aiming();
        if (Input.GetMouseButtonDown(0) && _leftClipAmmo != 0 && !_animator.GetBool(AnimState.IsFire.ToString()))
        {
            //Debug.Log($"LeftClipAmmo = {_leftClipAmmo}");
            _animator.SetBool(AnimState.IsFire.ToString(), true);
        }

        if (Input.GetKeyDown(KeyCode.R) && _leftClipAmmo != MaxClipAmmo && _leftAmmo != 0)
        {
            if (_leftClipAmmo == 0)
            {
                _animator.SetBool(AnimState.IsReloadFull.ToString(), true);
            }
            else _animator.SetBool(AnimState.IsReloadMag.ToString(), true);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            bool isHide = !_animator.GetBool(AnimState.IsHide.ToString());

            _animator.SetBool(AnimState.IsHide.ToString(), isHide);
        }

        if (Input.GetKeyDown (KeyCode.V))
        {
            _animator.SetBool(AnimState.IsMelle.ToString(), true);
        }


        bool ambient = !_animator.GetBool(AnimState.IsAmbient.ToString()) && _prevPos != transform.position;
        if (!_animator.GetBool(AnimState.IsAmbient.ToString()) && _prevPos != transform.position)
        {
            _animator.SetBool(AnimState.IsAmbient.ToString(), true);
        }
        else if (_animator.GetBool(AnimState.IsAmbient.ToString()) && _prevPos == transform.position)
        {
            _animator.SetBool(AnimState.IsAmbient.ToString(), false);
        }
    }

    public void EndAnim(AnimState anim)
    {
        if (anim == AnimState.IsFire)
        {
        }
        _animator.SetBool(anim.ToString(), false);
    }

    public void Shoot()
    {
        ray = _camera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(60);
        }
        direction = targetPoint - PointSpawnAmmo.position;

        createAmmo = Instantiate(Ammo, PointSpawnAmmo.position, transform.rotation);

        createAmmo.GetComponent<Rigidbody>().AddForce(direction.normalized * SpeedForce, ForceMode.Impulse);
        createAmmo.GetComponent<Bullet>().SetDamage(Damage);


        _leftClipAmmo--;
        WeaponManager.instance.SetAmmoText(_leftClipAmmo, _leftAmmo);

        Recoil();
        _particleSystem.gameObject.SetActive(true);
        _particleSystem.Play();
    }

    private void Recoil()
    {
        transform.localPosition -= Vector3.forward * -BackRecoil;

        float xRecoil = Random.Range(-ForceRecoil.x, ForceRecoil.x);
        float yRecoil = Random.Range(-ForceRecoil.y, ForceRecoil.y);

        _playerCamera.SetCurrentRotation(new Vector2(xRecoil, yRecoil));

    }

    private void Aiming()
    {
        currentWeaponPos = NormalPos;
        if (Input.GetMouseButton(1)) currentWeaponPos = AimingPos;

        transform.localPosition = Vector3.Lerp(transform.localPosition, currentWeaponPos, Time.deltaTime * AimingSpeed);
    }

    public void HitMelle()
    {
        Collider[] hitColliders = Physics.OverlapSphere(PointSpawnAmmo.position, HitRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                hitCollider.GetComponent<EnemyHP>().TakeDamage(HitDamage);
                return;
            }
        }
    }

    public void Reload()
    {
        if (_leftAmmo < MaxClipAmmo - _leftClipAmmo)
        {
            _leftClipAmmo += _leftAmmo;
            _leftAmmo = 0;
        }
        else
        {
            _leftAmmo -= MaxClipAmmo - _leftClipAmmo;
            _leftClipAmmo = MaxClipAmmo;
        }

        WeaponManager.instance.SetAmmoText(_leftClipAmmo, _leftAmmo);
    }

    public int GetLeftAmmo()
    {
        return _leftAmmo;
    }
    
    public int GetLeftClipAmmo()
    {
        return _leftClipAmmo;
    }

    public void SetImageColor(bool isActive)
    {
        if (isActive) _image.color = _colorActive;
        else _image.color = _colorInactive;
    }

    public void AddLeftAmmo()
    {
        _leftAmmo += MaxClipAmmo;
    }
}
