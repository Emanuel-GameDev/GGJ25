using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : Player
{
    #region Vars

    #region Inputs

    private InputActionAsset _inputAsset;
    private InputActionMap _playerMap;
    private InputAction _shootAction;

    #endregion

    #region Weapons
    [SerializeField] List<BaseWeapon> weaponsEquipped;

    #endregion

    #region Passives
    // Lista di passive
    #endregion

    #endregion

    #region UnityFunctions

    private void Awake()
    {
        _inputAsset = GetComponent<PlayerInput>().actions;
        _playerMap = _inputAsset.FindActionMap("Player");
        _shootAction = _playerMap.FindAction("Shoot");
    }

    void OnEnable()
    {
        _shootAction.performed += ShootAll;
        _shootAction.Enable();
    }

    void OnDisable()
    {
        _shootAction.performed -= ShootAll;
        _shootAction.Disable();

    }

    #endregion

    #region Weapons

    private void ShootAll(InputAction.CallbackContext context)
    {
        foreach (BaseWeapon weapon in weaponsEquipped)
        {
            weapon.Shoot();
        }
    }

    public void EquipWeapon(BaseWeapon weaponToEquip)
    {
        weaponToEquip.playerHandler = this;
        weaponsEquipped.Add(weaponToEquip);
    }

    public void UnEquipWeapon(BaseWeapon weapon)
    {
        // Non si usa per ora ma non si sa mai
    }

    #endregion
}
