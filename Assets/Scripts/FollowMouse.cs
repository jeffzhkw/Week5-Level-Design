using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public Camera cam;

    private static Vector2 mousePos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 currPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 lookDir = mousePos - currPos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;//angle in rad between x axis and the 2D vector;
        transform.rotation = Quaternion.Euler(0, 0, angle-90);
    }
}
