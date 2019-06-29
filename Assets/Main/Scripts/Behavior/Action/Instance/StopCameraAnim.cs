using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    internal class StopCameraAnim : Action
    {
        public override TaskStatus OnUpdate()
        {
            var cc = GameEntry.SceneLogic.BaseInstanceLogic.CameraController;
            if (cc == null)
            {
                Log.Warning("CameraController not found.");
                return TaskStatus.Failure;
            }

            cc.StopAnimation();
            return TaskStatus.Success;
        }
    }
}
