using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace MyGame
{
    public class ChangeSceneProcedure : MyProcedureBase
    {
        private bool m_IsChangeSceneComplete;
        private string nextSceneid;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

            GameEntry.Sound.StopAllLoadingSounds();
            GameEntry.Sound.StopAllLoadedSounds();

            // 隐藏所有实体
            GameEntry.Entity.HideAllLoadingEntities();
            GameEntry.Entity.HideAllLoadedEntities();

            // 卸载所有场景
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }

            // 还原游戏速度
            GameEntry.Base.ResetNormalGameSpeed();
            nextSceneid  = procedureOwner.GetData<VarString>("NextScene");
            if (nextSceneid == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", nextSceneid.ToString());
                return;
            }

            GameEntry.Scene.LoadScene($"Assets/GameMain/Scenes/{nextSceneid}.unity", 0, this);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

            base.OnLeave(procedureOwner, isShutdown);
        }


        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds,
            float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_IsChangeSceneComplete)
            {
                return;
            }

            if (nextSceneid== "MainScene")
            {
                
            }
            else if (nextSceneid== "MenuScene")
            {
                ChangeState<MenuProcedure>(procedureOwner);
            }
            // ChangeState<ProcedureMenu>(procedureOwner);
        }


        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);

            m_IsChangeSceneComplete = true;
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
        }

        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName,
                ne.DependencyAssetName,
                ne.LoadedCount.ToString(), ne.TotalCount.ToString());
        }
    }
}