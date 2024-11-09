using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOptions : MonoBehaviour
{

    [SerializeField] GameObject menuToClose;
    [SerializeField] GameObject menuToOpen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadOptions() {
        menuToClose.SetActive(false);
        menuToOpen.SetActive(true);
    }
}
