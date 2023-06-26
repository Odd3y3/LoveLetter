using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ī�忡 ���� ������ ��� �ִ� Ŭ����
/// </summary>
public class CardsInfo
{
    private CardBase[] cards;

    public CardsInfo()
    {
        cards = new CardBase[9];
        cards[1] = new Card1();
        cards[2] = new Card2();
        cards[3] = new Card3();
        cards[4] = new Card4();
        cards[5] = new Card5();
        cards[6] = new Card6();
        cards[7] = new Card7();
        cards[8] = new Card8();
    }

    /// <summary>
    /// ī�� ���� �������� �Լ�
    /// </summary>
    /// <returns>index�� �ش��ϴ� ��ȣ�� ī�� ����(�ν��Ͻ�)�� ������</returns>
    public CardBase GetCardInfo(int index)
    {
        return cards[index];
    }

}
