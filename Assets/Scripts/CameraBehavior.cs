using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public GameObject playerObj;
    public float minX, maxX; //set after level design
    public float minY, maxY;

    private Vector3 offset;
    void Start()
    {
        offset = transform.position - playerObj.transform.position;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desirePos = playerObj.transform.position + offset;
        desirePos = new Vector3(
            Mathf.Clamp(desirePos.x, minX, maxX),
            Mathf.Clamp(desirePos.y, minY, maxY),
            desirePos.z);
        Vector3 smoothPos = Vector3.Lerp(transform.position, desirePos, 0.125f);
        transform.position = smoothPos;

    }
}