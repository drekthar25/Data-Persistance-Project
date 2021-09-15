using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public MainManager Manager;

    void Update()
    {
        Manager = MainManager.Instance;
    }
    private void OnCollisionEnter(Collision other)
    {
        Destroy(other.gameObject);
      
        Manager.GameOver();
    }
}
