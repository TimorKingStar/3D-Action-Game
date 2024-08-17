using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float HorizontalValue;
    public float VerticalValue;

    public bool MouseButtonDown;

    public bool SpaceButtonDown;

    void Update()
    {
        if (!MouseButtonDown && Time.timeScale!=0)
        {
            MouseButtonDown = Input.GetMouseButtonDown(1);
        }
         
        if (!SpaceButtonDown && Time.timeScale!=0)
        {
            SpaceButtonDown = Input.GetKeyDown(KeyCode.Space);
        }

        HorizontalValue = Input.GetAxis("Horizontal");
        VerticalValue = Input.GetAxis("Vertical");
    }

    private void OnDisable()
    {
        SpaceButtonDown = false;
        MouseButtonDown = false;
        HorizontalValue = 0;
        VerticalValue = 0;
    }


    public void ClearCache()
    {
        SpaceButtonDown = false;
        MouseButtonDown = false;
        HorizontalValue = 0;
        VerticalValue = 0;
    }

}
