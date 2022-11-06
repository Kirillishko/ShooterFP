using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHP : MonoBehaviour
{
    public int MaxHP;
    private int _currentHP;
    private Slider _hpSlider;
    PostProcessVolume _volume;
    Vignette _vignette;
    private GameObject _canvas;

    private void Awake()
    {
        _canvas = GameObject.Find("BadCanvas");
        _canvas.SetActive(false);
    }
    void Start()
    {
        //_volume.profile.TryGetSettings(out _vignette);
        _hpSlider = GameObject.FindGameObjectWithTag("HP").GetComponent<Slider>();
        _hpSlider.maxValue = MaxHP;
        _hpSlider.value = MaxHP;

        _currentHP = MaxHP;
    }

    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            _canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        _hpSlider.value = _currentHP;
        //StartCoroutine(Mask());
    }

    public void TakeHeal(int heal)
    {
        _currentHP += heal;
        if (_currentHP > MaxHP) _currentHP = MaxHP;
        _hpSlider.value = _currentHP;
    }

    IEnumerator Mask()
    {
        _vignette.intensity.value = _currentHP;
        yield return new WaitForSeconds(0.1f);
        _vignette.intensity.value = 0;
    }
}
