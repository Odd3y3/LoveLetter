using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card1 : CardBase
{
    public Card1()
    {
        this.Num = 1;
        IsRequireTargetPlayer = true;
        IsRequireOptionNum = true;
    }

    public override void Use()
    {
        base.Use();
    }
}
public class Card2 : CardBase
{
    public Card2()
    {
        this.Num = 2;
        IsRequireTargetPlayer = true;
    }
}
public class Card3 : CardBase
{
    public Card3()
    {
        this.Num = 3;
        IsRequireTargetPlayer = true;
    }
}
public class Card4 : CardBase
{
    public Card4()
    {
        this.Num = 4;
    }
}
public class Card5 : CardBase
{
    public Card5()
    {
        this.Num = 5;
        IsRequireTargetPlayer = true;
    }
}
public class Card6 : CardBase
{
    public Card6()
    {
        this.Num = 6;
        IsRequireTargetPlayer = true;
    }
}
public class Card7 : CardBase
{
    public Card7()
    {
        this.Num = 7;
    }
}
public class Card8 : CardBase
{
    public Card8()
    {
        this.Num = 8;
    }
}
