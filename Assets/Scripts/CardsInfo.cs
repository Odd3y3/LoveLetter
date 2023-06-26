using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카드에 대한 정보를 담고 있는 클래스
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
    /// 카드 정보 가져오는 함수
    /// </summary>
    /// <returns>index에 해당하는 번호의 카드 정보(인스턴스)를 가져옴</returns>
    public CardBase GetCardInfo(int index)
    {
        return cards[index];
    }

}
