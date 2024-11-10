using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquippedGun : MonoBehaviour
{

    [SerializeField] Image gunRifle;
    [SerializeField] Image gunPistol;
    [SerializeField] Image gunShotgun;
    [SerializeField] PlayerMovementManager pmm;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (gunRifle == null || gunPistol == null || gunShotgun == null) {
            return;
        }


        if (pmm.getCurrentGunIndex() <= 0) {
            gunRifle.transform.localScale = new Vector3(1, 1, 1);
            gunPistol.transform.localScale = new Vector3(2, 2, 2);
            gunShotgun.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (pmm.getCurrentGunIndex() <= 1) {
            gunRifle.transform.localScale = new Vector3(1, 1, 1);
            gunPistol.transform.localScale = new Vector3(1, 1, 1);
            gunShotgun.transform.localScale = new Vector3(2, 2, 2);
        }
        else {
            gunRifle.transform.localScale = new Vector3(2, 2, 2);
            gunPistol.transform.localScale = new Vector3(1, 1, 1);
            gunShotgun.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
