using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CreatePlayerForm
    {
        private class SecondPageController
        {
            private CreatePlayerForm m_Form = null;
            private IFsm<SecondPageController> m_Fsm = null;
            private int m_RotationSign = -1;

            internal bool IsEnabled { get; set; }

            internal SecondPageController()
            {

            }

            internal void Init(CreatePlayerForm form)
            {
                m_Form = form;
                IsEnabled = true;
                m_Fsm = GameEntry.Fsm.CreateFsm(this, new StateInit(), new StateNormal(), new StateRotating());
                m_Fsm.Start<StateInit>();
            }

            internal void Shutdown()
            {
                IsEnabled = false;

                if (m_Fsm != null)
                {
                    GameEntry.Fsm.DestroyFsm(m_Fsm);
                    m_Fsm = null;
                }

                if (m_Form != null)
                {
                    m_Form.ClearAllCharacters();
                    m_Form = null;
                }
            }

            internal void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
                if (!IsEnabled)
                {
                    Log.Error("You cannot update SecondPageController when it's not enabled.");
                    return;
                }
            }

            internal void OnClickBackBtn()
            {
                if (m_Fsm == null || !(m_Fsm.CurrentState is StateNormal))
                {
                    return;
                }

                m_Form.ShowFirstPage();
            }

            internal void OnCreatePlayerClick()
            {
                if (m_Fsm == null || !(m_Fsm.CurrentState is StateNormal))
                {
                    return;
                }

                ProcedureCreatePlayer procedureCreatePlayer = GameEntry.Procedure.CurrentProcedure as ProcedureCreatePlayer;

                if (procedureCreatePlayer == null)

                {
                    Log.Warning("Can not create player in '{0}' procedure.", GameEntry.Procedure.CurrentProcedure.GetType().Name);
                    return;
                }

                procedureCreatePlayer.CreatePlayer(m_Form.m_PlayerName.value, m_Form.m_SelectedHeroType);
            }

            private abstract class StateBase : FsmState<SecondPageController>
            {
                protected IFsm<SecondPageController> m_CachedFsm = null;

                protected CreatePlayerForm Form
                {
                    get
                    {
                        return m_CachedFsm.Owner.m_Form;
                    }
                }

                protected float HeroPlatformAngleInterval
                {
                    get
                    {
                        return 360f / Form.m_CandidateHeroTypes.Length;
                    }
                }

                protected override void OnInit(IFsm<SecondPageController> fsm)
                {
                    base.OnInit(fsm);
                    m_CachedFsm = fsm;
                }

                protected override void OnEnter(IFsm<SecondPageController> fsm)
                {
                    base.OnEnter(fsm);
                }

                protected override void OnLeave(IFsm<SecondPageController> fsm, bool isShutdown)
                {
                    if (isShutdown)
                    {
                        m_CachedFsm = null;
                    }

                    base.OnLeave(fsm, isShutdown);
                }

                protected void RefreshHeroInfo()
                {
                    DRHero heroDataRow = GameEntry.DataTable.GetDataTable<DRHero>().GetDataRow(Form.m_SelectedHeroType);
                    if (heroDataRow == null)
                    {
                        Log.Warning("Cannot find hero type '{0}'.", Form.m_SelectedHeroType);
                        return;
                    }

                    Form.m_HeroNameText.text = GameEntry.Localization.GetString(heroDataRow.Name);
                    Form.m_HeroDescText.text = GameEntry.StringReplacement.GetString(GameEntry.Localization.GetString(heroDataRow.Description));
                }
            }

            private class StateInit : StateBase
            {
                private int m_HeroWaitCount = 0;

                protected override void OnEnter(IFsm<SecondPageController> fsm)
                {
                    base.OnEnter(fsm);
                    m_HeroWaitCount = 0;
                    Form.m_HeroDescText.text = string.Empty;
                    Form.m_HeroNameText.text = string.Empty;
                    GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
                    ShowHeroes();
                }

                protected override void OnLeave(IFsm<SecondPageController> fsm, bool isShutdown)
                {
                    GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);

                    if (!isShutdown && Form.m_SelectedHeroType <= 0)
                    {
                        Form.m_SelectedHeroType = Form.m_CandidateHeroTypes[0];
                    }

                    base.OnLeave(fsm, isShutdown);
                }

                private void ShowHeroes()
                {
                    for (int i = 0; i < Form.m_CandidateHeroTypes.Length; ++i)
                    {
                        Form.m_Characters.Add(null);
                    }

                    float angleInterval = HeroPlatformAngleInterval;
                    for (int i = 0; i < Form.m_CandidateHeroTypes.Length; ++i)
                    {
                        BaseLobbyHeroData heroData = new UnpossessedLobbyHeroData(Form.m_CandidateHeroTypes[i]);
                        var currentAngle = angleInterval * i;
                        var radian = Mathf.Deg2Rad * (currentAngle + Form.m_DefaultPlatformRotation);
                        var position = -new Vector2(Form.m_HeroPosRadiusToPlatform * Mathf.Sin(radian), Form.m_HeroPosRadiusToPlatform * Mathf.Cos(radian));
                        FakeCharacter.Show(heroData.CharacterId, heroData.Type, Form.m_PlatformRoot, position, currentAngle);
                        GameEntry.Waiting.StartWaiting(WaitingType.Default, GetType().Name);
                        ++m_HeroWaitCount;
                    }
                }

                private void OnShowEntitySuccess(object o, GameEventArgs e)
                {
                    var ne = e as UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;
                    if (ne == null)
                    {
                        return;
                    }

                    if (!(ne.Entity.Logic is FakeCharacter))
                    {
                        return;
                    }

                    var newFakeCharacter = ne.Entity.Logic as FakeCharacter;

                    if (newFakeCharacter.CachedTransform.parent != Form.m_PlatformRoot)
                    {
                        return;
                    }

                    for (int i = 0; i < Form.m_Characters.Count; ++i)
                    {
                        if (newFakeCharacter.Data.HeroId == Form.m_CandidateHeroTypes[i])
                        {
                            Form.m_Characters[i] = newFakeCharacter;
                            break;
                        }
                    }

                    GameEntry.Waiting.StopWaiting(WaitingType.Default, GetType().Name);
                    --m_HeroWaitCount;

                    if (m_HeroWaitCount <= 0)
                    {
                        ChangeState<StateNormal>(m_CachedFsm);
                    }
                }
            }

            private class StateNormal : StateBase
            {
                private int m_SelectedHeroEffectEntityId = 0;

                protected override void OnEnter(IFsm<SecondPageController> fsm)
                {
                    base.OnEnter(fsm);
                    RefreshHeroInfo();
                    ShowSelectedHeroEffect();
                    GameEntry.Event.Subscribe(EventId.DragStart, OnDragStart);
                    GameEntry.Event.Subscribe(EventId.SwipeStart, OnSwipeStart);
                }

                protected override void OnLeave(IFsm<SecondPageController> fsm, bool isShutdown)
                {
                    if (GameEntry.IsAvailable)
                    {
                        GameEntry.Event.Unsubscribe(EventId.DragStart, OnDragStart);
                        GameEntry.Event.Unsubscribe(EventId.SwipeStart, OnSwipeStart);
                    }

                    HideSelectedHeroEffect();
                    base.OnLeave(fsm, isShutdown);
                }

                private void OnSwipeStart(object sender, GameEventArgs e)
                {
                    var ne = e as SwipeStartEventArgs;
                    OnDragOrSwipeStart(ne.DeltaPosition);
                }

                private void OnDragStart(object sender, GameEventArgs e)
                {
                    var ne = e as DragStartEventArgs;
                    OnDragOrSwipeStart(ne.DeltaPosition);
                }

                private void OnDragOrSwipeStart(Vector2 deltaPosition)
                {
                    if (Mathf.Abs(deltaPosition.x) < 1f)
                    {
                        return;
                    }

                    m_CachedFsm.Owner.m_RotationSign = deltaPosition.x > 0f ? 1 : -1;
                    ChangeState<StateRotating>(m_CachedFsm);
                }

                private void ShowSelectedHeroEffect()
                {
                    m_SelectedHeroEffectEntityId = GameEntry.Entity.GetSerialId();
                    int index = Form.SelectedHeroIndex;
                    GameEntry.Entity.ShowEffect(new EffectData(m_SelectedHeroEffectEntityId, string.Empty, Form.m_SelectedHeroEffectResPath, Form.m_Characters[index].Id));
                    //Form.m_Characters[index].Debut();
                }

                private void HideSelectedHeroEffect()
                {
                    if (m_SelectedHeroEffectEntityId == 0)
                    {
                        return;
                    }

                    if (GameEntry.Entity.HasEntity(m_SelectedHeroEffectEntityId) || GameEntry.Entity.IsLoadingEntity(m_SelectedHeroEffectEntityId))
                    {
                        GameEntry.Entity.HideEntity(m_SelectedHeroEffectEntityId);
                    }

                    m_SelectedHeroEffectEntityId = -1;
                }
            }

            private class StateRotating : StateBase
            {
                private float m_CurrentAngle;
                private float m_TargetAngle;

                private Quaternion TargetRotation
                {
                    get
                    {
                        return Quaternion.Euler(0f, m_TargetAngle, 0f);
                    }
                }

                protected override void OnEnter(IFsm<SecondPageController> fsm)
                {
                    base.OnEnter(fsm);

                    int index = Form.SelectedHeroIndex;
                    m_CurrentAngle = Form.m_DefaultPlatformRotation - index * HeroPlatformAngleInterval;

                    if (fsm.Owner.m_RotationSign > 0)
                    {
                        Form.m_SelectedHeroType = Form.m_CandidateHeroTypes[(index + 1) % Form.m_CandidateHeroTypes.Length];
                        m_TargetAngle = m_CurrentAngle - HeroPlatformAngleInterval;
                    }
                    else
                    {
                        Form.m_SelectedHeroType = Form.m_CandidateHeroTypes[(index + Form.m_CandidateHeroTypes.Length - 1) % Form.m_CandidateHeroTypes.Length];
                        m_TargetAngle = m_CurrentAngle + HeroPlatformAngleInterval;
                    }
                }

                protected override void OnUpdate(IFsm<SecondPageController> fsm, float elapseSeconds, float realElapseSeconds)
                {
                    float amountToRotate = Form.m_HeroPlatformAngularSpeed * elapseSeconds;

                    if (Quaternion.Angle(Form.m_PlatformRoot.localRotation, TargetRotation) < amountToRotate)
                    {
                        Form.m_PlatformRoot.localRotation = TargetRotation;
                        ChangeState<StateNormal>(fsm);
                        return;
                    }

                    Form.m_PlatformRoot.Rotate(0f, -amountToRotate * fsm.Owner.m_RotationSign, 0f, Space.Self);
                }

                protected override void OnLeave(IFsm<SecondPageController> fsm, bool isShutdown)
                {
                    RefreshHeroInfo();
                    base.OnLeave(fsm, isShutdown);
                }
            }
        }
    }
}
