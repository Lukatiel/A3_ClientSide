//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class ReplayRecord : MonoBehaviour
//{

//    private List<ActionReplayRecord> ActionReplayManager = new List<ActionReplayRecord>();
//    private bool isInReplayMode;
//    private GameSystemManager gm;

//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.R))
//        {
//            if (isInReplayMode)
//            {
//                SetTransform(0);
//                foreach (GameObject go in gm.gridSpaceButtonArray)
//                {
//                    go.GetComponent<Button>().interactable = true;
//                }
//            }
//            else
//            {
//                SetTransform(ActionReplayManager.Count - 1);

//                foreach (GameObject go in gm.gridSpaceButtonArray)
//                {
//                    go.GetComponent<Button>().interactable = false;
//                }
//            }
//        }
//    }

//    private void FixedUpdate()
//    {
//        if (isInReplayMode == false)
//        {
//            ActionReplayManager.Add(new ActionReplayRecord { position = transform.position });
//        }

//    }

//    private void SetTransform(int index)
//    {
//        ActionReplayRecord replayManager = ActionReplayManager[index];

//        transform.position = replayManager.position;
//    }
//}

//public class ActionReplayRecord
//{
//    public Vector3 position;
    
//}

