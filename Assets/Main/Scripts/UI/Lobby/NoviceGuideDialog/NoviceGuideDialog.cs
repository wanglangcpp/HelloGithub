using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using System;

namespace Genesis.GameClient
{
    public class NoviceGuideDialog : NGUIForm
    {
        public enum GuideType : int
        {
            Click = 1,
            Text = 2
        }
        [SerializeField]
        private GuideToast guideToastForm;

        private const int r = 60;
        private const int R = r + 30;
        private readonly int[] ArrowDegree = new int[8] { 45, 0, -45, -90, -135, 180, 135, 90 };
        private readonly Vector2[] ArrowFrom = new Vector2[8] { new Vector2(-R / 1.42f, R / 1.42f), new Vector2(0, R), new Vector2(R / 1.42f, R / 1.42f), new Vector2(R, 0), new Vector2(R / 1.42f, -R / 1.42f), new Vector2(0, -R), new Vector2(-R / 1.42f, -R / 1.42f), new Vector2(-R, 0) };
        private readonly Vector2[] ArrowTo = new Vector2[8] { new Vector2(-r / 1.42f, r / 1.42f), new Vector2(0, r), new Vector2(r / 1.42f, r / 1.42f), new Vector2(r, 0), new Vector2(r / 1.42f, -r / 1.42f), new Vector2(0, -r), new Vector2(-r / 1.42f, -r / 1.42f), new Vector2(-r, 0) };
        [SerializeField]
        private GameObject[] circleArray;//两个圈圈
        [SerializeField]
        private GameObject circle;
        [SerializeField]
        private GameObject arrow;//箭头
        private NoviceGuideDialogData data;
        public float secondCircleDelay = 0.25f;
        [SerializeField]
        private UILabel contenxtText;//本文
        [SerializeField]
        private GameObject textBlock;
        [SerializeField]
        private GameObject textTouchArea;
        [SerializeField]
        private GameObject mask;
        private bool isTickFinished = true;
        private int guideIndex = 0;//新手引导到哪一步
        private GameObject curClickObject;
        private GuideType guideType;
        private Transform orgClickTransform;
        private List<EventDelegate> toggleList;
        private bool onceClick = false;//为避免重复点击

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            data = userData as NoviceGuideDialogData;
            mask.SetActive(data.oneGroup[0].IsShade);
            //Invoke("Guide", data.oneGroup[0].DelayTime);
            StartCoroutine(Guide(data.oneGroup[0].DelayTime));
        }

        private IEnumerator Guide(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            yield return null;
            yield return null;
            Guide();
        }

        private void Guide()
        {
            GuideType type = (GuideType)data.oneGroup[guideIndex].ShowType;
            ChangeState(type);
            switch (guideType)
            {
                case GuideType.Click:
                    PreClick(guideIndex);
                    break;
                case GuideType.Text:
                    string text = GameEntry.Localization.GetString(data.oneGroup[guideIndex].Desc);
                    SetText(text);
                    break;
            }
        }

        private void ChangeState(GuideType curType)
        {
            if (curType != guideType)
            {
                switch (curType)
                {
                    case GuideType.Click:
                        PlayCircle();
                        if (curClickObject != null)
                        {
                            curClickObject.SetActive(true);
                        }
                        arrow.SetActive(true);
                        textBlock.SetActive(false);
                        textTouchArea.gameObject.SetActive(false);
                        break;
                    case GuideType.Text:
                        StopCircle();
                        if (curClickObject != null)
                        {
                            curClickObject.SetActive(false);
                        }
                        arrow.SetActive(false);
                        textBlock.SetActive(true);
                        textTouchArea.gameObject.SetActive(true);
                        break;
                }
            }
            guideType = curType;
        }

