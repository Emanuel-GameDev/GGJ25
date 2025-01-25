using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponPoolSelector : MonoBehaviour
{
    int actualIndex;
    int previousIndex;

    PlayerInputs playerInputs;
    InputAction nextAction;
    InputAction previousAction;
    InputAction confirmAction;

    private GameObject[] childs = new GameObject[3];

    public int actualPlayer = 1;

    void Awake()
    {
        playerInputs = new PlayerInputs();
        nextAction = playerInputs.Selection.Next;
        previousAction = playerInputs.Selection.Back;
        confirmAction = playerInputs.Selection.Confirm;

        nextAction.performed += ctx => Next();
        previousAction.performed += ctx => Previous();
        confirmAction.performed += ctx => Confirm();

        childs[0] = transform.GetChild(0).gameObject;
        childs[1] = transform.GetChild(1).gameObject;
        childs[2] = transform.GetChild(2).gameObject;
    }



    void OnEnable()
    {
        nextAction.Enable();
        previousAction.Enable();
        confirmAction.Enable();
    }

    private void Next()
    {
        actualIndex++;
        if (actualIndex >= 3)
        {
            actualIndex = 0;
        }

        childs[previousIndex].GetComponent<Image>().color = Color.black;
        childs[actualIndex].GetComponent<Image>().color = Color.red;

        previousIndex = actualIndex;
    }



    private void Previous()
    {
        actualIndex--;
        if (actualIndex < 0)
        {
            actualIndex = 2;
        }

        childs[previousIndex].GetComponent<Image>().color = Color.black;
        childs[actualIndex].GetComponent<Image>().color = Color.red;

        previousIndex = actualIndex;
    }

    private void Confirm()
    {
        if(actualPlayer == 1)
            WeaponManager.Instance.CleanActualPoolFirstPlayer(actualIndex);
        if(actualPlayer == 2)
            WeaponManager.Instance.CleanActualPoolSecondPlayer(actualIndex);
    }

    void OnDisable()
    {
        nextAction.Disable();
        previousAction.Disable();
        confirmAction.Disable();
    }
}
