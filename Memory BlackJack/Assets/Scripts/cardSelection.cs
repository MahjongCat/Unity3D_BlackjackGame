using UnityEngine;
using System.Collections;

public class cardSelection : MonoBehaviour {

	public GameObject firstcard;
	public GameObject secondcard;
	private RaycastHit2D hit;
	public AudioSource flipup, flipdown, match;
	private Animator anim1, anim2;
	public float waitTime = 2.0f;
	public GUIText userscore;
	public GUIText opponentscore;
	public int userScore;
	public int opponentScore;
	public bool cardsismatched;
	public GameObject CardGenerator;
	private cardGenerator cardgenerator;
	private int Index1, Index2;
	public int value = 0;
	public bool userplay = false;
	public bool opponentplay = false;
	private bool useroperation = true;
	public GameObject effectprefab;

	// Use this for initialization
	void Start () {
		firstcard = null;
		secondcard = null;
		userScore = 0;
		opponentScore = 0;
		userscore.text = userScore.ToString();
		opponentscore.text = opponentScore.ToString();
		cardsismatched = false;

	}

	void Awake(){
		CardGenerator = GameObject.FindWithTag("CardGenerator");
		cardgenerator = CardGenerator.GetComponent<cardGenerator>();
	}

	
	// Update is called once per frame
	void Update () {

		#region TEST
		#if UNITY_EDITOR
		//opponent pick tow cards with same value
		if (Input.GetKeyDown (KeyCode.D)) {
			useroperation= false;
			opponentplay = true;
			userplay = false;
			firstcard = null;
			secondcard = null;

				for(int i = 0;  i< cardgenerator.pickedcardList.Count-1; i++){
					if(cardgenerator.pickedcardList[i] == null) continue;
				if(secondcard == null){
					for(int j = i+1; j <cardgenerator.pickedcardList.Count-1; j++){
						
						if(cardgenerator.pickedcardList[j] == null) continue;
						if(cardgenerator.pickedcardList[i].tag == cardgenerator.pickedcardList[j].tag ){
							firstcard =  GameObject.Find (cardgenerator.pickedcardList[i].name + "(Clone)");
							showFirstCard();
							secondcard = GameObject.Find (cardgenerator.pickedcardList[j].name + "(Clone)");
							showSecondCard();
							opponentScore = opponentScore+ value;
							opponentscore.text = opponentScore.ToString();
							break;
						}
					}
				}
				}

		}

		if (Input.GetKeyUp (KeyCode.D)) {
			useroperation= true;
		}
        #endif
		#endregion

		if(Input.GetMouseButtonDown(0)){
		    hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if(hit.collider != null){
				if (hit.collider.gameObject.name.Contains("Card") && useroperation) {
					if(firstcard == null && secondcard == null){
						firstcard = hit.collider.gameObject;
						showFirstCard();
					}else{
						if(secondcard == null){
							userplay = true;
							opponentplay = false;
							if( hit.collider.gameObject.name != firstcard.name){
								secondcard = hit.collider.gameObject;
								showSecondCard();
								
								StartCoroutine(autoSelection());
							}


						}  
						
					}
				}
			}

		}

		if(userplay){
			userScore = userScore + value;
			userscore.text = userScore.ToString();
			value = 0;
	
		}else if(opponentplay){
			opponentScore = opponentScore + value;
			opponentscore.text = opponentScore.ToString();
			value = 0;
		}

		if((userScore == 21 && opponentScore <21) || (userScore < 21 && opponentScore > 21)){
			StartCoroutine(gameWon());
		}else if((userScore < 21 && opponentScore ==21) || (userScore > 21 && opponentScore < 21)){
			StartCoroutine(gameLost());
		}
	}

	void showFirstCard(){

		anim1 = firstcard.GetComponent<Animator>();
		if(!anim1.GetCurrentAnimatorStateInfo(0).IsName("FlipUp")){
			anim1.SetTrigger("FlipUp");
			if(! flipup.isPlaying) flipup.Play();
		}  
	}

