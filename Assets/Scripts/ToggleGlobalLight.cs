using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGlobalLight : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject globalLight;
    private bool isActive;
    void Start()
    {
        globalLight = GameObject.Find("Global Light 2D");
        isActive = false;
        globalLight.SetActive(isActive);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            isActive = !isActive;
            globalLight.SetActive(isActive);
        }
    }
}
