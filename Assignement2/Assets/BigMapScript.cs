using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigMapScript : MonoBehaviour {

    [SerializeField] RawImage miniMap;
    [SerializeField] RawImage bigMap;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (bigMap != null) {
                bigMap.enabled = !bigMap.enabled;
                miniMap.enabled = !bigMap.enabled;
            }
        }
    }
}
