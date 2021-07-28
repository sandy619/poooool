using UnityEngine;
using System.Collections;
using Photon.Realtime;
using Photon.Pun;

namespace GameStates {
	public class WaitingForNextTurnState : AbstractGameObjectState {
		private PoolGameController gameController;
		private GameObject cue;
		private GameObject cueBall;
		private GameObject redBalls;
		private GameObject mainCamera;

		private Vector3 cameraOffset;
		private Vector3 cueOffset;
		private Quaternion cameraRotation;
		private Quaternion cueRotation;
		private PhotonView PV;

		public WaitingForNextTurnState(MonoBehaviour parent) : base(parent) {
			gameController = (PoolGameController)parent;

			cue = gameController.cue;
			cueBall = gameController.cueBall;
			redBalls = gameController.redBalls;
			mainCamera = gameController.mainCamera;

			cameraOffset = cueBall.transform.position - mainCamera.transform.position;
			cameraRotation = mainCamera.transform.rotation;
			cueOffset = cueBall.transform.position - cue.transform.position;
			cueRotation = cue.transform.rotation;
			
		}


		public override void FixedUpdate() {
			//Debug.Log(redBalls.GetComponentsInChildren<Transform>().Length);
			if (redBalls.GetComponentsInChildren<Transform>().Length == 1) {
				gameController.EndMatch();
			} else {
				var cueBallBody = cueBall.GetComponent<Rigidbody>();
				if (!(cueBallBody.IsSleeping() || cueBallBody.velocity == Vector3.zero))
					return;
				
				foreach (var rigidbody in redBalls.GetComponentsInChildren<Rigidbody>()) {
					if (!(rigidbody.IsSleeping() || rigidbody.velocity == Vector3.zero))
						return;
				}
				//gameController.pv.RPC("setTurn", RpcTarget.All);
				gameController.NextPlayer();
				// If all balls are sleeping, time for the next turn
				// This is kinda hacky but gets the job done
				gameController.currentState = new WaitingForStrikeState(gameController);
			}
		}

		public override void LateUpdate() {
			mainCamera.transform.position = cueBall.transform.position - cameraOffset;
			mainCamera.transform.rotation = cameraRotation;
			
			cue.transform.position = cueBall.transform.position - cueOffset;
			cue.transform.rotation = cueRotation;
		}

		//[PunRPC]
		//void setTurn()
  //      {
		//	Debug.Log("setting turn");
		//	//Photon.Realtime.Player _curplayer = gameController._CurrentPlayer.GetNext();
		//	//gameController.whoseTurn = _curplayer.ActorNumber;
		//}
	}
}