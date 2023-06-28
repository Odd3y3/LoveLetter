using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ī�忡 ���� ������ ��� �ִ� Ŭ����
/// </summary>
public static class CardsInfo
{
    private const int MAX_CARD_COUNT = 8;

    private static CardBase[] cards = new CardBase[MAX_CARD_COUNT + 1]
    {
        null, new Card1(), new Card2(), new Card3(),
        new Card4(), new Card5(), new Card6(),
        new Card7(), new Card8()
    };

    //public CardsInfo()
    //{
    //    cards = new CardBase[MAX_CARD_COUNT + 1];
    //    cards[1] = new Card1();
    //    cards[2] = new Card2();
    //    cards[3] = new Card3();
    //    cards[4] = new Card4();
    //    cards[5] = new Card5();
    //    cards[6] = new Card6();
    //    cards[7] = new Card7();
    //    cards[8] = new Card8();
    //}

    /// <summary>
    /// ī�� ���� �������� �Լ�
    /// </summary>
    /// <returns>index�� �ش��ϴ� ��ȣ�� ī�� ����(�ν��Ͻ�)�� ������</returns>
    public static CardBase GetCardInfo(int index)
    {
        if (index > 0 && index <= MAX_CARD_COUNT)
            return cards[index];
        else
            throw new IndexOutOfRangeException("index out of MAX_CARD_COUNT.");
    }

}
