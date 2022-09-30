using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace MyGame
{
    public class StartAniProcedure : MyProcedureBase
    {
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds,
            float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            // TODO: 这里可以播放一个 Splash 动画
            // ...

            if (GameEntry.Base.EditorResourceMode)
            {
                // 编辑器模式
                Log.Info("Editor resource mode detected.");
                ChangeState<PreLoadProcedure>(procedureOwner);
            }
            else if (GameEntry.Resource.ResourceMode == ResourceMode.Package)
            {
                // 单机模式
                Log.Info("Package resource mode detected.");
                ChangeState<ProcedureInitResources>(procedureOwner);
            }
        }
    }
}