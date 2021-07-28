using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;


public class PoolGameController : MonoBehaviour {
	public GameObject cue;
	public GameObject cueBall;
	public GameObject redBalls;
	public GameObject mainCamera;
	public GameObject scoreBar;
	public GameObject winnerMessage;
	public PhotonView pv;
	
	public float maxForce;
	public float minForce;
	public Vector3 strikeDirection;

	public const float MIN_DISTANCE = 27.5f;
	public const float MAX_DISTANCE = 32f;
	
	public IGameObjectState currentState;

	public Pool_Player CurrentPlayer;
	public Pool_Player OtherPlayer;

	//Photon.Realtime.Player _CurrentPlayer;
	//int myID = player.get();;
	//int whoseTurn = 0;
	public Photon.Realtime.Player _CurrentPlayer ;
	public int myID  ;
	public int whoseTurn;
	

	private bool currentPlayerContinuesToPlay = false;

	// This is kinda hacky but works
	static public PoolGameController GameInstance {
		get;
		private set;
	}

    private void Awake()
    {
		cue = GameObject.Find("Cue");
		cueBall = GameObject.Find("CueBall");
		redBalls = GameObject.Find("RedBalls");
		mainCamera = GameObject.Find("Main Camera");
		scoreBar = GameObject.Find("ScoreBar");
		winnerMessage = GameObject.Find("WinnerMessage");
		pv = PhotonView.Get(this);
	}

    void Start() {
		strikeDirection = Vector3.forward;
		CurrentPlayer = new Pool_Player("John");
		OtherPlayer = new Pool_Player("Doe");
		
		GameInstance = this;
		winnerMessage.GetComponent<Canvas>().enabled = false;

		currentState = new GameStates.WaitingForStrikeState(this);
		_CurrentPlayer = PhotonNetwork.LocalPlayer;
	    myID = _CurrentPlayer.ActorNumber;
		whoseTurn = 1;
		foreach (Player p in PhotonNetwork.PlayerList)
		{
			Debug.Log("actor number "+ p.ActorNumber);
		}
	}
    //if(myid==whoseturn)
    void Update()
    {

        if (myID == whoseTurn)
            currentState.Update();
        //print("my id " + myID);
        //print("whose turn " + whoseTurn);
    }

    void FixedUpdate()
    {
        if (myID == whoseTurn)
            currentState.FixedUpdate();
    }

    void LateUpdate()
    {
        if (myID == whoseTurn)
            currentState.LateUpdate();
    }

    public void BallPocketed(int ballNumber) {
		currentPlayerContinuesToPlay = true;
		CurrentPlayer.Collect(ballNumber);
	}

	public void NextPlayer() {
		if (currentPlayerContinuesToPlay) {
			currentPlayerContinuesToPlay = false;
			Debug.Log(CurrentPlayer.Name + " continues to play");
			return;
		}
		pv.RPC("setTurn", RpcTarget.All);
		Debug.Log(OtherPlayer.Name + " will play");
		var aux = CurrentPlayer;
		CurrentPlayer = OtherPlayer;
		OtherPlayer = aux;
	}

	public void EndMatch() {
		Pool_Player winner = null;
		if (CurrentPlayer.Points > OtherPlayer.Points)
			winner = CurrentPlayer; 
		else if (CurrentPlayer.Points < OtherPlayer.Points)
			winner = OtherPlayer;

		var msg = "Game Over\n";

		if (winner != null)
			msg += string.Format("The winner is '{0}'", winner.Name);
		else
			msg += "It was a draw!";

		var text = winnerMessage.GetComponentInChildren<UnityEngine.UI.Text>();
		text.text = msg;
		winnerMessage.GetComponent<Canvas>().enabled = true;
	}

	[PunRPC]
	void setTurn()
	{
		Debug.Log("setting turn");
		//whoseTurn = 1;
		//Photon.Realtime.Player _nextplayer = _CurrentPlayer.GetNext();
		//whoseTurn = _nextplayer.ActorNumber; 
		
	}
}
