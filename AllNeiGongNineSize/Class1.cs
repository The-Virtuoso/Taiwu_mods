using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Harmony12;
using UnityModManagerNet;
using UnityEngine;

namespace AllNeiGongNineSize//全内功9格
{
    //设置
    public class Settings:UnityModManager.ModSettings
    {
        //添加一个选项 如果不想破坏体验 可选只对神一品内功生效
        public int neiGongLevel;

        public bool bestNeiGongActive;
        //save setting

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this,modEntry);
        }
    }

    public class Main
    {
        //umm log
        public static UnityModManager.ModEntry.ModLogger logger;

        //mod settings
        public static Settings settings;

        //mod enabled
        public static bool enabled;

        //mod load
        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Main.logger = modEntry.Logger;//new a duixiang
            Main.settings = Settings.Load<Settings>(modEntry);//load user setting
            HarmonyInstance.Create(modEntry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());// info

            modEntry.OnToggle = Main.OnToggle;
            modEntry.OnGUI = Main.OnGUI;
            modEntry.OnSaveGUI = Main.OnSaveGUI;

            return true;
        }

        //mod jihuo
        public static bool OnToggle(UnityModManager.ModEntry modEntry,bool value)
        {
                Main.enabled = value;
                return true;
        }

        //mod set
        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUIStyle redLabelStyle = new GUIStyle();
            redLabelStyle.normal.textColor = new Color(159f / 20f, 20f / 256f, 29f / 256f);
            GUILayout.Label("第一次做mod不知道写点什么,感谢大家的使用,如果遇到BUG请跟帖反馈！",redLabelStyle);
            Main.settings.neiGongLevel = GUILayout.SelectionGrid(Main.settings.neiGongLevel, new string[] { "全内功9格", "一品内功9格" }, 2);

        }

        // mod set save
        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Main.settings.Save(modEntry);
        }
    }
    //load mod while user gointo savefiles
    [HarmonyPatch(typeof(Loading), "LoadGameDateStart2")]
    public static class SetNeiGongNineSize_For_IDontKnow
    {
        //chushihua all neigong level
        static List<string> neiGongLevel = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        //front chuli
        static void Prepare()
        {
            if (Main.settings.neiGongLevel != 1)
            {
                if (!Main.settings.bestNeiGongActive)
                {
                    neiGongLevel.Remove("1");
                    neiGongLevel.Remove("2");
                    neiGongLevel.Remove("3");
                    neiGongLevel.Remove("4");
                    neiGongLevel.Remove("5");
                    neiGongLevel.Remove("6");
                    neiGongLevel.Remove("7");
                    neiGongLevel.Remove("8");
                }
            }
        }

        //neigong nine size
        static void Postfix()
        {
            if (!Main.enabled) return;

            foreach (Dictionary<int,string> item in DateFile.instance.gongFaDate.Values)
            {
                if (neiGongLevel.Contains(item[2]) && item[62] == "101")
                {
                    item[921] = "9";
                    item[922] = "9";
                    item[923] = "9";
                    item[924] = "9";
                }
            }
        }
    }
}
