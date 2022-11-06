using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;
    public List<Weapon> Weapons = new List<Weapon>();
    private Text _clipAmmo;
    private Text _ammo;
    public int StartWeaponIndex;

    private int _weaponIndex = 0;
    private int _nextWeaponIndex = 0;

    private void Awake()
    {
        instance = this;
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(this.gameObject);
        //}
        //else Destroy(this.gameObject);
    }

    void Start()
    {
        _clipAmmo = GameObject.Find("LeftClipAmmoText").GetComponent<Text>();
        _ammo = GameObject.Find("LeftAmmoText").GetComponent<Text>();


        for (int i = 0; i < Weapons.Count; i++)
        {
            //Debug.Log(Weapons[i].gameObject.name);
            if (i == StartWeaponIndex && !Weapons[i].gameObject.activeInHierarchy)
            {
                //Debug.Log("True");
                Weapons[i].gameObject.SetActive(true);
            }
            if (i != StartWeaponIndex)
            {
                //Debug.Log("False");
                Weapons[i].gameObject.SetActive(false);
            }
        }

        _weaponIndex = _nextWeaponIndex = StartWeaponIndex;
        //Weapons[i].gameObject.SetActive(true);
        //for (int i = 1; i < Weapons.Count; i++)
        //{
        //    Weapons[i].gameObject.SetActive(false);
        //}
        //_ammo = GameObject.Find("Ammo").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && _nextWeaponIndex != 0 && Weapons.Count != 1)
        {
            _nextWeaponIndex = 0;
            StartCoroutine(SetWeapon());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && _nextWeaponIndex != 1 && Weapons.Count != 1)
        {
            _nextWeaponIndex = 1;
            StartCoroutine(SetWeapon());

        }
    }

    public void GetNewWeapon()
    {
        Weapons.Add(transform.GetChild(0).GetChild(1).GetComponent<Weapon>());
        _nextWeaponIndex = 1;
        StartCoroutine(SetWeapon());
        //Weapons.Add(weapon);
        //Weapon temp = Weapons[0];
        //Weapons[0] = Weapons[1];
        //Weapons[1] = temp;
    }

    IEnumerator SetWeapon()
    {
        Weapons[_weaponIndex]._animator.SetBool(Weapon.AnimState.IsHide.ToString(), true);
        yield return new WaitForSeconds(0.6f);

        Weapons[_weaponIndex].gameObject.SetActive(false);
        Weapons[_weaponIndex].SetImageColor(false);


        Weapons[_nextWeaponIndex].gameObject.SetActive(true);
        Weapons[_nextWeaponIndex].SetImageColor(true);

        SetAmmoText(Weapons[_nextWeaponIndex].GetLeftClipAmmo(), Weapons[_nextWeaponIndex].GetLeftAmmo());

        _weaponIndex = _nextWeaponIndex;
    }

    public void SetAmmoText(int leftClipAmmo, int leftAmmo)
    {
        _clipAmmo.text = leftClipAmmo.ToString();
        _ammo.text = leftAmmo.ToString();
    }

    public void TakeAmmo()
    {
        for(int i = 0; i < Weapons.Count; i++)
        {
            Weapons[i].AddLeftAmmo();
        }
        SetAmmoText(Weapons[_weaponIndex].GetLeftClipAmmo(), Weapons[_weaponIndex].GetLeftAmmo());
    }
}
