using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public class WelfareCenterForm : NGUIForm
    {
        [SerializeField]
        private Temlate TypeTableTemlate = null;//列表项模板
        [SerializeField]
        private GameObject m_WelfareTable = null;//福利列表
        [SerializeField]
        private GameObject m_WelfareList = null;//福利窗口
        [SerializeField]
        private List<UIToggle> m_Tabs = null;//福利列表集合

        private string CurrentPath = null;
        private int CurrentFormId = 0;

        private Dictionary<int, GameObject> m_CachedChargeForms = new Dictionary<int, GameObject>();

        private DRWelfareCenter[] m_AllFunctionInWelfare = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_AllFunctionInWelfare = GameEntry.DataTable.GetDataTable<DRWelfareCenter>().GetAllDataRows();
            m_WelfareTable = transform.Find("Welfare List Bg/Welfare Type Border/Welfare Type Table").gameObject;
            m_WelfareList = transform.Find("Welfare List Bg/Welfare List Border").gameObject;
            TypeTableTemlate.Self.SetActive(false);
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            ResetWelfareTypeList();
            SubscribeEvents();
            var displayData = userData as WelfareCenterDisplayData;
            if (displayData == null)
            {
                InitTabs(WelfareType.EveryDayGift);
                return;
            }
            InitTabs(displayData.Scenario);
        }
        //LoadAndInstantiateUIInstanceSuccessEventArgs

        /// <summary>
        /// 重置福利列表
        /// </summary>
        private void ResetWelfareTypeList()
        {
            List<DROpenFunction> OpenFunction = GameEntry.OpenFunction.ShowSubTabFunction((int)OpenFunctionComponent.Function.WelfareCenter);
            for (int i = 0; i < m_AllFunctionInWelfare.Length; i++)
            {
                var OpenWelfareType = OpenFunction.Find(x => x.Id == m_AllFunctionInWelfare[i].Id);
                if (OpenWelfareType == null) continue;

                TypeTableTemlate.IntKey.Key = m_AllFunctionInWelfare[i].Id;
                for (int count = 0; count < TypeTableTemlate.BtnText.Length; count++)
                {
                    TypeTableTemlate.BtnText[count].text = GameEntry.Localization.GetString(m_AllFunctionInWelfare[i].Type);
                }
                GameObject go = NGUITools.AddChild(m_WelfareTable, TypeTableTemlate.Self);
                go.name = string.Format("Welfare Center {0}", (i + 1).ToString());
                go.SetActive(true);
                m_Tabs.Add(go.GetComponent<UIToggle>());
            }
            m_WelfareTable.GetComponent<UITable>().enabled = true;
        }

        private void SubscribeEvents()
        {
            GameEntry.Event.Subscribe(EventId.LoadAndInstantiateUIInstanceSuccess, LoadAndInstantiateUIInstanceSuccess);
            GameEntry.Event.Subscribe(EventId.LoadAndInstantiateUIInstanceFailure, LoadAndInstantiateUIInstanceFailure);
        }
        private void UnsubscribeEvents()
        {
            GameEntry.Event.Unsubscribe(EventId.LoadAndInstantiateUIInstanceSuccess, LoadAndInstantiateUIInstanceSuccess);
            GameEntry.Event.Unsubscribe(EventId.LoadAndInstantiateUIInstanceFailure, LoadAndInstantiateUIInstanceFailure);
        }

        private void LoadAndInstantiateUIInstanceSuccess(object sender, GameEventArgs e)
        {
            LoadAndInstantiateUIInstanceSuccessEventArgs args = e as LoadAndInstantiateUIInstanceSuccessEventArgs;

            GameObject objform = args.GameObject;
            objform.transform.SetParent(m_WelfareList.transform);
            objform.transform.localPosition = Vector3.zero;
            objform.transform.localScale = Vector3.one;
            m_CachedChargeForms.Add(CurrentFormId, objform);
            int prentDepth = this.GetComponent<UIPanel>().depth;
            UIPanel[] panels = objform.GetComponentsInChildren<UIPanel>(true);
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].depth += prentDepth;
            }
            objform.SetActive(true);
        }
        private void LoadAndInstantiateUIInstanceFailure(object sender, GameEventArgs e)
        {
            LoadAndInstantiateUIInstanceFailureEventArgs args = e as LoadAndInstantiateUIInstanceFailureEventArgs;
            CurrentFormId = 0;
            UIUtility.ShowToast(GameEntry.Localization.GetString("UI_UNOPENED_FUNCTION"));
        }
        public void OnTab(int id, bool ischange)
        {
            if (!ischange)
            {
                return;
            }
            var CurrentData = GameEntry.DataTable.GetDataTable<DRWelfareCenter>().GetDataRow(id);
            CurrentPath = CurrentData.FormPath;

            //判断窗口是否已经存在
            if (!m_CachedChargeForms.ContainsKey(id))
            {
                if (CurrentFormId != 0)
                {
                    m_CachedChargeForms[CurrentFormId].gameObject.SetActive(false);
                }
                GameEntry.UIFragment.LoadAndInstantiate(CurrentPath, id);
                CurrentFormId = id;
                return;
            }
            if (CurrentFormId != 0)
            {
                m_CachedChargeForms[CurrentFormId].SetActive(false);
            }
            m_CachedChargeForms[id].SetActive(true);
            CurrentFormId = id;
        }

        private void InitTabs(WelfareType scenario)
        {
            for (int i = 0; i < m_Tabs.Count; i++)
            {
                int key = m_Tabs[i].gameObject.GetComponent<UIIntKey>().Key;
                if ((int)scenario == key)
                {
                    m_Tabs[i].Set(true);
                    return;
                }
            }
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            UnsubscribeEvents();
        }

        [Serializable]
        private class Temlate
        {
            public GameObject Self = null;
            public UIToggle Toggle = null;
            public UIIntKey IntKey = null;
            public UISprite RemindIcon = null;
            public UILabel[] BtnText = null;
        }
    }

}

