using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillController : HumanActorController
{
    [SerializeField] ChrisController Chris;

    public override void HitReached()
    {
        Chris.GotPunched();
    }
}
