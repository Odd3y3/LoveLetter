using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CardBase
{
    public int Num { get; protected set; }
    public bool IsRequireTargetPlayer { get; protected set; } = false;
    public bool IsRequireOptionNum { get; protected set; } = false;

    public virtual void Use()
    {

    }
}
