using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Door : IPassiveDevice
{
    // Start is called before the first frame update
    protected override void isPowered()
    {
        GetComponent<Transform>().position.Set(GetComponent<Transform>().position.x, 20000f, 20f);

    }
    protected override void isDepowered()
    {
        GetComponent<Transform>().position.Set(GetComponent<Transform>().position.x, 20000f, -2f);
    }
}