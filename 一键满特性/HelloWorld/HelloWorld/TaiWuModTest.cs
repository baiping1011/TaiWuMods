using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaiwuModdingLib.Core.Plugin;
using TaiwuModdingLib.Core.Utils;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Debug = UnityEngine.Debug;
using Config;
using CharacterDataMonitor;
using GameData.GameDataBridge;

namespace HelloWorld
{
    [PluginConfig("HelloWorld", "听琴入梦", "1.0.0")]
    public class TaiWuModTest : TaiwuRemakePlugin
    {
        Harmony harmony;
        public override void Dispose()
        {
            if (harmony != null)
            {
                harmony.UnpatchSelf();
            }
        }

        public override void Initialize()
        {
            harmony = Harmony.CreateAndPatchAll(typeof(TaiWuModTest));
            Debug.Log("TaiWuModTest  loaded!");
        }
        public override void OnModSettingUpdate()
        {
            base.OnModSettingUpdate();
            Debug.Log("TaiWuModTest  OnModSettingUpdate!");
        }
        public override void OnEnterNewWorld()
        {
            base.OnEnterNewWorld();
            Debug.Log("TaiWuModTest  OnEnterNewWorld!");
        }
        public override void OnLoadedArchiveData()
        {
            base.OnLoadedArchiveData();
            Debug.Log("TaiWuModTest  OnLoadedArchiveData!");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UI_NewGame), "AbilityViewAwake")]
        public static void UI_NewGame_AbilityViewAwake_Postfix(UI_NewGame __instance)
        {

            var __instance_traverse = Traverse.Create(__instance);
            Refers refers = __instance.CGet<Refers>("AbilityView");
            var button = refers.CGet<CButton>("ClearAbility").gameObject;
            var getAllBtn = GameObject.Instantiate(button, button.transform.parent);
            getAllBtn.transform.localPosition = button.transform.localPosition + new Vector3(-150, 0, 0);
            getAllBtn.transform.localScale = Vector3.one;
            var btn = getAllBtn.GetComponent<CButton>();
            var text = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = "全特性";
            Debug.LogWarning("生成按钮");
            btn.ClearAndAddListener(() =>
            {

                var _selectedAbilities = __instance_traverse.Field("_selectedAbilities").GetValue<List<ProtagonistFeatureItem>>();
                _selectedAbilities.Clear();
                for (int i = 0; i < 30; i++)
                {
                    _selectedAbilities.Add(ProtagonistFeature.Instance[i]);
                }
                __instance_traverse.Field("_selectedAbilities").SetValue(_selectedAbilities);
                //__instance_traverse.Method("UpdatePoints");
                //刷新点数
                __instance.CallMethod("UpdatePoints");

                var _selectedPoints = __instance_traverse.Field("_selectedPoints").GetValue<int[]>();
                _selectedPoints[3] = 10;
                __instance_traverse.Field("_selectedPoints").SetValue(_selectedPoints);
                //刷新UI
                refers.CGet<InfinityScroll>("FamilyAbilityView").UpdateData(0);
                refers.CGet<InfinityScroll>("WealthAbilityView").UpdateData(0);
                refers.CGet<InfinityScroll>("SkillAbilityView").UpdateData(0);
                refers.CGet<InfinityScroll>("ChooseAbilityView").UpdateData(_selectedAbilities.Count);

                for (ushort i = 0; i < 10000; i++)
                {
                    var ss = LocalStringManager.Get(i);
                    Debug.Log($"游戏内string：index:{i} value:{ss}");

                }
                for (ushort i = 168; i < 338; i++)
                {
                    var ss = CharacterFeature.Instance[i];
                    Debug.Log($"游戏内特性：index:{i} Name:{ss.Name}");

                }


                Debug.LogWarning("按下一键全特性");
            });
        }

        [HarmonyPostfix, HarmonyPatch(typeof(FeatureMonitor), "OnNotifyData")]
        public static void FeatureMonitor_OnNotifyData_Postfix(FeatureMonitor __instance, NotificationWrapper wrapper)
        {
            var __instance_traverse = Traverse.Create(__instance);

            var FeatureIds = __instance_traverse.Field("FeatureIds").GetValue<List<short>>();

            foreach (var id in FeatureIds)
            {
                //CharacterId
                SingletonObject.getInstance<CharacterMonitorModel>().GetMonitorItem<FeatureMonitor>(taiwuid, 5);
            }
            //var taiwuid = SingletonObject.getInstance<BasicGameData>().TaiwuCharId;

            //var feature = SingletonObject.getInstance<CharacterMonitorModel>().GetMonitorItem<FeatureMonitor>(taiwuid, 5);
            //if (feature != null)
            //{
            //    feature.
            //    }
            //base.CharacterId
            //    feature.
        }
    }
}
