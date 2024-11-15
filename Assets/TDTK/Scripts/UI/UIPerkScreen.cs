﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TDTK;

namespace TDTK{

	public class UIPerkScreen : UIScreen {
		
		[Tooltip("Check to use custom layout\n\nYou can use arrange your own layout in the scroll view\nYou will need to manually assign the item to itemList and assign the perk associate to the item")]
		public bool customLayout=false;
		public static bool UseCustomLayout(){ return instance.customLayout; }
		
		[Space(10)]
		[Tooltip("A list of all the item in the scroll view\nNeed to be manually assign when using custom layout\nFill up automatically when not using custom layout")]
		public List<UIPerkItem> itemList=new List<UIPerkItem>();
		
		[Space(5)]
		public RectTransform selectHighlightT;
		
		[Space(5)]
		public Text lbPerkCount;
		
		[Space(10)]
		public Text lbPerkName;
		public Text lbPerkDesp;
		public Text lbPerkUnavailable;
		public List<UIObject> costItemList=new List<UIObject>();
		
		[Space(10)]
		public UIButton buttonPurchase;
		public UIButton buttonClose;
		
		private static UIPerkScreen instance;
		
		public override void Awake(){
			base.Awake();
			
			instance=this;
		}
		
		public override void Start(){ 
			base.Start();
			
			if(!PerkManager.IsEnabled()){
				thisObj.SetActive(false);
				return;
			}
			
			if(PerkManager.UseRscManagerForCost()){
				for(int i=0; i<RscManager.GetResourceCount(); i++){
					if(i>0) costItemList.Add(new UIObject(UI.Clone(costItemList[0].rootObj)));
					costItemList[i].Init();
					costItemList[i].imgMain.sprite=RscManager.GetRscIcon(i);
				}
			}
			else{
				costItemList[0].Init();
				costItemList[0].imgMain.sprite=PerkDB.GetRscIcon();
			}
			
			if(customLayout){
				List<Perk> perkList=PerkManager.GetPerkList();
				for(int i=0; i<itemList.Count; i++){
					itemList[i].Init();
					itemList[i].SetCallback(null, null, this.OnItem, null);
					
					bool matched=false;
					for(int n=0; n<perkList.Count; n++){
						if(itemList[i].linkedPerkPID==perkList[n].prefabID){
							itemList[i].linkedPerkIdx=n;	matched=true;
							itemList[i].imgMain.sprite=perkList[n].icon;
						}
					}
					
					if(!matched){
						Debug.LogWarning("No perk with matching prefab found");
						itemList[i].rootObj.SetActive(false);
						continue;
					}
					
					itemList[i].UnparentConnector();
					itemList[i].UnparentConnectorBase();
				}
			}
			else{
				List<Perk> perkList=PerkManager.GetPerkList();
				for(int i=0; i<perkList.Count; i++){
					if(i>0) itemList.Add(UIPerkItem.Clone(itemList[0].rootObj, "Item"+(i)));
					itemList[i].Init();
					itemList[i].SetCallback(null, null, this.OnItem, null);
					itemList[i].linkedPerkIdx=i;
					itemList[i].imgMain.sprite=perkList[i].icon;
				}
			}
			
			buttonPurchase.Init();
			buttonPurchase.SetCallback(null, null, this.OnPurchaseButton, null);
			
			if(UIControl.IsGameScene()){
				buttonClose.Init();
				buttonClose.SetCallback(null, null, this.OnCloseButton, null);
			}
			
			if(!UIControl.IsGameScene()){
				canvasGroup.alpha=1;
				thisObj.SetActive(true);
				StartCoroutine(DelayUpdateList());
			}
			else{
				thisObj.SetActive(false);
			}
			
			OnItem(itemList[0].rootObj);
		}
		
		
		public IEnumerator DelayUpdateList(){
			yield return null;
			UpdateList();
		}
		public void UpdateList(){
			for(int i=0; i<itemList.Count; i++){
				bool purchased=PerkManager.GetPerkFromList(itemList[i].linkedPerkIdx).IsPurchased();
				itemList[i].imgHighlight.enabled=purchased;
				
				if(purchased) itemList[i].button.interactable=purchased;
				else itemList[i].button.interactable=PerkManager.GetPerkFromList(itemList[i].linkedPerkIdx).IsAvailable();
				
				if(itemList[i].connector!=null) itemList[i].connector.SetActive(purchased);
			}
			
			lbPerkCount.text="Purchased Perk: "+PerkManager.GetPurchasedPerkCount();
		}
		
		
		private int selectedIdx=-1;
		public void OnItem(GameObject butObj, int pointerID=-1){
			int idx=GetItemIndex(butObj);
			
			selectedIdx=idx;
			
			selectHighlightT.localPosition=itemList[idx].rectT.localPosition-new Vector3(35, -35, 0);
			//Debug.Log(selectHighlightT.localPosition+"  "+itemList[idx].rectT.localPosition);
			
			Perk perk=PerkManager.GetPerkFromList(itemList[idx].linkedPerkIdx);
			
			lbPerkName.text=perk.name;
			lbPerkDesp.text=perk.desp;
			lbPerkUnavailable.text=perk.GetDespUnavailable();
			
			if(!PerkManager.UseRscManagerForCost()){
				int cost=perk.GetPurchaseCost();
				costItemList[0].lbMain.text=cost.ToString("f0");
			}
			else{
				List<float> cost=perk.GetPurchaseCostRsc();
				for(int i=0; i<cost.Count; i++) costItemList[i].lbMain.text=cost[i].ToString("f0");
			}
			
			buttonPurchase.SetActive(!perk.IsPurchased() & perk.IsAvailable());
		}
		private int GetItemIndex(GameObject butObj){
			for(int i=0; i<itemList.Count; i++){ if(itemList[i].rootObj==butObj) return i;}
			return 0;
		}
		
		
		public void OnPurchaseButton(GameObject butObj, int pointerID=-1){
			if(PerkManager.PurchasePerk(itemList[selectedIdx].linkedPerkIdx)){
				UpdateList();
				buttonPurchase.SetActive(false);
			}
		}
		
