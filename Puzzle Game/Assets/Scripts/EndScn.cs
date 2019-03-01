using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScn : MonoBehaviour
{
    public int NumberOfImage;
    public static int picInPlace;

    void Update()
    {

        if (picInPlace == NumberOfImage)
        {
            Debug.Log("Kazandın!");

        }
    }
}