using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionNumSelectUI : MonoBehaviour
{
    private int targetDir = 0;

    private InGameManager inGameManager;

    //� �÷��̾ �����ߴ��� �˱����ؼ� TargetPlayerUI���� ������ ������
    private TargetPlayerUI targetPlayerUI;

    private void Awake()
    {
        //InGameManager �ʱ�ȭ
        inGameManager = FindFirstObjectByType<InGameManager>();
        if (inGameManager == null)
            Debug.LogWarning("inGameManager is null.");


        //targetPlayerUI �ʱ�ȭ
        targetPlayerUI = FindFirstObjectByType<TargetPlayerUI>();
        if (targetPlayerUI == null)
            Debug.LogWarning("targetPlayerUI is null.");
    }


    /// <summary>
    /// � TargetPlayer�� �����ߴ��� set���ִ� �Լ�
    /// </summary>
    /// <param name="dir">������ Dir�� int�� ��ȯ�� ��</param>
    public void SetTargetInfo(int dir)
    {
        targetDir = dir;
    }


    /// <summary>
    /// ��ư�� ������ �� ȣ�� �Ǵ� �Լ�
    /// </summary>
    public void OnClickSelectOption(int cardNum)
    {
        inGameManager.EndTurn(1, targetDir, cardNum);
        transform.parent.gameObject.SetActive(false);
    }
}