		public void OnCloseButton(GameObject butObj, int pointerID=-1){ Hide(); }
		
		
		
		
		
		private float cachedTimeScale=1;
		
		public static void Show(){ if(instance!=null) instance._Show(); }
		public override void _Show(float duration=0.25f){
			cachedTimeScale=Time.timeScale;
			Time.timeScale=0;
			
			UpdateList();
			
			UIControl.BlurFadeIn();
			
			base._Show();
		}
		public static void Hide(){ if(instance!=null) instance._Hide(); }
		public override void _Hide(float duration=0.25f){
			Time.timeScale=cachedTimeScale;
			
			UIControl.BlurFadeOut();
			
			base._Hide();
		}
		
		
		
		[System.Serializable]
		public class UIPerkItem : UIButton{
			
			[HideInInspector] 
			public int linkedPerkIdx=-1;
			[Tooltip("The prefabID of the perk associated to this item")]
			public int linkedPerkPID=-1;
			
			[HideInInspector] public GameObject connector;
			[HideInInspector] public GameObject connectorB;
			
			
			public UIPerkItem(){}
			public UIPerkItem(GameObject obj){ rootObj=obj; Init(); }
			
			public override void Init(){
				base.Init();
				
				if(!UIPerkScreen.UseCustomLayout()) return;
				
				foreach(Transform child in rectT){
					if(child.name=="ConnectorBase")	connectorB=child.gameObject;
					if(child.name=="Connector")			connector=child.gameObject;
				}
			}
			
			public void UnparentConnectorBase(){
				if(connectorB==null) return;
				connectorB.transform.SetParent(rootT.parent);
				connectorB.transform.SetAsFirstSibling();
			}
			public void UnparentConnector(){
				if(connector==null) return;
				connector.transform.SetParent(rootT.parent);
				connector.transform.SetAsFirstSibling();
			}
			
			public static new UIPerkItem Clone(GameObject srcObj, string name="", Vector3 posOffset=default(Vector3)){
				GameObject newObj=UI.Clone(srcObj, name, posOffset);
				return new UIPerkItem(newObj);
			}
		}
	}

}
