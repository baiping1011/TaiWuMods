using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Localization;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace MyGame
{
    public class LaunchProcedure : MyProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            //配置语言
            InitLanguageSettings();

            // 声音配置：根据用户配置数据，设置即将使用的声音选项
            InitSoundSettings();
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds,
            float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            // 运行一帧即切换到 Splash 展示流程
            ChangeState<StartAniProcedure>(procedureOwner);
        }

        private void InitLanguageSettings()
        {
            if (GameEntry.Base.EditorResourceMode && GameEntry.Base.EditorLanguage != Language.Unspecified)
            {
                // 编辑器资源模式直接使用 Inspector 上设置的语言
                return;
            }

            Language language = GameEntry.Localization.Language;
            if (GameEntry.Setting.HasSetting(Constant.Setting.Language))
            {
                try
                {
                    string languageString = GameEntry.Setting.GetString(Constant.Setting.Language);
                    language = (Language)Enum.Parse(typeof(Language), languageString);
                }
                catch
                {
                }
            }

            if (language != Language.English
                && language != Language.ChineseSimplified
                && language != Language.ChineseTraditional)
            {
                // 若是暂不支持的语言，则使用英语
                language = Language.English;

                GameEntry.Setting.SetString(Constant.Setting.Language, language.ToString());
                GameEntry.Setting.Save();
            }

            GameEntry.Localization.Language = language;
            Log.Info("Init language settings complete, current language is '{0}'.", language.ToString());
        }

        private void InitSoundSettings()
        {
            GameEntry.Sound.Mute("Music", GameEntry.Setting.GetBool(Constant.Setting.MusicMuted, false));
            GameEntry.Sound.SetVolume("Music", GameEntry.Setting.GetFloat(Constant.Setting.MusicVolume, 0.3f));
            GameEntry.Sound.Mute("Sound", GameEntry.Setting.GetBool(Constant.Setting.SoundMuted, false));
            GameEntry.Sound.SetVolume("Sound", GameEntry.Setting.GetFloat(Constant.Setting.SoundVolume, 1f));
            GameEntry.Sound.Mute("UISound", GameEntry.Setting.GetBool(Constant.Setting.UISoundMuted, false));
            GameEntry.Sound.SetVolume("UISound", GameEntry.Setting.GetFloat(Constant.Setting.UISoundVolume, 1f));
            Log.Info("Init sound settings complete.");
        }
    }
}