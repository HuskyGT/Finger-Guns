using BepInEx;
using System;
using UnityEngine;
using Utilla;
using UnityEngine.XR;
using System.ComponentModel;


namespace FingerGuns
{
	[Description("HauntedModMenu")]
	[ModdedGamemode]
	[BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	public class Plugin : BaseUnityPlugin
	{

		GameObject handl;
		GameObject handr;
		GameObject leftshot;
		GameObject rightshot;
		GameObject left;
		GameObject right;
		GameObject allbullets;
		Vector3 leftpos;
		Vector3 rightpos;

		float rdelay = 0.05f;
		float rtime;
		bool ractive = false;

		float ldelay = 0.05f;
		float ltime;
		bool lactive = false;
		

		void OnEnable()
		{
			HarmonyPatches.ApplyHarmonyPatches();
			Utilla.Events.GameInitialized += OnGameInitialized;
		}

		void OnDisable()
		{
			HarmonyPatches.RemoveHarmonyPatches();
			Utilla.Events.GameInitialized -= OnGameInitialized;
		}


		void OnGameInitialized(object sender, EventArgs e)
		{
			handl = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L/palm.01.L/");
			handr = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R/");
			right = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			right.transform.SetParent(handr.transform, true);
			right.transform.localPosition = new Vector3(0f, 0.5f, 0f);
			GameObject.Destroy(right.GetComponent<Rigidbody>());
			GameObject.Destroy(right.GetComponent<SphereCollider>());
			GameObject.Destroy(right.GetComponent<Renderer>());

			left = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			left.transform.SetParent(handl.transform, true);
			left.transform.localPosition = new Vector3(0f, 0.5f, 0f);
			GameObject.Destroy(left.GetComponent<Rigidbody>());
			GameObject.Destroy(left.GetComponent<SphereCollider>());
			GameObject.Destroy(left.GetComponent<Renderer>());
			GameObject bullets = new GameObject();
			bullets.name = "bullets";
			bullets.layer = 8;
			allbullets = GameObject.Find("bullets");
			
		}
		void Update()
		{
			if (inRoom)
			{
				bool rbottombutton;
				bool rgrip;
				InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.secondaryButton, out rbottombutton);
				InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.gripButton, out rgrip);

				bool lbottombutton;
				bool lgrip;
				InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.secondaryButton, out lbottombutton);
				InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.gripButton, out lgrip);

				if (rbottombutton && rgrip)
				{
					if (ractive)
					{
						if (rbottombutton && rgrip)
						{
							rightpos = right.transform.position;
							rightshot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
							rightshot.GetComponent<Renderer>().material.color = Color.yellow;
							rightshot.AddComponent<Rigidbody>();
							rightshot.transform.localScale = new Vector3(0.050f, 0.050f, 0.050f);
							rightshot.layer = 8;
							rightshot.transform.position = handr.transform.position;
							rightshot.transform.LookAt(rightpos);
							rightshot.GetComponent<Rigidbody>().AddForce(rightshot.transform.forward * 1750);
							rightshot.name = "bullet";
							rightshot.transform.SetParent(allbullets.transform, true);
						}
					}
					if (ractive)
					{

					}
					if (Time.time > rtime)
					{
						if (!ractive)
						{
							ractive = true;
						}
					}
					else
					{
						ractive = false;
					}
					rtime = Time.time + rdelay;
				}
				if (lbottombutton && lgrip)
				{
					if (lactive)
					{
						if (lbottombutton && lgrip)
						{
							leftpos = left.transform.position;
							leftshot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
							leftshot.GetComponent<Renderer>().material.color = Color.yellow;
							leftshot.AddComponent<Rigidbody>();
							leftshot.transform.localScale = new Vector3(0.050f, 0.050f, 0.050f);
							leftshot.layer = 8;
							leftshot.transform.position = handl.transform.position;
							leftshot.transform.LookAt(leftpos);
							leftshot.GetComponent<Rigidbody>().AddForce(leftshot.transform.forward * 1750);
							leftshot.name = "bullet";
							leftshot.transform.SetParent(allbullets.transform, true);
						}
					}
					if (lactive)
					{

					}
					if (Time.time > ltime)
					{
						if (!lactive)
						{
							lactive = true;
						}
					}
					else
					{
						lactive = false;
					}
					ltime = Time.time + ldelay;
				}
				if (!allbullets)
				{
					GameObject bullets = new GameObject();
					bullets.name = "bullets";
					bullets.layer = 8;
					allbullets = GameObject.Find("bullets");
				}
				if (allbullets.transform.childCount > 75)
				{
					Destroy(allbullets);
					GameObject bullets = new GameObject();
					bullets.name = "bullets";
					bullets.layer = 8;
					allbullets = GameObject.Find("bullets");
				}
			}
		}
		bool inRoom;
		[ModdedGamemodeJoin]
		public void OnJoin(string gamemode)
		{
			inRoom = true;
			Destroy(allbullets);
		}
		[ModdedGamemodeLeave]
		public void OnLeave(string gamemode)
		{
			inRoom = false;
			Destroy(allbullets);
		}
	}
}




	


	


