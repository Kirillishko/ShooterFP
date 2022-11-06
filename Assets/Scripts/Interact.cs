using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    class Interactables
    {
        public string Door = "Открыть дверь";
        public string Ammo = "Подобрать патроны";
        public string Healer = "Подобрать аптечку";
        public string Weapon = "Подобрать оружие";
        public string Finish = "Закончить уровень";
    }

    private Text _text;
    private Ray ray;
    private RaycastHit hit;
    private PlayerHP _HP;

    private Camera _camera;
    private GameObject _canvas;

    private Interactables interactables = new Interactables();

    private void Awake()
    {
        _canvas = GameObject.Find("GoodCanvas");
        _canvas.SetActive(false);
    }

    private void Start()
    {
        _camera = Camera.main;
        _HP = transform.parent.GetComponent<PlayerHP>();
        _text = GameObject.Find("InteractText").GetComponent<Text>();
    }

    void Update()
    {
        ray = _camera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(ray, out hit, 5f))
        {
            if (hit.transform.CompareTag("Door"))
            {
                _text.text = interactables.Door;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.collider.enabled = false;
                    _text.text = null;
                    hit.transform.GetComponent<Door>().Open();
                }
            }
            else if (hit.transform.CompareTag("Ammo"))
            {
                _text.text = interactables.Ammo;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _text.text = null;
                    WeaponManager.instance.TakeAmmo();
                    Destroy(hit.transform.gameObject);
                }
            }
            else if (hit.transform.CompareTag("FakeWall") && Input.GetKeyDown(KeyCode.E))
            {
                hit.transform.GetComponent<FakeWall>().Move(transform.position);
            }
            else if (hit.transform.CompareTag("Healer"))
            {
                _text.text = interactables.Healer;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _text.text = null;
                    _HP.TakeHeal(hit.transform.GetComponent<Healer>().GetHealHP());
                    Destroy(hit.transform.gameObject);
                }
            }
            else if (hit.transform.CompareTag("Weapon"))
            {
                _text.text = interactables.Weapon;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _text.text = null;
                    WeaponManager.instance.GetNewWeapon();
                }
            }
            else if (hit.transform.CompareTag("Finish"))
            {
                _text.text = interactables.Finish;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //Debug.Log($"{ScoutingDrone.EnemiesLeft} / {ScoutingDrone.AllEnemies}");
                    _canvas.SetActive(true);
                    _canvas.transform.GetChild(1).GetComponent<Text>().text = $"{ScoutingDrone.AllEnemies - ScoutingDrone.EnemiesLeft} / {ScoutingDrone.AllEnemies}";
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                }
            }
            else
            {
                _text.text = null;
            }
        }
    }
}