        private void PreClick(int index)
        {
            Transform formTransform = data.mForm.transform;
            DRGuideUI item = data.oneGroup[index];
            //Log.Debug("NoviceGuideDialog.PreClick data:" + data);
            //Log.Debug("NoviceGuideDialog.PreClick data.mForm:" + data.mForm);
            //Log.Debug("NoviceGuideDialog.PreClick formTransform:" + formTransform);
            //Log.Debug("NoviceGuideDialog.PreClick Instance Chapter Bg:" + formTransform.Find("Instance Chapter Bg"));
            //Log.Debug("NoviceGuideDialog.PreClick Instance Chapter Bg/Chapter 1:" + formTransform.Find("Instance Chapter Bg/Chapter 1"));
            Log.Debug("NoviceGuideDialog.PreClick Instance Chapter Bg/Chapter 1/Chapter Item List 1:" + formTransform.Find("Instance Chapter Bg/Chapter 1/Chapter Item List 1"));
            Log.Debug("NoviceGuideDialog.PreClick Instance Chapter Bg/Chapter 1/Chapter Item List 1/Chapter 1 Level 117 IntKey 0:" + formTransform.Find("Instance Chapter Bg/Chapter 1/Chapter Item List 1/Chapter 1 Level 117 IntKey 0"));

            var tmp = formTransform.Find("Instance Chapter Bg/Chapter 1/Chapter Item List 1");
            if (tmp)
            {
                var grandFa = tmp.GetComponentsInChildren<Transform>();
                for (int i=0; i< tmp.childCount;i++)
                {
                    Log.Debug("grandFa children:" + tmp.GetChild(i).gameObject.name+"__");
                }
            }


            try
            {//复制目标对象
                orgClickTransform = formTransform.Find(item.ClickObject);
                if (curClickObject != null)
                {
                    curClickObject.SetActive(false);
                }
                curClickObject = Instantiate(orgClickTransform.gameObject);
            }
            catch (Exception e)
            {
                Log.Debug("NoviceGuideDialog.CopyObjectPath is error," + item.toString());
                Log.Debug("NoviceGuideDialog. is error," + e.StackTrace.ToString());
                CloseSelf();
            }

            //为兼容 抽奖特效复制后报错
            UIEffect compoment = curClickObject.transform.GetComponentInChildren<UIEffect>();
            if (compoment != null)
            {
                compoment.gameObject.SetActive(false);// = false;
            }
            //Test 目前表内容中会对抽奖时无法被选中

            if (item.ClickObject.StartsWith("Instance Chapter Bg/Chapter "))
            {//只有选关卡的时候隐藏
                orgClickTransform.gameObject.SetActive(false);
            }
            //orgClickTransform.gameObject.SetActive(false);
            UIButton clickBtnCompoment = curClickObject.GetComponent<UIButton>();
            if (clickBtnCompoment)
            {
                clickBtnCompoment.onClick.Clear();
                clickBtnCompoment.onClick.Add(new EventDelegate(this, "ButtonClick"));
            }
            else
            {
                UIToggle toggle = curClickObject.GetComponent<UIToggle>();
                if (toggle)
                {
                    toggleList = new List<EventDelegate>(toggle.onChange);
                    toggle.onChange.Clear();
                }
                UIEventListener listener = curClickObject.gameObject.GetComponent<UIEventListener>();
                if (!listener)
                {
                    curClickObject.gameObject.AddComponent<UIEventListener>();
                }
                curClickObject.gameObject.GetComponent<UIEventListener>().onClick += OnClickLister;
            }
            curClickObject.transform.SetParent(transform, true);
            curClickObject.transform.localScale = new Vector3(1, 1, 1);
            UIPanel curClickPanel = curClickObject.GetComponent<UIPanel>();
            if (curClickPanel)
            {
                int dialogDepth = transform.gameObject.GetComponent<UIPanel>().depth;
                curClickPanel.depth = dialogDepth + 1;
            }


            //计算目标对象中心点
            //clickObject.transform.position = form.InverseTransformPoint(orgClickTransform.position);
            //yield return null;
            Log.Debug("clickObject.pos:" + curClickObject.transform.position.ToString());
            Log.Debug("orgClickTransform.pos:" + orgClickTransform.position.ToString());
            curClickObject.transform.position = orgClickTransform.position;

            //移动圈圈到复制物体中心点
            circle.transform.parent = curClickObject.transform;
            circle.transform.localPosition = new Vector3(0, 0, 0);
            //移动箭头到中心点
            arrow.transform.parent = curClickObject.transform;
            arrow.transform.localPosition = new Vector3(0, 0, 0);
            SetArror(item.Direction);
            //yield return null;
        }

