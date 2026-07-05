SceneManager.cs 역할 및 기능 설명
전체 역할
SceneManager는 Scene 파트의 로직 허브입니다. 직접 게임 로직을 계산하지 않고, 이미 만들어진 각 파트의 매니저(Character/Time/Asset/Deck/Hand/Scheduler)를 참조해서 호출 순서를 조율하는 역할만 합니다. UI 표시는 HUDController에 위임합니다.

한 줄 요약
SceneManager = 각 매니저의 함수를 "언제, 어떤 순서로" 부를지 결정하는 지휘자. 실제 계산(스탯 증감, 자산 계산, 카드 효과 등)은 전혀 하지 않고 전부 기존 매니저에게 위임

구조별 설명
1. 참조 필드 + Awake

Character, Time, Asset, Deck, Hand, Scheduler, HUDController까지 총 7개 매니저를 필드로 갖고 있음
Awake()에서 인스펙터에 연결이 안 되어 있으면 FindObjectOfType() 또는 .Instance로 자동 탐색해서 연결 (CardEffectManager.cs가 쓰던 방식과 동일한 안전장치)
gameOverPanel은 시작 시 무조건 꺼둠

2. 게임오버 상태 관리

isGameOver 플래그 하나로 게임 진행 가능 여부를 통제
진입점 함수(StartDay, NextTurn, SelectOT, SelectRest) 맨 앞에 전부 if (isGameOver) return; 가드가 있어서, 게임오버 이후에는 어떤 액션도 먹히지 않도록 막아둠

3. 조회(Get) 함수들

전부 "다른 매니저 함수를 그대로 위임하는 한 줄짜리 래퍼"
null 체크 후 기본값(0, TimeMode.Day 등) 반환하는 방어 로직 포함
HUDController가 UI를 그릴 때 이 함수들만 호출하면 되게끔 창구 역할

4. 진입점(Action) 함수들 — 핵심 로직
StartDay()

손패 카드 뽑기(handManager.DrawCards())만 수행 후 HUD 갱신

NextTurn() (가장 복잡한 함수)

while(스케줄러에 카드 남아있는 동안) 반복
매 반복마다:

schedulerManager.UseCard() 실행 → 성공/실패(bool) 받음
실패하면 → 즉시 TriggerGameOver() 후 함수 종료 (남은 카드는 처리 안 함, 롤백도 안 함 — 지난 대화에서 합의한 대로)
성공하면 → 그 즉시 CheckWeekGoal() 호출 (시간이 리셋되기 전에 상환일 여부를 확인해야 하므로 카드 1장 처리할 때마다 바로 체크)
이어서 CheckGameOver()로 스탯 0 여부도 확인
둘 중 하나라도 게임오버를 유발했으면 즉시 종료


모든 카드가 문제없이 소진되면 마지막에 UpdateHUD()

SelectOT() / SelectRest()

각각 characterController.OTAct() / RestAct()만 호출하고 HUD 갱신
실제 스탯 계산 로직은 전혀 없음 (Character 파트가 이미 다 처리)

CheckWeekGoal()

timeManager.IsDebtDay()가 false면 그냥 리턴 (상환일이 아니면 할 일 없음)
상환일이면 보유 자산 < 목표 자산 비교해서 미달 시 게임오버

CheckGameOver()

HP 또는 MP가 0 이하인지만 확인해서 게임오버 트리거

TriggerGameOver() (private)

실제 게임오버 처리의 최종 지점: 플래그 세팅 + 패널 활성화만, 씬 전환 없음
이미 게임오버 상태면 중복 실행 방지

5. 화면 갱신 함수

UpdateHUD() / UpdateBackground()는 로직이 없고 hudController에 그대로 넘기는 위임 함수 — SceneManager는 "언제 갱신할지"만 결정하고, "어떻게 그릴지"는 HUDController 책임


StatHUD.cs
체력/정신력 Slider와 Text 표시. UpdateStatUI(hp, maxHp, mp, maxMp) 하나만 갖고 있음

AssetHUD.cs
보유 자산 / 목표 자산 텍스트 표시. UpdateAssetUI(asset, goalAsset)

TimeHUD.cs
시/일차/주차 텍스트 + 낮·야근·휴식 아이콘 전환. UpdateTimeUI(hour, day, week, mode) 내부에서 아이콘 전환은 private UpdateTimeModeIcon()으로 분리

DeckHandHUD.cs
덱/무덤/손패/스케줄러 카드 수 텍스트 표시. UpdateDeckHandUI(deckCnt, discardCnt, handCnt, schedulerCnt)

DeckHandHUD.cs
덱/무덤/손패/스케줄러 카드 수 텍스트 표시. UpdateDeckHandUI(deckCnt, discardCnt, handCnt, schedulerCnt)

BackgroundController.cs
낮/밤 배경 이미지 전환. UpdateBackground(mode) — Rest는 밤 배경 재사용(임시 처리)

HUDController.cs
위 5개를 조립하는 상위 조율자. SceneManager에게서 값을 받아 각 하위 HUD에 전달만 함. TimeManager.OnTimeChanged/AssetManager.OnAssetChanged 이벤트 구독도 여기서 처리