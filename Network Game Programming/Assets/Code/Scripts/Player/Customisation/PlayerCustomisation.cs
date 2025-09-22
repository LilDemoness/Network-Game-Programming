using UnityEngine;

public class PlayerCustomisation : MonoBehaviour
{
    public event System.Action<FrameData> OnSelectedFrameChanged;
    public event System.Action<LegsData> OnSelectedLegChanged;
    public event System.Action<int, WeaponData> OnSelectedWeaponChanged;
    public event System.Action<AbilityData> OnSelectedAbilityChanged;

    public event System.Action<FrameData, LegsData, WeaponData[], AbilityData> OnFinalisedCustomisation;


    [Header("Testing")]
    [SerializeField] private bool _updateGFX;
    [SerializeField] private bool _finaliseGFX;

    [Space(5)]
    [SerializeField] private FrameData _activeFrame;
    [SerializeField] private LegsData _activeLeg;
    [SerializeField] private WeaponData[] _activeWeapons;
    [SerializeField] private AbilityData _activeAbility;



    private void Update()
    {
        if (_finaliseGFX)
        {
            OnFinalisedCustomisation.Invoke(_activeFrame, _activeLeg, _activeWeapons, _activeAbility);
            _finaliseGFX = false;
            Destroy(this);
            return;
        }


        if (_updateGFX)
        {
            OnSelectedFrameChanged.Invoke(_activeFrame);
            OnSelectedLegChanged.Invoke(_activeLeg);
            for (int i = 0; i < _activeWeapons.Length; ++i)
                OnSelectedWeaponChanged.Invoke(i, _activeWeapons[i]);
            OnSelectedAbilityChanged.Invoke(_activeAbility);
            _updateGFX = false;
        }
    }
}
