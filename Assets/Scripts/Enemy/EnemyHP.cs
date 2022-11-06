using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    public int MaxHP;
    private int _currentHP;
    private Slider _hpSlider;

    void Start()
    {
        _hpSlider = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Slider>();
        _hpSlider.maxValue = MaxHP;
        _hpSlider.value = MaxHP;

        _currentHP = MaxHP;
    }

    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            ScoutingDrone.EnemiesLeft--;
            //Debug.Log($"{ScoutingDrone.AllEnemies - ScoutingDrone.EnemiesLeft} / {ScoutingDrone.AllEnemies}");
            Destroy(this.gameObject);
        }
        _hpSlider.value = _currentHP;

        if (gameObject.name == "Scouting Drone(Clone)")
            gameObject.GetComponent<ScoutingDrone>().SetDestination();
    }
}
