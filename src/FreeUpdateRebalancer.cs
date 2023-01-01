
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


/*
namespace FreeUpdatePatcher
{
	[HarmonyPatch(typeof(Menu_Dev_Update))]
	public class FreeUpdatePatcher
    {
		//[HarmonyPrefix, HarmonyPatch(typeof(Menu_Dev_Update), "BUTTON_Start")]
		static bool Prefix(Menu_Dev_Update __instance, gameScript ___gS_, gameScript ___rS_, GUI_Main ___guiMain_, textScript ___tS_, sfxScript ___sfx_, mainScript ___mS_)
		{
			Menu_Dev_Update m = new Menu_Dev_Update();
			bool[] buttonAdds = Traverse.Create(m).Field("buttonAdds").GetValue<bool[]>();
			bool[] sprachen = Traverse.Create(m).Field("sprachen").GetValue<bool[]>();
			//!!!!!testing!!!!!!!
			gameScript hoge = (gameScript)Traverse.Create(m).Field("gS_").GetValue();
			//bool[] buttonAdds = AccessTools.StaticFieldRefAccess<bool[]>(typeof(Menu_Dev_Update), "buttonAdds");
			//bool[] sprachen = AccessTools.StaticFieldRefAccess<bool[]>(typeof(Menu_Dev_Update), "sprachen");
			var GetDevCosts = AccessTools.Method(typeof(Menu_Dev_Update), "GetDevCosts").;
			var GetP_Gameplay = AccessTools.Method(typeof(Menu_Dev_Update), "GetP_Gameplay");
			var GetP_Grafik = AccessTools.Method(typeof(Menu_Dev_Update), "GetP_Grafik");
			var GetP_Sound = AccessTools.Method(typeof(Menu_Dev_Update), "GetP_Sound");
			var GetP_Technik = AccessTools.Method(typeof(Menu_Dev_Update), "GetP_Technik");
			var GetP_Bugs = AccessTools.Method(typeof(Menu_Dev_Update), "GetP_Bugs");
			int num = Mathf.RoundToInt((float)GetDevCosts.Invoke(__instance, new object[] { }));
			if (!___gS_)
			{
				return false;
			}
			if (!___rS_)
			{
				return false;
			}
			if (num <= 0)
			{
				___guiMain_.MessageBox(___tS_.GetText(662), false);
				return false;
			}
			if (__instance.uiObjects[37].GetComponent<Toggle>().isOn)
			{
				bool flag = false;
				for (int i = 0; i < buttonAdds.Length; i++)
				{
					if (buttonAdds[i])
					{
						flag = true;
					}
				}
				if (!flag)
				{
					___guiMain_.MessageBox(___tS_.GetText(727), false);
					return false;
				}
			}
			___sfx_.PlaySound(3, true);
			___mS_.Pay((long)num, 15);
			taskUpdate taskUpdate = ___guiMain_.AddTask_Update();
			taskUpdate.Init(false);
			taskUpdate.targetID = ___gS_.myID;
			taskUpdate.devCosts = Mathf.RoundToInt((float)GetDevCosts.Invoke(__instance, new object[] { }));
			taskUpdate.pointsGameplay = (int)GetP_Gameplay.Invoke(__instance, new object[] { });
			taskUpdate.pointsGrafik = (int)GetP_Grafik.Invoke(__instance, new object[] { });
			taskUpdate.pointsSound = (int)GetP_Sound.Invoke(__instance, new object[] { });
			taskUpdate.pointsTechnik = (int)GetP_Technik.Invoke(__instance, new object[] { });
			taskUpdate.pointsBugs = (int)GetP_Bugs.Invoke(__instance, new object[] { });
			taskUpdate.automatic = __instance.uiObjects[37].GetComponent<Toggle>().isOn;
			float num2 = (float)___gS_.GetGesamtDevPoints();
			num2 *= 0.1f;
			taskUpdate.points = (float)Mathf.RoundToInt(num2);
			for (int j = 0; j < buttonAdds.Length; j++)
			{
				if (buttonAdds[j])
				{
					taskUpdate.quality += 0.1f;
					taskUpdate.points += (float)Mathf.RoundToInt((float)___gS_.GetGesamtDevPoints() * 0.02f);
				}
			}
			for (int k = 0; k < sprachen.Length; k++)
			{
				if (sprachen[k])
				{
					taskUpdate.quality += 0.02f;
					taskUpdate.sprachen[k] = sprachen[k];
					taskUpdate.points += 10f;
				}
			}
			taskUpdate.points += (float)GetP_Bugs.Invoke(__instance, new object[] { }) * 0.3f;
			taskUpdate.pointsLeft = taskUpdate.points;
			GameObject gameObject = GameObject.Find("Room_" + ___rS_.myID.ToString());
			if (gameObject)
			{
				gameObject.GetComponent<roomScript>().taskID = taskUpdate.myID;
			}
			___guiMain_.CloseMenu();
			gameObject.SetActive(false);
			return false;
		}
	}
}
*/


