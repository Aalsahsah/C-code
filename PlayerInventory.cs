using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //Variables used for gun swapping
    [SerializeField] GameObject[] weaponsHeld;
    private int weaponUsing;

    // Start is called before the first frame update
    void Start()
    {
        //Sets gun to primary
        weaponUsing = 0;
        weaponsHeld[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Toggle Weapon") && !GunFunctionality.isReloading)
        {
            ToggleWeapon();
        }
    }

    void ToggleWeapon()
    {
        //Toggles weapon through array.  Makes sure it never goes out of range
        if(weaponUsing < weaponsHeld.Length - 1)
        {
            weaponUsing++;
            weaponsHeld[weaponUsing].SetActive(true);
            weaponsHeld[weaponUsing - 1].SetActive(false);
        }
        else
        {
            //Sets weapon back at start after going to final weapon
            weaponUsing = 0;
            weaponsHeld[weaponUsing].SetActive(true);
            weaponsHeld[weaponsHeld.Length - 1].SetActive(false);
        }
    }
}