        void OnClickLister(GameObject df)
        {
            if (onceClick) return;
            onceClick = true;
            PreClickLogic();
            orgClickTransform.gameObject.SetActive(true);
            UIToggle toggle = orgClickTransform.GetComponent<UIToggle>();
            if (toggle)
            {
                toggle.value = true;
                UIToggle.current = toggle;
                UIToggle.current.value = true;
            }
            else
            {
                orgClickTransform.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
            }
            ToNextStep();
            //toggle.value = true;
            ////UIToggle.current.value = true;
            ////UIToggle toggle = orgClickTransform.GetComponent<UIToggle>();
            //////toggle.value = true;
            ////if (toggle.onChange.Count > 0)
            ////{
            ////    EventDelegate.Execute(toggleList);
            ////}

        }

        void ButtonClick()
        {
            Log.Debug("NoviceGuideDialog.ButtonClick");
            if (onceClick) return;
            onceClick = true;
            PreClickLogic();
            orgClickTransform.gameObject.SetActive(true);
            //Log.Debug("ButtonClick:parent:"+orgClickTransform.parent.name);
            UIButton btnCompoment = orgClickTransform.GetComponent<UIButton>();
            if (btnCompoment)
            {
                if (btnCompoment.onClick.Count > 0)
                {
                    EventDelegate.Execute(btnCompoment.onClick);
                }
                else
                {
                    UIButton.current = null;
                    UICamera.Notify(orgClickTransform.gameObject, "OnClick", null);
                    //orgClickTransform.SendMessage("OnClick");
                }
            }
            //UICamera.Notify(orgClickObject.gameObject, "OnClick", null);
            //string ss =orgClickTransform.GetComponent<UIButton>();
            ToNextStep();
            //data.mForm.SendMessage("OnClickInstanceButton",SendMessageOptions.DontRequireReceiver);
        }

        private void SetArror(int arrowIndex)
        {
            arrow.transform.rotation = Quaternion.identity;
            arrow.transform.Rotate(0, 0, ArrowDegree[arrowIndex]);
            TweenPosition arrowTweenPos = arrow.GetComponent<TweenPosition>();
            arrowTweenPos.ResetToBeginning();
            arrowTweenPos.from = ArrowFrom[arrowIndex];
            arrowTweenPos.to = ArrowTo[arrowIndex];
        }

        private void PlayCircle()
        {
            CancelInvoke("PlayCircle");
            circleArray[0].SetActive(true);
            Invoke("CircleTwo", secondCircleDelay);
        }

        private void CircleTwo()
        {
            CancelInvoke("CircleTwo");
            circleArray[1].SetActive(true);
        }

        private void StopCircle()
        {
            circleArray[0].SetActive(false);
            circleArray[1].SetActive(false);
        }

        private void SetText(string text)
        {
            isTickFinished = false;
            contenxtText.text = text;
            contenxtText.GetComponent<TypewriterEffect>().ResetToBeginning();
        }

        public void OnTextTickFinished()
        {
            isTickFinished = true;
        }

        //屏蔽遮罩区点击
        public void OnClickOthers()
        {
            Log.Debug("OnClickOthers");
            switch (guideType)
            {
                case GuideType.Click:
                    ShowToast();
                    break;
                case GuideType.Text:
                    if (isTickFinished)
                    {
                        PreClickLogic();
                        ToNextStep();
                    }
                    else
                    {
                        isTickFinished = true;
                        contenxtText.GetComponent<TypewriterEffect>().Finish();
                    }
                    break;
            }
        }

        private void PreClickLogic()
        {
            Log.Debug("PreClickLogic:curID:" + data.oneGroup[guideIndex].Id);
            GameEntry.NoviceGuide.CheckKeyStep(data.oneGroup[guideIndex]);
            GameEntry.NoviceGuide.SetNextGuideID(data.oneGroup[guideIndex].Id);
            CheckIsEnd();
        }

        private void ToNextStep()
        {
            onceClick = false;
            if (guideIndex + 1 < data.oneGroup.Count)
            {
                guideIndex++;
                Log.Debug("ToNextStep:guideIndex=" + guideIndex);
                Guide();
            }
            else
            {
                if (data.lastGuideId == data.oneGroup[guideIndex].Id)
                {
                    GameEntry.NoviceGuide.SetNextGroupID();
                }
                CloseSelf();
            }
        }

        private void CheckIsEnd()
        {
            if (guideIndex + 1 >= data.oneGroup.Count)
            {
                GameEntry.NoviceGuide.isShow = false;
            }
        }

        private void ShowToast()
        {
            guideToastForm.gameObject.SetActive(true);
            guideToastForm.Fire();
        }
    }
}