using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;
using UnityEngine.UI;

/*
namespace FreeUpdatePatcher
{
    [BepInPlugin("user.aerin.plugin.FreeUpdatePatcher", "FreeUpdatePatcher", "0.0.1")]
    public class FreeUpdatePatcherClass : BaseUnityPlugin
    {
        void Start()
        {
            Harmony.CreateAndPatchAll(typeof(FreeUpdatePatcherClass));
            Debug.Log("Hello World!");
        }
        [HarmonyPostfix, HarmonyPatch(typeof(Menu_Dev_Update), "BUTTON_Start")]
        public static void Menu_Dev_Update_BUTTON_Start_PostPatch(taskUpdate __instance, Menu_Dev_Update __instance2) 
        {
            var objects_Menu_Dev_Update = GameObject.FindObjectOfType<Menu_Dev_Update>();
            //var points_GameScript = Traverse.Create(objects_Menu_Dev_Update).Field("gS_").Method("GetGesamtDevPoints").GetValue<int>();
            //var points_GameScript = Traverse.Create(objects_Menu_Dev_Update).Field("gS_").Method("GetGesamtDevPoints").GetValue();
            __instance.pointsGameplay = Mathf.RoundToInt(0f);
            __instance.quality *= 100f;
            __instance.points += (float)Mathf.RoundToInt((float)num2 * 0.02f);
            //__instance.points *= (float)Mathf.RoundToInt((float)points_GameScript * 100f);
            Debug.Log("Hey! Patcher activated!");
            Debug.Log("points_GameScript : " + num2);
            Debug.Log("__instance.quality : " + __instance.quality);
            Debug.Log("__instance.pointsGameplay : " + __instance.pointsGameplay);
        }
    }
}
*/
namespace FreeUpdateRebalancer
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInProcess("Mad Games Tycoon 2.exe")]
    public class MainPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "aerin.Mad_Games_Tycoon_2.plugins.FreeUpdateRebalancer";
        public const string PluginName = "FreeUpdateRebalancer";
        public const string PluginVersion = "1.2.0.0";

        //https://www.ipentec.com/document/csharp-auto-implemented-property get, set???????????????????????????
        // ****** Setting ******
        // Free Update???????????????????????????
        public static ConfigEntry<float> devCostMultiplyValue { get; private set; }
        // Free Update???????????????????????????
        public static ConfigEntry<float> devAddPointMultiplyValue { get; private set; }
        // Free Update?????????????????????
        public static ConfigEntry<float> devTimeMultiplyValue { get; private set; }
        // Free Update?????????Hype??????
        public static ConfigEntry<float> devAddHypeAdditionValue { get; private set; }
        // Free Update?????????Hype??????
        public static ConfigEntry<float> devAddFunAdditionValue { get; private set; }
        // Free Update?????????Hype??????
        public static ConfigEntry<float> devBonusSellsMultiplyValue { get; private set; }
        public static ConfigEntry<float> devAddIP_PointsMultiplyValue { get; private set; }
        public static ConfigEntry<float> dev_ResetBoredFansMultiplyValue { get; private set; }


        public void Load_FreeUpdateRebalancer()
        {
            string text1 = "1. Basic settings";
            string text2 = "2. Additional Settings";
            string text3 = "3. Advanced Settings";

            devCostMultiplyValue = Config.Bind<float>(text1, "Multiply Value : Costs", 1.0f, "The value to multiply the cost of Free Update. The higher this value, the higher cost you require to development. Default : 1");
            devAddPointMultiplyValue = Config.Bind<float>(text1, "Multiply Value : Add Points", 1.0f, "The value to multiply the add extra points of Free Update. The higher this value, the more points you get. Default : 1");
            devTimeMultiplyValue = Config.Bind<float>(text1, "Multiply Value : Development Time", 1.0f, "The value to multiply the development time of Free Update. The lower this value, the slower to development. Default : 1");
            devAddHypeAdditionValue = Config.Bind<float>(text2, "Extra Points : Add Hype", 0f, "This is not feature in original game. The value to addition the Hype of Free Update. The higher this value, the more hypes you get. Default : 0");
            devAddFunAdditionValue = Config.Bind<float>(text2, "Multiply Value : Add Fans", 0f, "This is not feature in original game. The value to addition the fans of Free Update. The higher this value, the more fans you get. But It's not actual number you type add fans to your company. Default : 0");
            devBonusSellsMultiplyValue = Config.Bind<float>(text1, "Multiply Value : Bonus Sells", 1f, "The value to addition the bonus sells of Free Update. The higher this value, the more bonus sells on Market. Default : 0");
            devAddIP_PointsMultiplyValue = Config.Bind<float>(text3, "Extra Points : IP Points", 0f, "###be careful with game balance### This is not feature in original game. The value to addition the IP points of Free Update. The higher this value, the more IP Points your game's IP get. **200 IP Points per 1 IP** Default : 0");
            dev_ResetBoredFansMultiplyValue = Config.Bind<float>(text3, "n% Chance : Reset Bored Fans", 0f, "This is not feature in original game. If you got ****in' Bored Fans, Free Update cares this debuff at n% chance. **Bored Fans come back to debuff again every monthly.**   Default : 0");

            Config.SettingChanged += delegate (object sender, SettingChangedEventArgs args)
            {
            };
        }

        void Awake()
        {
            Load_FreeUpdateRebalancer();
            Harmony.CreateAndPatchAll(typeof(FreeUpdateRebalancer));
        }

        void Update()
        {

        }


    }

}