namespace FreeUpdateRebalancer
{

	public class FreeUpdateRebalancer
	{
		/*
		[HarmonyPrefix, HarmonyPatch(typeof(Menu_Dev_Update), "BUTTON_Start")]
		static bool BUTTON_StartPrefix(Menu_Dev_Update __instance, gameScript ___gS_, gameScript ___rS_, GUI_Main ___guiMain_, textScript ___tS_, sfxScript ___sfx_, mainScript ___mS_)
		{
			Debug.Log("Button1");
			bool[] buttonAdds = Traverse.Create(__instance).Field("buttonAdds").GetValue<bool[]>();
			bool[] sprachen = Traverse.Create(__instance).Field("sprachen").GetValue<bool[]>();
			//!!!!!testing!!!!!!!
			//gameScript gS_ = (gameScript)Traverse.Create(m).Field("gS_").GetValue();
			//gameScript rS_ = (gameScript)Traverse.Create(m).Field("rS_").GetValue();
			//GUI_Main guiMain_ = (GUI_Main)Traverse.Create(m).Field("guiMain_").GetValue();
			//textScript tS_ = (textScript)Traverse.Create(m).Field("tS_").GetValue();
			//sfxScript sfx_ = (sfxScript)Traverse.Create(m).Field("sfx_").GetValue();
			//mainScript mS_ = (mainScript)Traverse.Create(m).Field("mS_").GetValue();
			//bool[] buttonAdds = AccessTools.StaticFieldRefAccess<bool[]>(typeof(Menu_Dev_Update), "buttonAdds");
			//bool[] sprachen = AccessTools.StaticFieldRefAccess<bool[]>(typeof(Menu_Dev_Update), "sprachen");
			long GetDevCosts = Traverse.Create(__instance).Method("GetDevCosts").GetValue<long>();
			int GetP_Gameplay = Traverse.Create(__instance).Method("GetP_Gameplay").GetValue<int>();
			int GetP_Grafik = Traverse.Create(__instance).Method("GetP_Grafik").GetValue<int>();
			int GetP_Sound = Traverse.Create(__instance).Method("GetP_Sound").GetValue<int>();
			int GetP_Technik = Traverse.Create(__instance).Method("GetP_Technik").GetValue<int>();
			int GetP_Bugs = Traverse.Create(__instance).Method("GetP_Bugs").GetValue<int>();
			Debug.Log("gS_ : " + ___gS_);
			Debug.Log("___gS_.GetGesamtDevPoints() : " + ___gS_.GetGesamtDevPoints());
			Debug.Log("rS : " + ___rS_);
			Debug.Log("guiMain_ : " + ___tS_);
			Debug.Log("tS_ : " + ___tS_);
			Debug.Log("sfx_ : " + ___sfx_);
			Debug.Log("GetDevCosts : " + GetDevCosts);
			Debug.Log("Button2");
			int num = Mathf.RoundToInt((float)GetDevCosts);
			if (!___gS_)
			{
				Debug.Log("Error! gS_");
				//return;
			}
			if (!___rS_)
			{
				Debug.Log("Error! rS_");
				//return;
			}
			if (num <= 0)
			{
				___guiMain_.MessageBox(___tS_.GetText(662), false);
				return false;
			}
			if (__instance.uiObjects[37].GetComponent<Toggle>().isOn)
			{
				bool flag = false;
				for (int i = 0; i < buttonAdds.Length; i++)
				{
					if (buttonAdds[i])
					{
						flag = true;
					}
				}
				if (!flag)
				{
					___guiMain_.MessageBox(___tS_.GetText(727), false);
					return false;
				}
			}
			Debug.Log("Button3");
			___sfx_.PlaySound(3, true);
			___mS_.Pay((long)num, 15);
			//mS_.Pay((long)(num * 99), 15);
			GameObject gameObject = GameObject.Find("Room_" + ___rS_.myID.ToString());
			var taskID = gameObject.GetComponent<roomScript>().taskID;
			taskUpdate taskUpdate = ___guiMain_.AddTask_Update();
			taskUpdate.Init(false);
			taskUpdate.targetID = ___gS_.myID;
			Debug.Log("taskUpdate : " + taskUpdate);
			Debug.Log("taskUpdate.targetID : " + taskUpdate.targetID);
			Debug.Log("taskUpdate.myID : " + taskUpdate.myID);
			taskUpdate.devCosts = Mathf.RoundToInt((float)GetDevCosts);
			taskUpdate.pointsGameplay = (int)GetP_Gameplay;
			taskUpdate.pointsGrafik = (int)GetP_Grafik;
			taskUpdate.pointsSound = (int)GetP_Sound;
			taskUpdate.pointsTechnik = (int)GetP_Technik;
			taskUpdate.pointsBugs = (int)GetP_Bugs;
			taskUpdate.automatic = __instance.uiObjects[37].GetComponent<Toggle>().isOn;
			Debug.Log("Button4");
			float num2 = (float)___gS_.GetGesamtDevPoints();
			num2 *= 0.1f;
			taskUpdate.points = (float)Mathf.RoundToInt(num2);
			for (int j = 0; j < buttonAdds.Length; j++)
			{
				if (buttonAdds[j])
				{
					taskUpdate.quality += 0.1f;
					taskUpdate.points += (float)Mathf.RoundToInt((float)___gS_.GetGesamtDevPoints() * 0.02f);
				}
			}
			for (int k = 0; k < sprachen.Length; k++)
			{
				if (sprachen[k])
				{
					taskUpdate.quality += 0.02f;
					taskUpdate.sprachen[k] = sprachen[k];
					taskUpdate.points += 10f;
				}
			}
			Debug.Log("Button5");
			taskUpdate.points += (float)GetP_Bugs * 0.3f;
			taskUpdate.pointsLeft = taskUpdate.points;
			Debug.Log("Button6");
			if (gameObject)
			{
				gameObject.GetComponent<roomScript>().taskID = taskUpdate.myID;
				Debug.Log("Button 6 ex");
			}
			Debug.Log("Button7");
			___guiMain_.CloseMenu();
			Debug.Log("Button8");
			Debug.Log("Button Activated");
			return false;
		}
		*/
		/// <summary>
		/// 押したボタンの数を記録しておく。
		/// </summary>
		public static int buttonsAmount { get; private set; }

