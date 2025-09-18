using TMPro;
using UnityEngine;

public class CardUiManager : MonoBehaviour
{
    public TextMeshPro EnergyCounter;
    public TextMeshPro ShieldCount;

    public void UpdateEnergy(int energy)
    {
        EnergyCounter.text = energy.ToString();
    }

    public void UpdateShield(int shielding)
    {
        ShieldCount.text = shielding.ToString();
    }
}
