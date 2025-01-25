using UnityEngine;
using UnityEngine.UI;

public class BarsUI : MonoBehaviour
{
    public Slider healthBarP1;
    public Slider expBarP1;
    public Slider oxygenBarP1;

    public Slider healthBarP2;
    public Slider expBarP2;
    public Slider oxygenBarP2;

    public static BarsUI instance;


    private int _player1ID = 0;
    private int _player2ID = 1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SetHealth(float health, int ID)
    {
        if (ID == _player1ID)
            healthBarP1.value = health;
        else if (ID == _player2ID)
            healthBarP2.value = health;
    }

    public void SetExp(float exp, int ID)
    {
        if (ID == _player1ID)
            expBarP1.value = exp;
        else if (ID == _player2ID)
            expBarP2.value = exp;
    }

    public void SetOxygen(float oxygen, int ID)
    {
        if (ID == _player1ID)
            oxygenBarP1.value = oxygen;
        else if (ID == _player2ID)
            oxygenBarP1.value = oxygen;
    }

    public void SetMaxHealth(float health, int ID)
    {
        if (ID == _player1ID)
            healthBarP1.maxValue = health;
        else if (ID == _player2ID)
            healthBarP2.maxValue = health;


        if (ID == _player1ID)
            healthBarP1.value = health;
        else if (ID == _player2ID)
            healthBarP2.value = health;
    }

    public void SetMaxOxygen(float oxygen, int ID)
    {
        if (ID == _player1ID)
            oxygenBarP1.maxValue = oxygen;
        else if (ID == _player2ID)
            oxygenBarP1.maxValue = oxygen;


        if (ID == _player1ID)
            oxygenBarP1.value = oxygen;
        else if (ID == _player2ID)
            oxygenBarP1.value = oxygen;
    }
}