	void showSecondCard(){

	
			anim2 = secondcard.GetComponent<Animator>();
			if(!anim2.GetCurrentAnimatorStateInfo(0).IsName("FlipUp")){
				anim2.SetTrigger("FlipUp");
				if(! flipup.isPlaying) flipup.Play();
				checkIfMatch();

			}
		

	}

	void opponentSelection(){
		do{
			Index1 = UnityEngine.Random.Range(0,cardgenerator.pickedcardList.Count-1);
		}while(cardgenerator.pickedcardList[Index1] == null);

		firstcard = GameObject.Find (cardgenerator.pickedcardList[Index1].name + "(Clone)");
	
		showFirstCard();

		do{
			Index2 = UnityEngine.Random.Range(0,cardgenerator.pickedcardList.Count-1);
		}while(cardgenerator.pickedcardList[Index2] == null || Index2 == Index1);

		secondcard = GameObject.Find (cardgenerator.pickedcardList[Index2].name + "(Clone)");
	
		showSecondCard();
		opponentScore = opponentScore+ value;
		opponentscore.text = opponentScore.ToString();


	}

	IEnumerator autoSelection(){
		useroperation= false;
		yield return new WaitForSeconds(waitTime + 4.0f);
		opponentplay = true;
		userplay = false;
		opponentSelection();
		yield return new WaitForSeconds(waitTime);
		useroperation = true;

	}
		
	void checkIfMatch(){
		if(firstcard.tag == secondcard.tag){
			StartCoroutine(isMatched());
		}
		else{
			StartCoroutine(notMatched());
		}

	}

	IEnumerator isMatched(){
		cardsismatched = true;
		yield return new WaitForSeconds(waitTime);
		getScore();
		Instantiate(effectprefab, firstcard.transform.position, Quaternion.identity);
		Instantiate(effectprefab, secondcard.transform.position, Quaternion.identity);
		if(! match.isPlaying) match.Play();
		Destroy(firstcard);
		Destroy(secondcard);
		firstcard = null;
		secondcard = null;

	}

	IEnumerator notMatched(){
		value = 0;
		yield return new WaitForSeconds(waitTime);
		if(!anim1.GetCurrentAnimatorStateInfo(0).IsName("FlipDown")){
			anim1.SetTrigger("FlipDown");
			if(! flipdown.isPlaying) flipdown.Play();
		}  
		if(!anim2.GetCurrentAnimatorStateInfo(0).IsName("FlipDown")){
			anim2.SetTrigger("FlipDown");
			if(! flipdown.isPlaying) flipdown.Play();
		}
	
		firstcard = null;
		secondcard = null;

	}

	void getScore(){
		value = 0;
		switch (firstcard.tag){
		case "2":
			value = value +2;
			break;
		case "3":
			value = value +3;
			break;
		case "4":
			value = value +4;
			break;
		case "5":
			value = value +5;
			break;
		case "6":
			value = value +6;
			break;
		case "7":
			value = value +7;
			break;
		case "8":
			value = value +8;
			break;
		case "9":
			value = value +9;
			break;
		case "10":
			value = value +10;
			break;
		case "J":
			value = value +10;
			break;
		case "Q":
			value = value +10;
			break;
		case "K":
			value = value +10;
			break;
		case "A":
			value = value +1;
			break;
		}

		Debug.Log(value);

	}
	// show user "win" GUI
	IEnumerator gameWon(){
		userplay= false;
		opponentplay = false;
		yield return new WaitForSeconds(waitTime-1.0f);
		userscore.text = "win";
		opponentscore.text = "lose";
		StartCoroutine(reloadScene());

	}

	// show user "lost" GUI
	IEnumerator gameLost(){
		userplay= false;
		opponentplay = false;
		yield return new WaitForSeconds(waitTime-1.0f);
		userscore.text = "lose";
		opponentscore.text = "win";
		StartCoroutine(reloadScene());
	}
	

	// reload current scene
	IEnumerator reloadScene(){
		yield return new WaitForSeconds(waitTime);
		Application.LoadLevel(Application.loadedLevel);
	}


}