		public void AddSomethingEachButton()
		{
			foreach (gameScript gameScript in UnityEngine.Object.FindObjectsOfType<gameScript>())
			{
				if (gameScript != null)
				{
					GameObject gameObject2 = GameObject.Find("GAME_" + gameScript.myID.ToString());
					gameScript gameScript2 = gameObject2.GetComponent<gameScript>();
					if (MainPlugin.devAddHypeAdditionValue.Value != 0f)
					{
						gameScript2.AddHype(MainPlugin.devAddHypeAdditionValue.Value);
					}
					if (MainPlugin.devAddFunAdditionValue.Value != 0f)
					{
						//genres genres = gameObject2.GetComponent<genres>();
						if (gameScript2.mS_.genres_.GetAmountFans() == 0)
						{
							gameScript2.mS_.genres_.genres_FANS[gameScript2.maingenre] += 1;

						}
						int genre_ = gameScript2.maingenre;
						if (gameScript2.subgenre != -1 && UnityEngine.Random.Range(0, 100) > 70)
						{
							genre_ = gameScript2.subgenre;
						}

						int sells = gameScript2.sellsPerWeek[1];
						float sellsEffect = gameScript2.sellsPerWeek[1] * 0.01f;
						try
						{
							gameScript2.mS_.AddFans(Mathf.RoundToInt(sellsEffect * MainPlugin.devAddFunAdditionValue.Value), genre_);
						}
						catch { }
					}
				}
			}
		}
		public void taskUpdate_Complete_Injecter()
		{
			FreeUpdateRebalancer freeUpdateRebalancer = new FreeUpdateRebalancer();
			foreach (roomScript roomScript in UnityEngine.Object.FindObjectsOfType<roomScript>())
            {
				if(roomScript != null)
                {
					GameObject taskGameObject = roomScript.taskGameObject;
                    if (taskGameObject)
                    {
						taskUpdate taskUpdateObject = taskGameObject.GetComponent<taskUpdate>();
						if (taskUpdateObject)
                        {
							//オリジナルのFree Updateで増やした分を減らす。
							taskUpdateObject.gS_.costs_updates += (long)taskUpdateObject.devCosts;
							taskUpdateObject.gS_.amountUpdates++;
							taskUpdateObject.gS_.bonusSellsUpdates -= taskUpdateObject.quality / (float)taskUpdateObject.gS_.amountUpdates;

							//予め保存しておいたボタンの数で増やしていく。
							for (int j = 0; j < buttonsAmount; j++)
							{
								//Hype, Fansなどの設定
								freeUpdateRebalancer.AddSomethingEachButton();

								//ポイントなどの設定
								taskUpdateObject.gS_.bonusSellsUpdates += (taskUpdateObject.quality * MainPlugin.devBonusSellsMultiplyValue.Value)  / (float)taskUpdateObject.gS_.amountUpdates;
								if ((double)taskUpdateObject.gS_.bonusSellsUpdates > MainPlugin.devBonusSellsMultiplyValue.Value)
								{
									taskUpdateObject.gS_.bonusSellsUpdates = MainPlugin.devBonusSellsMultiplyValue.Value;
								}

							}
							//init buttonsAmount
							buttonsAmount = 0;
						
						}
					}
                }
            }
		}

