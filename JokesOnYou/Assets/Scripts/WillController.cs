using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillController : MonoBehaviour
{
    [SerializeField] ChrisController Chris;

    public void HitReached()
    {
        Chris.GotPunched();
    }
}
