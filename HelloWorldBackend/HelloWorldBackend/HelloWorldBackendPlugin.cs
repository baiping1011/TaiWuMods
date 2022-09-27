
using GameData.Utilities;
using HarmonyLib;
using System;
using System.Data.Common;
using TaiwuModdingLib.Core.Plugin;
using TaiwuModdingLib.Core.Utils;
using GameData.Domains.Item;
using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using System.Collections.Generic;
using Config;
using Character = GameData.Domains.Character.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.Organization;

namespace HelloWorldBackend
{
    [PluginConfig("HelloWorld","听琴入梦","1.0.0")]
    public class HelloWorldBackendPlugin : TaiwuRemakePlugin
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
            harmony = Harmony.CreateAndPatchAll(typeof(HelloWorldBackendPlugin));
            AdaptableLog.Info("后端mod启动！");
        }

        //[HarmonyPrefix, HarmonyPatch(typeof(Character), "GetFeatureIds")]
        //public static void Character_GetFeatureIds_Prefix(Character __instance)
        //{
        //   var taiwu = DomainManager.Taiwu.GetTaiwu();

        //   var character_traverse = Traverse.Create(__instance);
        //   var CharacterFeature_traverse = Traverse.Create(CharacterFeature.Instance);
        //    if (__instance.GetId()==taiwu.GetId())
        //    {
        //        var _featureIds = character_traverse.Field("_featureIds").GetValue<List<short>>();
        //        var _dataArray = CharacterFeature_traverse.Field("_dataArray").GetValue<List<CharacterFeatureItem>>();

        //        _featureIds.Clear();
        //        foreach (var item in _dataArray)
        //        {
        //            AdaptableLog.Info($"特性:{item.Name}");
        //            _featureIds.Add(item.TemplateId);
        //        }

        //        //field.AddRange(_normalPositiveBasicFeaturesPool);
        //        //field.AddRange(_normalNegativeBasicFeaturesPool);
        //        //field.AddRange(_protagonistPositiveBasicFeaturesPool);
        //        //field.AddRange(_protagonistNegativeBasicFeaturesPool);

        //        AdaptableLog.Info("添加所有的特性！");

        //    }
        //}

        //[HarmonyPostfix, HarmonyPatch(typeof(Character), "OfflineCreateProtagonistRandomFeatures")]
        //public static void Character_OfflineCreateProtagonistRandomFeatures_Postfix(Character __instance)
        //{

        //    Dictionary<short, short> featureGroup2Id = new Dictionary<short, short>();
        //    for (short i = 1; i <=16; i++)
        //    {
        //        AdaptableLog.Info($"添加特性:{ CharacterFeature.Instance[i].Name}");

        //        AddFeature(featureGroup2Id, i);
        //    }
        //    Traverse.Create(__instance).Method("OfflineApplyFeatureIds", new object[] { featureGroup2Id, -1 });
        //    AdaptableLog.Info("添加特性1-16");

        //}

        //private static void AddFeature(Dictionary<short, short> featureGroup2Id, short featureId)
        //{
        //    short groupId = CharacterFeature.Instance[featureId].MutexGroupId;
        //    bool flag = !featureGroup2Id.ContainsKey(groupId);
        //    if (flag)
        //    {
        //        featureGroup2Id.Add(groupId, featureId);
        //    }
        //}
        [HarmonyPostfix, HarmonyPatch(typeof(CharacterDomain), "CreateProtagonist")]
        public static void CharacterDomain_CreateProtagonist_Postfix(DataContext context, ProtagonistCreationInfo info)
        {
            AdaptableLog.Info($"name{info.GivenName}");
            AdaptableLog.Info($"name{info.Surname}");
           
        }
        /// <summary>
        /// 修改主属性 膂力 等
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPostfix, HarmonyPatch(typeof(Character), "OfflineCreateProtagonistMainAttributes")]
        public  static unsafe void Character_OfflineCreateProtagonistMainAttributes_Postfix(Character __instance)
        {
            var characterDomain=Traverse.Create(__instance);
            //AdaptableLog.Info($"orgMemberId： {orgMemberId}");
            MainAttributes _baseMainAttributes = characterDomain.Field("_baseMainAttributes").GetValue<MainAttributes>();
                
            for (sbyte b = 0; b < 6; b = (sbyte)(b + 1))
            {

                _baseMainAttributes.Items[b] = 255;
            }
            characterDomain.Field("_baseMainAttributes").SetValue(_baseMainAttributes);
            AdaptableLog.Info($"修改主属性");
        }
        
        ///// <summary>
        ///// 基础内力 正常 4个15% 一个主要属性25%
        ///// </summary>
        ///// <param name="__instance"></param>
        //[HarmonyPostfix, HarmonyPatch(typeof(Character), "OfflineInitializeBaseNeiliProportionOfFiveElements")]
        //public static unsafe void Character_OfflineInitializeBaseNeiliProportionOfFiveElements_Postfix(Character __instance)
        //{
        //    var charactern = Traverse.Create(__instance);
        //    //AdaptableLog.Info($"orgMemberId： {orgMemberId}");
        //    NeiliProportionOfFiveElements _baseNeiliProportionOfFiveElements = charactern.Field("_baseNeiliProportionOfFiveElements")
        //        .GetValue<NeiliProportionOfFiveElements>();
        //    for (int i = 0; i < 5; i++)
        //    {
        //        _baseNeiliProportionOfFiveElements.Items[i] =30;
        //    }
        //    charactern.Field("_baseNeiliProportionOfFiveElements").SetValue(_baseNeiliProportionOfFiveElements);
        //    AdaptableLog.Info($"修改内力为 全 100%");
        //}
    }
}
