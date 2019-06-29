using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public struct PoolObject
    {
        public GameObject mPoolObject;
        public int mIndex;

        public void SetValue(GameObject tempObject, int index)
        {
            mPoolObject = tempObject;
            mIndex = index;
        }
    }
    public class ResourcePool
    {
        Dictionary<string, List<int>> m_DiretyMap = new Dictionary<string, List<int>>();
        Dictionary<string, List<GameObject>> m_ResourceMap = new Dictionary<string, List<GameObject>>();
        Dictionary<string, UnityEngine.Object> m_SourceMap = new Dictionary<string, UnityEngine.Object>();

//         private static ResourcePool __instance = null;
//         public static ResourcePool Instance
//         {
//             get
//             {
//                 if (null == __instance)
//                     __instance = new ResourcePool();
//                 return __instance;
//             }
//         }
//         public ResourcePool()
//         {
//             if (null != __instance)
//                 Debug.Log("GameApplication has already exist !");
// 
//             __instance = this;
//         }
        public PoolObject GetPoolObject(string resourcePath, GameObject resourcObj)
        {
            PoolObject temp = new PoolObject();

            //1 查找dirty中的值
            if (!m_SourceMap.ContainsKey(resourcePath))
            {
                if (!CreatMetaResource(resourcePath, resourcObj))
                {
                    Debug.Log("resource load is error at first:" + resourcePath);
                    return temp;
                }
            }

            if (!m_ResourceMap.ContainsKey(resourcePath) || !m_DiretyMap.ContainsKey(resourcePath))
            {
                Debug.Log("metaResource is miss or Resource list is error:" + resourcePath);
                return temp;
            }


            //如果实例化池中的资源都在被使用，则增加新的实例
            if (m_DiretyMap[resourcePath].Count <= 0)
            {
                temp = InsertNewResource(resourcePath);
            }
            else
            {
                temp = GetGameObjectByDirty(resourcePath);
            }

            return temp;
        }

        public void DisablePoolObject(string resourcePath, int indexID)
        {
            AddDirtyTag(resourcePath, indexID);
        }

        private PoolObject GetGameObjectByDirty(string resourcePath)
        {
            PoolObject tempObj = new PoolObject();
            List<int> dirtyList = m_DiretyMap[resourcePath];
            int index = dirtyList[0];
            GameObject resourceObj = m_ResourceMap[resourcePath][index];
            if (resourceObj != null)
            {
                dirtyList.Remove(index);
                resourceObj.SetActive(true);

                tempObj.SetValue(resourceObj, index);
            }
            return tempObj;
        }

        private PoolObject InsertNewResource(string resourcePath)
        {
            PoolObject tempObject = new PoolObject();


            GameObject newObj = GameObject.Instantiate(m_SourceMap[resourcePath], Vector3.zero, Quaternion.identity) as GameObject;
            if (newObj == null)
            {
                Debug.Log("creat new object fail:" + resourcePath);
            }
            else
            {
                List<GameObject> resourceList = m_ResourceMap[resourcePath];
                resourceList.Add(newObj);
                int index = resourceList.Count - 1;
                tempObject.SetValue(newObj, index);
            }
            return tempObject;
        }

        private void AddDirtyTag(string resourcePath, int index)
        {
            if (!m_ResourceMap.ContainsKey(resourcePath))
                return;
            GameObject tempObj = m_ResourceMap[resourcePath][index];
            if (tempObj != null)
            {
                tempObj.SetActive(false);
                m_DiretyMap[resourcePath].Add(index);
            }
        }

        private bool CreatMetaResource(string resourcePath, GameObject resourceObj)
        {
            //UnityEngine.Object newMetaObj = UnityEngine.Resources.Load(resourcePath);
            if (resourceObj != null)
            {
                ////为新的key值开辟空间,元资源进行赋值
                m_SourceMap.Add(resourcePath, resourceObj);
                m_ResourceMap.Add(resourcePath, new List<GameObject>());
                m_DiretyMap.Add(resourcePath, new List<int>());
                return true;
            }
            return false;
        }
        public void Clear(string resourcePath)
        {
            //m_SourceMap.Clear();
            if (!m_ResourceMap.ContainsKey(resourcePath))
                return;

            for (int i = 0; i < m_ResourceMap[resourcePath].Count; i++)
            {
                GameObject.Destroy(m_ResourceMap[resourcePath][i]);
            }
            m_ResourceMap[resourcePath].Clear();
            m_DiretyMap[resourcePath].Clear();
        }
        public void Clear()
        {
            m_SourceMap.Clear();
            m_ResourceMap.Clear();
            m_DiretyMap.Clear();
        }

        public void Destroy()
        {
            m_SourceMap = null;
            m_ResourceMap = null;
            m_DiretyMap = null;
  //          __instance = null;
        }
    }
}
