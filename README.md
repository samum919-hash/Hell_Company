# Hell Company

픽셀 아트 기반의 회사 경영 카드 게임

##  게임 소개

지옥의 회사에서 악마 사장으로 살아남으며  
자산, 체력, 정신력을 관리하는 카드 기반 경영 게임입니다.

카드를 사용하여 자산을 확보하고,  
체력과 정신력을 관리하며 주어진 목표를 달성해야 합니다.

##  개발 환경

- Unity
- C#
- Git / GitHub

##  주요 시스템

- 카드 기반 게임 플레이
- 자산 관리
- 체력 / 정신력 관리
- 주차별 목표 시스템
- 상점 시스템

##  프로젝트 구조

```
Assets/
├── Images/        # 게임 이미지
├── Scenes/        # UI 및 패널
└── Scripts/
    ├── Card/
    │   ├── Card.cs                # 카드 UI 표시, 클릭 시 HandManager에 선택 전달
    │   ├── CardData.cs            # 카드 기본 데이터(이름/종류/HP·MP·시간 소모/자산 획득)
    │   ├── CardEffectManager.cs   # 카드 사용 시 캐릭터 행동·시간 진행·자산 획득 처리
    │   ├── DeckManager.cs         # 덱 생성/셔플/뽑기, 무덤 관리
    │   ├── HandManager.cs         # 손패 카드 뽑기·선택·스케줄러 이동
    │   └── SchedulerManager.cs    # 스케줄러에 올라간 카드 사용·제거
    │
    ├── Character/
    │   ├── CharacterController.cs # 캐릭터 파트(Data/Stat/Motion) 통합 제어
    │   ├── CharacterData.cs       # HP/MP 최대값·현재값 데이터
    │   ├── CharacterMotion.cs     # 상태별(Idle/Tired/Stress/Exhaust) 이미지 전환
    │   └── CharacterStat.cs       # HP/MP 증감 계산, 저체력·저정신력 판정
    │
    ├── Scene/
    │   ├── SceneManager.cs        # 전체 로직 허브: 턴 진행, 야근/휴식, 상환일·게임오버 판정
    │   ├── HUDController.cs       # 하위 HUD 일괄 갱신, Time/Asset 이벤트 구독
    │   ├── StatHUD.cs             # 체력/정신력 슬라이더·텍스트 표시
    │   ├── AssetHUD.cs            # 보유/목표 자산 텍스트 표시
    │   ├── TimeHUD.cs             # 시간/일차/주차 텍스트, 낮·야근·휴식 아이콘 표시
    │   ├── DeckHandHUD.cs         # 덱/무덤/손패/스케줄러 카드 수 표시
    │   └── BackgroundController.cs# 시간대별 배경 전환 (SceneManager 경유)
    │
    └── TimeAsset/
        ├── TimeManager.cs         # 시간/일차/주차 진행, 시간대 판정, 상환일 체크
        ├── AssetManager.cs        # 자산 증감, 주차별 목표 자산, 야근 수당
        ├── AssetPanel.cs          # 자산 텍스트 표시 (OnAssetChanged 이벤트 구독)
        ├── BackgroundManager.cs   # 시간대별 배경 전환 (OnTimeChanged 이벤트 구독)
        ├── Enums.cs                # TimeMode(Day/Overtime/Rest) 정의
        └── TimePannelMove.cs      # 낮↔야근 전환 시 안내 패널 슬라이드 애니메이션
