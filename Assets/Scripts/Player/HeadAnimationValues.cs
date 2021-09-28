using System;
using UnityEngine;

public class HeadAnimationValues : MonoBehaviour
{
    public static event Action<bool> IsLanding;

    public void SetLandingBool()
    {
        IsLanding?.Invoke(false);
    }
}
