using UnityEngine;

/// <summary>
/// 캐릭터 기본 데이터와 초기 설정값을 관리한다.
/// HP, MP의 최대값과 현재값을 저장한다.
/// 외부 StatManager.GetHP(), StatManager.GetMP()와 연결될 수 있도록
/// 현재 HP, MP 값을 보관하는 데이터 역할만 담당한다.
/// </summary>
public class CharacterData : MonoBehaviour
{
    [Header("HP Data")]
    public int maxHP = 100;
    public int currentHP = 100;

    [Header("MP Data")]
    public int maxMP = 100;
    public int currentMP = 100;
}