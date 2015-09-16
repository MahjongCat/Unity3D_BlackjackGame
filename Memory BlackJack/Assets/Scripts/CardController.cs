using UnityEngine;
using System.Collections;


public class CardController : MonoBehaviour {

	public Sprite img1, img2;
	Animator anim;
	public GameObject maincamera;
	private cardSelection cardselection;
	private ChangeImg changimg;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}

	void Awake(){
		maincamera = GameObject.FindWithTag("MainCamera");
		cardselection = maincamera.GetComponent<cardSelection>();
		img1= GameObject.Find ("img1").GetComponent<SpriteRenderer>().sprite;
		img2 = GameObject.Find ("img2").GetComponent<SpriteRenderer>().sprite;
		changimg = maincamera.GetComponent<ChangeImg>();
	}
	
	// Update is called once per frame
	void Update () {

		#if UNITY_EDITOR
		//show card value
		if (Input.GetKeyDown (KeyCode.A)) {
			anim.SetTrigger ("FlipUp");
		}

		//cover card value
		if (Input.GetKeyDown (KeyCode.S)) {
			anim.SetTrigger ("FlipDown");
			cardselection.firstcard = null;
			cardselection.secondcard = null;
		}

		#endif

		//Change card's background image
		if(changimg.change){
			SetSprite2();
		}else{
			SetSprite();
		}
	
	}

	//set card's background image to image1
	void SetSprite()
	{
		gameObject.GetComponentInChildren<SpriteRenderer>().sprite = img1;
	}

	//set card's background image to image2
	void SetSprite2()
	{
		gameObject.GetComponentInChildren<SpriteRenderer>().sprite = img2;
	}
}
