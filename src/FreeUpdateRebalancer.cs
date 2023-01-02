
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
using System.Runtime.CompilerServices;
using System.Reflection;

namespace FreeUpdateRebalancer
{

	public class FreeUpdateRebalancer
	{

		/// <summary>
		/// 押したボタンの数を記録しておく。
		/// </summary>
		public static int _buttonsAmount { get; private set; }
		/// <summary>
		/// 記録しておく。
		/// </summary>
		public static gameScript _tmpUpdateGameScript { get; private set; }
		public static int _amountUpdateAmount { get; private set; }
		public static int _roomID { get; private set; }

		public void Rebalancer_AddHype(gameScript gameScript)
		{
			if (MainPlugin.devAddHypeAdditionValue.Value != 0f)
			{
				gameScript.AddHype(MainPlugin.devAddHypeAdditionValue.Value);
			}
		}

		public void Rebalancer_AddFans(gameScript gameScript)
        {
			if (MainPlugin.devAddFunAdditionValue.Value != 0f)
			{
				if (gameScript.mS_.genres_.GetAmountFans() == 0)
				{
					gameScript.mS_.genres_.genres_FANS[gameScript.maingenre] += 1;

				}
				int genre_ = gameScript.maingenre;
				if (gameScript.subgenre != -1 && UnityEngine.Random.Range(0, 100) > 70)
				{
					genre_ = gameScript.subgenre;
				}
				int sells = gameScript.sellsPerWeek[0];
				float sellsEffect = sells * 0.001f;
				try
				{
					gameScript.mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, sellsEffect * MainPlugin.devAddFunAdditionValue.Value)), genre_);
				}
				catch { }
			}
		}
		public void Rebalancer_AddBonusSells(taskUpdate taskUpdateObject)
        {
			taskUpdateObject.gS_.bonusSellsUpdates += (taskUpdateObject.quality * MainPlugin.devBonusSellsMultiplyValue.Value) / (float)taskUpdateObject.gS_.amountUpdates;
			if ((double)taskUpdateObject.gS_.bonusSellsUpdates > MainPlugin.devBonusSellsMultiplyValue.Value)
			{
				taskUpdateObject.gS_.bonusSellsUpdates = MainPlugin.devBonusSellsMultiplyValue.Value;
			}
		}


		public void Rebalarancer_EachButton()
		{
			gameScript gS = _tmpUpdateGameScript;
			GameObject gameObject = GameObject.Find("GAME_" + gS.myID.ToString());
			gameScript gameScript = gameObject.GetComponent<gameScript>();
			Rebalancer_AddHype(gameScript);
			Rebalancer_AddFans(gameScript);
		}
		public void taskUpdate_Complete_Injecter()
		{
			int roomID = _roomID;
			FreeUpdateRebalancer freeUpdateRebalancer = new FreeUpdateRebalancer();
			GameObject gameObject = GameObject.Find("Room_" + roomID.ToString());
			roomScript roomScript = gameObject.GetComponent<roomScript>();
			GameObject taskGameObject = roomScript.taskGameObject;
            if (taskGameObject)
            {
				taskUpdate taskUpdateObject = taskGameObject.GetComponent<taskUpdate>();
				if (taskUpdateObject)
                {
					//オリジナルのFree Updateで増やした分を減らす。
					_amountUpdateAmount = taskUpdateObject.gS_.amountUpdates;
					taskUpdateObject.gS_.bonusSellsUpdates -= taskUpdateObject.quality / (float)taskUpdateObject.gS_.amountUpdates;

					//予め保存しておいたボタンの数で増やしていく。
					for (int j = 0; j < _buttonsAmount; j++)
					{
						//Hype, Fansなどの設定
						Rebalarancer_EachButton();

						//ポイントなどの設定
						Rebalancer_AddBonusSells(taskUpdateObject);

					}
					//init member
					_buttonsAmount = 0;
					_amountUpdateAmount = 0;
					_roomID = 0;
					_tmpUpdateGameScript = null;

				}
            }
		}

		[HarmonyPostfix, HarmonyPatch(typeof(taskUpdate), "Complete")]
		static void taskUpdate_Complete_PostPatch(taskUpdate __instance, gameScript ___gS_)
        {
			int roomID = Traverse.Create(__instance).Method("FindMyRoomWithTask").GetValue<int>();
			_roomID = roomID;
			Traverse.Create(__instance).Method("FindMyObject");
			_tmpUpdateGameScript = ___gS_;
			FreeUpdateRebalancer FreeUpdateRebalancer = new FreeUpdateRebalancer();
			FreeUpdateRebalancer.taskUpdate_Complete_Injecter();

		}


		[HarmonyPrefix, HarmonyPatch(typeof(Menu_Dev_Update), "GetP_Gameplay")]
		static bool GetP_GameplayPatch(Menu_Dev_Update __instance, gameScript ___gS_, ref int __result)
		{
			//Free Update of points more ehnanced.
			//increase the point 0.02% to 0.2%
			bool[] buttonAdds = Traverse.Create(__instance).Field("buttonAdds").GetValue<bool[]>();

			float num = 0f;
			if (buttonAdds[0])
			{
				num += 1f;
				//num += ___gS_.points_gameplay * 0.02f;
				num += ___gS_.points_gameplay * 0.02f * MainPlugin.devAddPointMultiplyValue.Value;
			}
			if (buttonAdds[1])
			{
				num += 1f;
				//num += ___gS_.points_gameplay * 0.02f;
				num += ___gS_.points_gameplay * 0.02f * MainPlugin.devAddPointMultiplyValue.Value;
			}
			__result = Mathf.RoundToInt(num);
			return false;
		}

		[HarmonyPrefix, HarmonyPatch(typeof(Menu_Dev_Update), "GetP_Grafik")]
		static bool GetP_GrafikPatch(Menu_Dev_Update __instance, gameScript ___gS_, ref int __result)
		{
			//Free Update of points more ehnanced.
			//increase the point 0.02% to 0.2%
			bool[] buttonAdds = Traverse.Create(__instance).Field("buttonAdds").GetValue<bool[]>();

			float num = 0f;
			if (buttonAdds[2])
			{
				num += 1f;
				//num += ___gS_.points_gameplay * 0.02f;
				num += ___gS_.points_grafik * 0.02f * MainPlugin.devAddPointMultiplyValue.Value;
			}
			if (buttonAdds[3])
			{
				num += 1f;
				//num += ___gS_.points_gameplay * 0.02f;
				num += ___gS_.points_grafik * 0.02f * MainPlugin.devAddPointMultiplyValue.Value;
			}
			__result = Mathf.RoundToInt(num);
			return false;
		}

		[HarmonyPrefix, HarmonyPatch(typeof(Menu_Dev_Update), "GetP_Sound")]
		static bool GetP_SoundPatch(Menu_Dev_Update __instance, gameScript ___gS_, ref int __result)
		{
			//Free Update of points more ehnanced.
			//increase the point 0.02% to 0.2%
			bool[] buttonAdds = Traverse.Create(__instance).Field("buttonAdds").GetValue<bool[]>();

			float num = 0f;
			if (buttonAdds[4])
			{
				num += 1f;
				//num += ___gS_.points_gameplay * 0.02f;
				num += ___gS_.points_sound * 0.02f * MainPlugin.devAddPointMultiplyValue.Value;
			}
			if (buttonAdds[5])
			{
				num += 1f;
				//num += ___gS_.points_gameplay * 0.02f;
				num += ___gS_.points_sound * 0.02f * MainPlugin.devAddPointMultiplyValue.Value;
			}
			__result = Mathf.RoundToInt(num);
			return false;
		}

		[HarmonyPrefix, HarmonyPatch(typeof(Menu_Dev_Update), "GetP_Technik")]
		static bool GetP_TechnikPatch(Menu_Dev_Update __instance, gameScript ___gS_, ref int __result)
		{
			//Free Update of points more ehnanced.
			//increase the point 0.02% to 0.2%
			bool[] buttonAdds = Traverse.Create(__instance).Field("buttonAdds").GetValue<bool[]>();

			float num = 0f;
			if (buttonAdds[6])
			{
				num += 1f;
				//num += ___gS_.points_gameplay * 0.02f;
				num += ___gS_.points_technik * 0.02f * MainPlugin.devAddPointMultiplyValue.Value;
			}
			if (buttonAdds[7])
			{
				num += 1f;
				//num += ___gS_.points_gameplay * 0.02f;
				num += ___gS_.points_technik * 0.02f * MainPlugin.devAddPointMultiplyValue.Value;
			}
			__result = Mathf.RoundToInt(num);
			return false;
		}

		// Known bug of 32 bit-games with MonoMod(BepInEx), but still not release official patch yet.
		// So fixed with user mod of https://github.com/BepInEx/BepInEx/issues/458 (MMTestFix.zip)

		[HarmonyPrefix, HarmonyPatch(typeof(Menu_Dev_Update), "GetDevCosts")]
		static bool GetDevCostsPatch(Menu_Dev_Update __instance, gameScript ___gS_, mainScript ___mS_, ref long __result)
		{

			long GetCosts_Bugs = Traverse.Create(__instance).Method("GetCosts_Bugs").GetValue<long>();
			bool[] buttonAdds = Traverse.Create(__instance).Field("buttonAdds").GetValue<bool[]>();
			bool[] sprachen = Traverse.Create(__instance).Field("sprachen").GetValue<bool[]>();

			long num = GetCosts_Bugs;
			long num2 = ___gS_.costs_entwicklung + ___gS_.costs_updates;
			if (num2 > 50000000L)
			{
				num2 = 50000000L;
			}
			for (int i = 0; i < buttonAdds.Length; i++)
			{
				if (buttonAdds[i])
				{
					num += (long)Mathf.RoundToInt((float)num2 * __instance.devCostsPercent[i] * MainPlugin.devCostMultiplyValue.Value);
				}
			}
			for (int j = 0; j < sprachen.Length; j++)
			{
				if (sprachen[j] && !___mS_.Muttersprache(j))
				{
					num += (long)(___gS_.GetGesamtDevPoints() * 5);
				}
			}

			Debug.Log(num);
			__result = num;
			return false;
		}


		[HarmonyPostfix, HarmonyPatch(typeof(Menu_Dev_Update), "BUTTON_Start")]
		static void BUTTON_StartPatch(Menu_Dev_Update __instance, gameScript ___gS_, mainScript ___mS_, GUI_Main ___guiMain_, roomScript ___rS_)
		{
			//init _buttonsAmount;
			_buttonsAmount = 0;
			bool[] buttonAdds = Traverse.Create(__instance).Field("buttonAdds").GetValue<bool[]>();
			for (int j = 0; j < buttonAdds.Length; j++)
			{
				if (buttonAdds[j])
				{
					_buttonsAmount++;
				}
			}

		}

		[HarmonyPostfix, HarmonyPatch(typeof(Menu_Dev_Update), "Init")]
		static void InitPatch(Menu_Dev_Update __instance, gameScript ___gS_, mainScript ___mS_)
		{
			long num = ___gS_.costs_entwicklung + ___gS_.costs_updates;

			var m = Traverse.Create("Menu_Dev_Update");
			__instance.uiObjects[9].GetComponent<Text>().text = ___mS_.GetMoney((long)Mathf.RoundToInt((float)num * __instance.devCostsPercent[0] * MainPlugin.devCostMultiplyValue.Value), true);
			__instance.uiObjects[10].GetComponent<Text>().text = ___mS_.GetMoney((long)Mathf.RoundToInt((float)num * __instance.devCostsPercent[1] * MainPlugin.devCostMultiplyValue.Value), true);
			__instance.uiObjects[11].GetComponent<Text>().text = ___mS_.GetMoney((long)Mathf.RoundToInt((float)num * __instance.devCostsPercent[2] * MainPlugin.devCostMultiplyValue.Value), true);
			__instance.uiObjects[12].GetComponent<Text>().text = ___mS_.GetMoney((long)Mathf.RoundToInt((float)num * __instance.devCostsPercent[3] * MainPlugin.devCostMultiplyValue.Value), true);
			__instance.uiObjects[13].GetComponent<Text>().text = ___mS_.GetMoney((long)Mathf.RoundToInt((float)num * __instance.devCostsPercent[4] * MainPlugin.devCostMultiplyValue.Value), true);
			__instance.uiObjects[14].GetComponent<Text>().text = ___mS_.GetMoney((long)Mathf.RoundToInt((float)num * __instance.devCostsPercent[5] * MainPlugin.devCostMultiplyValue.Value), true);
			__instance.uiObjects[15].GetComponent<Text>().text = ___mS_.GetMoney((long)Mathf.RoundToInt((float)num * __instance.devCostsPercent[6] * MainPlugin.devCostMultiplyValue.Value), true);
			__instance.uiObjects[16].GetComponent<Text>().text = ___mS_.GetMoney((long)Mathf.RoundToInt((float)num * __instance.devCostsPercent[7] * MainPlugin.devCostMultiplyValue.Value), true);
            
			float[] TextProzent = new float[8];
            string[] Button_Mn = new string[8];
			for (int i = 0; i < 8; i++)
            {
				TextProzent[i] = 2f * MainPlugin.devAddPointMultiplyValue.Value;
                Button_Mn[i] = "Button_M" + (i + 1) + "/TextProzent";
				GameObject.Find(Button_Mn[i]).GetComponent<Text>().text = String.Join(string.Empty, "+", TextProzent[i].ToString(), "%");
			}

			m.Method("UpdateGUI");
		}

		[HarmonyPostfix, HarmonyPatch(typeof(Menu_Dev_Update), "UpdateGUI")]
		static void UpdateGUIPatch(Menu_Dev_Update __instance, gameScript ___gS_, mainScript ___mS_)
        {
			/*
			Debug.Log(__instance.uiObjects[9].GetComponent<Text>().text);
			//CanvasInGameMenu / Menu_Dev_Update / WindowMain / PanelButtons / Button_M1 /
			//Findで親と一緒に検索をかけるには、GameObject.Find("Button_M1/TextProzent").GetComponent<hoge>のようにすると通る。
			var hoge = GameObject.Find("Button_M1/TextProzent").GetComponent<Text>();
			var fuga = hoge.text;
			Debug.Log(fuga);
			*/

		}

		// Total Game Development Speed with Character
		[HarmonyPostfix, HarmonyPatch(typeof(characterScript), "GetWorkSpeed")]
		static void GetWorkSpeed_PostPatch(characterScript __instance, mainScript ___mS_, ref float __result, roomScript ___roomS_)
		{
			float num = __result;
			//**** Added ****
			try
			{
				taskUpdate taskUpdate = ___roomS_.GetTaskUpdate();
				if (taskUpdate.myID == ___roomS_.taskID)
				{
					num *= MainPlugin.devTimeMultiplyValue.Value;
				}
			}
			catch { }
			//**** Added ended ****
			__result = num;
		}
	}
}
