using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class cardGenerator : MonoBehaviour {

	public List<GameObject> pickedcardList = new List<GameObject>();
	public List<GameObject> unpickedcardList = new List<GameObject>();
	public GameObject[] cardprefabs;
	public GameObject empty;
	public Transform[] positions;
	public GameObject maincamera;
	private cardSelection cardselection;
	private int index1, index2;
	private int prefabIndex;


	// Use this for initialization
	void Start () {
		pickedcardList.Clear();
		unpickedcardList.Clear();

		empty = null;

		for(int i = 0; pickedcardList.Count< 16; i++){
			pickedcardList.Add (empty);
		}

		for(int i = 0; unpickedcardList.Count< 52; i++){
			unpickedcardList.Add (cardprefabs[i]);
		}

		firstdealCards();
	
	}

	void Awake(){
		maincamera = GameObject.FindWithTag("MainCamera");
		cardselection = maincamera.GetComponent<cardSelection>();
	}
	
	// Update is called once per frame
	void Update () {

		if(cardselection.cardsismatched){
			clearcard();
		}

	
	}

	// deal cards in the first time
	void firstdealCards(){
		for(int i = 0; i < pickedcardList.Count; i ++){
		    prefabIndex = UnityEngine.Random.Range(0,unpickedcardList.Count-1);
			if(unpickedcardList[prefabIndex]!= null){
				pickedcardList[i] = unpickedcardList[prefabIndex];
				unpickedcardList[prefabIndex] = null;
				Instantiate(pickedcardList[i], positions[i].position, Quaternion.identity);
			}else{
				i --;
			}
		}
	}

	//clear cards that have been picked
	void clearcard(){
		if(cardselection.firstcard !=null){
			Debug.Log(cardselection.firstcard.name);
			foreach(var item in pickedcardList){
				if(item != null){
					if(item.name + "(Clone)" == cardselection.firstcard.name){
						index1 = pickedcardList.IndexOf(item);
						break;
					}
				}

			}
		}

		if(cardselection.secondcard !=null){
			Debug.Log(cardselection.secondcard.name);
			foreach(var item in pickedcardList){
				if(item != null){
					if(item.name + "(Clone)" == cardselection.secondcard.name){
						index2 = pickedcardList.IndexOf(item);
						break;
					}
				}

			}
		}

	
		pickedcardList[index1] = null;
		StartCoroutine(addcards(index1));

		pickedcardList[index2] = null;
		StartCoroutine(addcards(index2));

		cardselection.cardsismatched = false;
		cardselection.value = 0;
	}
		
		// add a card at the table
	IEnumerator addcards(int index){
		yield return new WaitForSeconds(cardselection.waitTime+2.0f);
		do{
			prefabIndex = UnityEngine.Random.Range(0,unpickedcardList.Count-1);
		}while(unpickedcardList[prefabIndex] == null);
		pickedcardList[index] = unpickedcardList[prefabIndex];
		unpickedcardList[prefabIndex] = null;
		Instantiate(pickedcardList[index], positions[index].position, Quaternion.identity);
		
	}
}
