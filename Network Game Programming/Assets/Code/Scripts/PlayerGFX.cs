using UnityEngine;

public class PlayerGFX : MonoBehaviour
{
    // Note: Currently is very dependant on the order of things in the hierarchy. Change to be less dependant on this (E.g. Using a dictionary with enums)
    // We are having the prefabs contain all options so that we can have custom positions per frame.

    [Header("Container References")]
    [SerializeField] private Transform _legsContainer;
    [SerializeField] private Transform[] _weaponsAttachPoints;
    [SerializeField] private Transform _abilityContainer;


    [Header("Testing")]
    [SerializeField] private bool _updateMechSelection = true;
    [SerializeField] private bool _finaliseMechSelection = false;

    [Space(5)]
    [SerializeField] private int _selectedLegIndex = 0;
    [SerializeField] private int[] _selectedWeaponIndicies = new int[0];
    [SerializeField] private int _selectedAbilityIndex = 0;


    private void Awake()
    {
        EnableSelectedLeg(_selectedLegIndex);
        for (int i = 0; i < _selectedWeaponIndicies.Length; ++i)
            EnableSelectedWeapon(i, _selectedWeaponIndicies[i]);
        EnableSelectedAbility(_selectedAbilityIndex);
    }
    private void Update()
    {
        if (_finaliseMechSelection)
        {
            _finaliseMechSelection = false;
            FinaliseFrameGFX();
            Destroy(this);
            return;
        }

        if (_updateMechSelection)
        {
            EnableSelectedLeg(_selectedLegIndex);
            for(int i = 0; i < _selectedWeaponIndicies.Length; ++i)
                EnableSelectedWeapon(i, _selectedWeaponIndicies[i]);
            EnableSelectedAbility(_selectedAbilityIndex);
        }
    }


    public void EnableSelectedLeg(int selectedLegIndex)
    {
        if (selectedLegIndex >= _legsContainer.childCount)
            return;

        for (int i = 0; i < _legsContainer.childCount; ++i)
        {
            _legsContainer.GetChild(i).gameObject.SetActive(i == selectedLegIndex);
        }
    }
    public void EnableSelectedWeapon(int weaponSlot, int selectedWeaponIndex)
    {
        if (weaponSlot >= _weaponsAttachPoints.Length || selectedWeaponIndex >= _weaponsAttachPoints[weaponSlot].childCount)
            return;

        for (int i = 0; i < _weaponsAttachPoints[weaponSlot].childCount; ++i)
        {
            _weaponsAttachPoints[weaponSlot].GetChild(i).gameObject.SetActive(i == selectedWeaponIndex);
        }
    }
    public void EnableSelectedAbility(int selectedAbilityIndex)
    {
        if (selectedAbilityIndex >= _abilityContainer.childCount)
            return;

        for (int i = 0; i < _abilityContainer.childCount; ++i)
        {
            _abilityContainer.GetChild(i).gameObject.SetActive(i == selectedAbilityIndex);
        }
    }

    public void FinaliseFrameGFX()
    {
        // Remove all non-chosen legs.
        for(int i = 0; i < _legsContainer.childCount; ++i)
        {
            if (TestAndDestroyGameobject(_legsContainer.GetChild(i).gameObject))
            {
                continue;
            }
        }

        // Remove all non-chosen weapons.
        for(int i = 0; i < _weaponsAttachPoints.Length; ++i)
        {
            for (int j = 0; j < _weaponsAttachPoints[i].childCount; ++j)
            {
                if (TestAndDestroyGameobject(_weaponsAttachPoints[i].GetChild(j).gameObject))
                {
                    continue;
                }
            }
        }

        // Remove all non-chosen abilities.
        for (int i = 0; i < _abilityContainer.childCount; ++i)
        {
            if (TestAndDestroyGameobject(_abilityContainer.GetChild(i).gameObject))
            {
                continue;
            }
        }
    }

    // If the passed GameObject is inactive, destroys it and returns true.
    private bool TestAndDestroyGameobject(GameObject objectToTest)
    {
        if (!objectToTest.activeSelf)
        {
            // The passed GameObject is disabled. Destroy it.
            Destroy(objectToTest);
            return true;
        }

        // The passed GameObject is enabled.
        return false;
    }
}
