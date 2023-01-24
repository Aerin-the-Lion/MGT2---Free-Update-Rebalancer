
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

	public class FreeUpdateRebalancer : MonoBehaviour
	{

		/// <summary>
		/// 押したボタンの数を記録しておく。
		/// </summary>
		public static int _buttonsAmount { get; private set; }
		/// <summary>
		/// 記録しておく。
		/// </summary>
		public static gameScript _tmpUpdateGameScript { get; private set; }
		public static GUI_Main _tmpUpdateGuiMain { get; private set; }
		public static mainScript _tmpUpdatemainScript { get; private set; }
		public static int _amountUpdateAmount { get; private set; }
		public static int _roomID { get; private set; }
		public static bool enabledBoredFans { get; private set; }

		public static GameObject inputLoopNumber;
		public static int countLoopNumber = 0;
		public static bool isLoopStart = false;

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

		public void Rebalancer_AddIP(gameScript gameScript)
		{
			if (MainPlugin.devAddIP_PointsMultiplyValue.Value != 0f)
			{
				gameScript.AddIpPoints(MainPlugin.devAddIP_PointsMultiplyValue.Value);
			}
		}
		public void Rebalancer_MinusYear(mainScript main_)
		{
			if (MainPlugin.devAddIP_PointsMultiplyValue.Value != 0f)
			{
				main_.year--;
			}
		}

		public void Rebalancer_ResetBoredFans(GUI_Main guiMain, mainScript mainScript)
        {
			if (MainPlugin.dev_ResetBoredFansMultiplyValue.Value != 0f)
			{
				//GameObject gameObject = guiMain.GetComponent<GameObject>();
				//Traverse.Create(guiMain).Method("UpdateLangweiligIcon");
				if (guiMain.uiObjects[258].activeSelf)
				{
					mainScript.gelangweiltGenre = -1;
					guiMain.uiObjects[258].SetActive(false);
				}
			}

		}

		[HarmonyPostfix, HarmonyPatch(typeof(GUI_Main), "UpdateLangweiligIcon")]
		static void UpdateLangweiligIconPatch(GUI_Main __instance, mainScript ___mS_)
        {
			if (enabledBoredFans)
			{
				FreeUpdateRebalancer freeUpdateRebalancer = new FreeUpdateRebalancer();
				freeUpdateRebalancer.Rebalancer_ResetBoredFans(__instance, ___mS_);
			}
		}

		[HarmonyPostfix, HarmonyPatch(typeof(mainScript), "MonatlicheUpdates")]
		static void MonatlicheUpdatesPatch()
        {
			enabledBoredFans = false;
        }
		public void Rebalancer_EachButton()
		{
			gameScript gS = _tmpUpdateGameScript;
			GUI_Main guiMain = _tmpUpdateGuiMain;
			mainScript mainScript = _tmpUpdatemainScript;
			GameObject gameObject = GameObject.Find("GAME_" + gS.myID.ToString());
			gameScript gameScript = gameObject.GetComponent<gameScript>();
			Rebalancer_AddHype(gameScript);
			Rebalancer_AddFans(gameScript);
			Rebalancer_AddIP(gameScript);
			Rebalancer_MinusYear(mainScript);
			//enabledBoredFans = false;
			if (MainPlugin.dev_ResetBoredFansMultiplyValue.Value != 0f)
			{
				int judgeFansComeBack = Mathf.RoundToInt(UnityEngine.Random.Range(0, 100));
				if (judgeFansComeBack < MainPlugin.dev_ResetBoredFansMultiplyValue.Value)
				{
					if (mainScript.gelangweiltGenre != -1)
					{
						enabledBoredFans = true;
					}
				}
			}
		}
		public void taskUpdate_Complete_Startup(taskUpdate taskUpdate, gameScript gS_, GUI_Main guiMain_, mainScript mS_)
		{
			int roomID = Traverse.Create(taskUpdate).Method("FindMyRoomWithTask").GetValue<int>();
			_roomID = roomID;
			Traverse.Create(taskUpdate).Method("FindMyObject");
			_tmpUpdateGameScript = gS_;
			_tmpUpdateGuiMain = guiMain_;
			_tmpUpdatemainScript = mS_;
			Debug.Log("taskUpdate_Complete_Startup");
		}

		public void taskUpdate_Complete_Init()
        {
			//init member
			_buttonsAmount = 0;
			_roomID = 0;
			_tmpUpdateGameScript = null;
			_tmpUpdateGuiMain = null;
			_tmpUpdatemainScript = null;
			countLoopNumber = 0;
			Debug.Log("taskUpdate_Complete_Init");
		}

		public void taskUpdate_Complete_Injecter()
		{
			int roomID = _roomID;
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
						Rebalancer_EachButton();

						//ポイントなどの設定
						Rebalancer_AddBonusSells(taskUpdateObject);

					}
				}
				Debug.Log("taskUpdate_Complete_Injecter");
			}
		}
		[HarmonyPostfix, HarmonyPatch(typeof(taskUpdate), "Complete")]
		static void taskUpdate_Complete_PostPatch(taskUpdate __instance, gameScript ___gS_, GUI_Main ___guiMain_, mainScript ___mS_)
        {
			FreeUpdateRebalancer FreeUpdateRebalancer = new FreeUpdateRebalancer();
			FreeUpdateRebalancer.taskUpdate_Complete_Startup(__instance, ___gS_,___guiMain_, ___mS_);
			FreeUpdateRebalancer.taskUpdate_Complete_Injecter();

			///////// 2023.01.24 有限ループ処理用
			if (__instance.automatic && inputLoopNumber != null)
			{
				countLoopNumber++;
				if (Int32.Parse(inputLoopNumber.GetComponent<InputField>().text) == countLoopNumber)
				{
					__instance.automatic = false;
					countLoopNumber = 0;
				}
			}

			if (!__instance.automatic)
			{
				FreeUpdateRebalancer.taskUpdate_Complete_Init();
			}
			Debug.Log("Complete");
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
			//init _buttonsAmount and something;
			_buttonsAmount = 0;
			countLoopNumber = 0;
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



		//有限の処理を可能にするためのループ用テキストボックスを用意
		[HarmonyPostfix, HarmonyPatch(typeof(Menu_Dev_Update), "Init")]
		static void AddTextBox(Menu_Dev_Update __instance, gameScript ___gS_, mainScript ___mS_, GameObject ___main_, GUI_Main ___guiMain_)
        {
			if (GameObject.Find("Menu_Dev_Update/WindowMain/inputLoopNumber") == null)
			{
				//DevGameのテキストボックスをインスタンス化する
				//var cloneTextBox = Instantiate(menu_DevGame.uiObjects[0]);
				GameObject parent = GameObject.Find("CanvasInGameMenu");
				GameObject originalInputFieldName = parent.transform.Find("Menu_Dev_Game/WindowMain/Seite1/InputFieldName").gameObject;

				inputLoopNumber = Instantiate(originalInputFieldName);	//インスタンス化
				inputLoopNumber.name = "inputLoopNumber";
				inputLoopNumber.transform.parent = parent.transform.Find("Menu_Dev_Update/WindowMain").transform;   //親子関係を結ぶ

				//形や座標をいい感じにする
				GameObject ToggleAuto = parent.transform.Find("Menu_Dev_Update/WindowMain/ToggleAuto").gameObject;
				Vector3 set_Position = ToggleAuto.transform.localPosition;
				set_Position.x = -70;
				inputLoopNumber.transform.localPosition = set_Position;
				Vector2 sizeDelta = inputLoopNumber.GetComponent<RectTransform>().sizeDelta;
				sizeDelta.x = Mathf.RoundToInt(sizeDelta.x / 3);
				inputLoopNumber.GetComponent<RectTransform>().sizeDelta = sizeDelta;
				inputLoopNumber.GetComponent<InputField>().contentType = InputField.ContentType.IntegerNumber;	//整数のみ許可する
			}
			inputLoopNumber.GetComponentInChildren<Text>().text = "Loop N";
			inputLoopNumber.GetComponentInChildren<setText>().c = "Loop N";
			GameObject placeholder = inputLoopNumber.transform.Find("Placeholder").gameObject;
			placeholder.GetComponent<Text>().text = "Loop N";
		}

	}
}
