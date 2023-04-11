using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrbType
{
    healthOrb,
    weaponOrb,
    armorOrb,
    movementOrb,
    equipmentOrb
}

public class Orb : MonoBehaviour
{
    public OrbType orbTypeID;

    PlayerProgress playerProgress;

    [SerializeField] int orbValue;

    // Start is called before the first frame update
    void Awake()
    {
        playerProgress = FindObjectOfType<PlayerProgress>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerProgress>())
        {
            switch (orbTypeID)
            {
                case OrbType.healthOrb:
                    playerProgress.TotalHealthOrbs += orbValue;
                    break;
                case OrbType.weaponOrb:
                    playerProgress.TotalWeaponOrbs += orbValue;
                    break;
                case OrbType.armorOrb:
                    playerProgress.TotalArmorOrbs += orbValue;
                    break;
                case OrbType.movementOrb:
                    playerProgress.TotalMovementOrbs += orbValue;
                    break;
                case OrbType.equipmentOrb:
                    playerProgress.TotalEquipmentOrbs += orbValue;
                    break;
                default:
                    break;
            }

            Destroy(gameObject);
        }
    }
}
