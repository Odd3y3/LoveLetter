using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionNumSelectUI : MonoBehaviour
{
    private int targetDir = 0;

    private InGameManager inGameManager;

    //어떤 플레이어를 선택했는지 알기위해서 TargetPlayerUI에서 정보를 가져옴
    private TargetPlayerUI targetPlayerUI;

    private void Awake()
    {
        //InGameManager 초기화
        inGameManager = FindFirstObjectByType<InGameManager>();
        if (inGameManager == null)
            Debug.LogWarning("inGameManager is null.");


        //targetPlayerUI 초기화
        targetPlayerUI = FindFirstObjectByType<TargetPlayerUI>();
        if (targetPlayerUI == null)
            Debug.LogWarning("targetPlayerUI is null.");
    }


    /// <summary>
    /// 어떤 TargetPlayer를 지정했는지 set해주는 함수
    /// </summary>
    /// <param name="dir">열거형 Dir을 int로 변환한 값</param>
    public void SetTargetInfo(int dir)
    {
        targetDir = dir;
    }


    /// <summary>
    /// 버튼을 눌렀을 때 호출 되는 함수
    /// </summary>
    public void OnClickSelectOption(int cardNum)
    {
        inGameManager.EndTurn(1, targetDir, cardNum);
        transform.parent.gameObject.SetActive(false);
    }
}
