using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaiwuModdingLib.Core.Plugin;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Debug = UnityEngine.Debug;

namespace HelloWorld
{
    [PluginConfig("HelloWorld","听琴入梦","1.0.0")]
    public class TaiWuModTest : TaiwuRemakePlugin
    {
        Harmony harmony;
        public override void Dispose()
        {
            if (harmony!=null)
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
            var __instance_traverse=Traverse.Create(__instance);
            Refers refers = __instance.CGet<Refers>("AbilityView");
            var button=refers.CGet<CButton>("ClearAbility").gameObject;
            var getAllBtn = GameObject.Instantiate(button, button.transform.parent);
            getAllBtn.transform.localPosition = button.transform.localPosition + new Vector3(-150, 0, 0);
            getAllBtn.transform.localScale = Vector3.one;
            var btn = getAllBtn.GetComponent<CButton>();
            var text =btn.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = "全特性";
            Debug.LogWarning("生成按钮");
            btn.ClearAndAddListener(() =>
            {
                Debug.LogWarning("按下一键全特性");
            });
        }


    }
}