		[HarmonyPostfix, HarmonyPatch(typeof(taskUpdate), "Complete")]
		static void taskUpdate_Complete_PrePatch()
        {
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
			//init buttonsAmount;
			buttonsAmount = 0;
			bool[] buttonAdds = Traverse.Create(__instance).Field("buttonAdds").GetValue<bool[]>();
			for (int j = 0; j < buttonAdds.Length; j++)
			{
				if (buttonAdds[j])
				{
					buttonsAmount++;
					//FreeUpdateRebalancer freeUpdateRebalancer = new FreeUpdateRebalancer();
					//freeUpdateRebalancer.AddSomethingEachButton();
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
			m.Method("UpdateGUI");
		}



		/*
		// Total Game Development Speed with Character
		[HarmonyPrefix, HarmonyPatch(typeof(characterScript), "GetWorkSpeed")]
		static bool GetWorkSpeed_PrePatch( characterScript __instance ,mainScript ___mS_, ref float __result, roomScript ___roomS_)
        {
			float num =0.01f * ((__instance.s_motivation + 10f) * 0.5f);
			switch (___mS_.personal_druck)
			{
				case 1:
					num *= 1.25f;
					break;
				case 2:
					num *= 1.5f;
					break;
			}
			if (__instance.perks[29])
			{
				num *= 1.1f;
			}
			float num2 = (float)___mS_.GetAchivementBonus(9);
			num2 *= 0.01f;
			num += num * num2;
			if (__instance.krank > 0)
			{
				num *= 0.25f;
			}
			//**** Added ****
			try
			{
				taskUpdate taskUpdate = ___roomS_.GetTaskUpdate();
				if (taskUpdate.myID == ___roomS_.taskID)
				{
					num *= MainPlugin.devTimeMultiplyValue.Value;
				}
			} catch (Exception e){}
			//Debug.Log("TaskGame ID :" + taskID);
			//**** Added ended ****
			if (___mS_.settings_arbeitsgeschwindigkeitAnpassen)
				{
					__result = num * (___mS_.speedSetting * 20f);
					return false;
				}
			__result = num;
			return false;
		}
		*/
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
			catch (Exception e) { }
			//Debug.Log("TaskGame ID :" + taskID);

			//**** Added ended ****
			__result = num;
		}
	}
}
