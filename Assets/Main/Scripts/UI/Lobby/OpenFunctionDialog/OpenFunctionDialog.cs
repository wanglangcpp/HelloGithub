using GameFramework;
using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 功能解锁动画
    /// </summary>
    public class OpenFunctionDialog : NGUIForm
    {
        //具体效果参数在编辑器调整
        public AnimationCurve cure;
        private OpenFunctionsData lobbyData;
        [SerializeField]
        private UISprite block_effect;
        [SerializeField]
        private UILabel desc;
        private int currentIndex = 0;
        [SerializeField]
        private float DelayTime = 2;
        [SerializeField]
        private UISprite defualtSprite;
        [SerializeField]
        public UILabel itemName;
        private Transform destinationItem;
        //DROpenFunction data;
        private GameObject moveObject;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            lobbyData = userData as OpenFunctionsData;
            StartCoroutine(Play(0));
        }

        private IEnumerator Play(int index)
        {
            DROpenFunction data = lobbyData.itemList[index];
            //重排
            destinationItem = lobbyData.lobby.transform.Find(data.FunctionPath);
            if (!destinationItem) Log.Error("Not Find:" + data.FunctionPath);
            destinationItem.gameObject.SetActive(true);
            ResortGroupFunction(data);
            yield return null;
            yield return null;
            destinationItem.gameObject.SetActive(data.IsSubpage());

            if (moveObject != null) {
                Destroy(moveObject);
            }

            //UI初始化
            block_effect.gameObject.SetActive(true);
            desc.gameObject.SetActive(true);
            desc.text = GameEntry.Localization.GetString(data.Desc);
            //移动的功能
            if (data.IsSubpage())
            {
                defualtSprite.LoadAsync(data.IconId);
                moveObject = defualtSprite.gameObject;
                itemName.text = GameEntry.Localization.GetString(data.Name);
            }
            else {
                Transform beCopyTransform = lobbyData.lobby.transform.Find(data.FunctionPath);
                moveObject = Instantiate(beCopyTransform.gameObject);
                UIButton beCopyCompomentUIButton = moveObject.GetComponent<UIButton>();
                if (beCopyCompomentUIButton)
                {
                    beCopyCompomentUIButton.enabled = false;
                }
                else {
                    beCopyCompomentUIButton = moveObject.GetComponentInChildren<UIButton>();
                    if (beCopyCompomentUIButton) {
                        beCopyCompomentUIButton.enabled = false;
                    }
                }
            }
            
            moveObject.SetActive(true);
            moveObject.transform.SetParent(gameObject.transform);
            moveObject.transform.localScale = new Vector3(1f, 1f, 1f);
            moveObject.transform.localPosition = new Vector3(0f, 0f, 0f);

            Invoke("SetVisible", DelayTime);
            TweenPosition moveTween = moveObject.GetComponent<TweenPosition>();
            if (!moveTween)
            {
                moveTween = moveObject.AddComponent<TweenPosition>();
                moveTween.delay = DelayTime;
                moveTween.method = UITweener.Method.EaseIn;//UITweener.Method.EaseIn
                moveTween.animationCurve = cure;
                moveTween.SetOnFinished(new EventDelegate(this, "OnTweenPositionFinished"));
            }
            moveTween.ResetToBeginning();
            moveTween.from = moveObject.transform.position;
            moveTween.to = lobbyData.lobby.transform.InverseTransformPoint(destinationItem.position);
            moveTween.PlayForward();
            yield return 0;
        }

        private void SetVisible()
        {
            block_effect.gameObject.SetActive(false);
            desc.gameObject.SetActive(false);
        }

        private void OnTweenPositionFinished()
        {
            currentIndex++;
            desc.gameObject.SetActive(false);
            destinationItem.gameObject.SetActive(true);
            if (currentIndex < lobbyData.itemList.Count)
            {
                StartCoroutine(Play(currentIndex));
            }
            else
            {
                lobbyData.itemList.Clear();
                GameEntry.OpenFunction.isPlayingAnimation = false;
                CloseSelf();
            }
        }

        /// <summary>
        /// 重排，要协程中调用
        /// </summary>
        /// <param name="item"></param>
        public void ResortGroupFunction(DROpenFunction item)
        {
            string functionPath = item.FunctionPath;
            Transform grid = lobbyData.lobby.transform.Find(functionPath.Substring(0, functionPath.LastIndexOf("/")));
            //for (int i = 0; i < grid.childCount; i++) Log.Debug("grid " + grid.GetChild(i) + ":" + grid.GetChild(i).gameObject.activeSelf + ":" + grid.GetChild(i).position.ToVector2().ToString());
            //Log.Debug("RefreshGroupItem:" + grid.name);
            UIGrid gridComponent = grid.GetComponent<UIGrid>();
            gridComponent.enabled = true;
            gridComponent.sorting = UIGrid.Sorting.Custom;
            gridComponent.onCustomSort = CustomSort;
            gridComponent.repositionNow = true;
            gridComponent.Reposition();
        }

        private int CustomSort(Transform orgTransform, Transform compareTransform)
        {
            int pos1 = orgTransform.GetComponent<OpenFunctionData>().data.GroupIndex;
            int pos2 = compareTransform.GetComponent<OpenFunctionData>().data.GroupIndex;
            if (orgTransform.GetComponent<OpenFunctionData>().data.FunctionGroup == 1)
            {
                return pos1 - pos2;
            }
            else return pos2 - pos1;
        }

        public override void OnClickBackButton()
        {
            //屏蔽遮罩区点击 Done
        }

        protected override void OnPreClose()
        {
            base.OnPreClose();
            GameEntry.Event.Fire(EventId.OpenFunctionAnimationEnd, new OpenFunctionAnimationEndEventArgs(lobbyData.lobby));

        }
    }
}
