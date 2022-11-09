using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NRKernal;

namespace WorldPosition {
    public class GetPosition : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject bodyLockMenu;
        public Text positionText;
        public Text distanceText;

        private static float currentExitDistance;

        void Start()
        {
            currentExitDistance = 10000000.0f;
        }

        // Update is called once per frame
        void Update()
        {
            // Debug.Log((Vector3)NRKernal.NRFrame.HeadPose.position);
            bodyLockMenu.transform.position = NRFrame.HeadPose.position;
            bodyLockMenu.transform.rotation = Camera.main.transform.rotation;
        }

        float CalculateAngle(Vector3 v1, Vector3 v2, Vector3 n) 
        {
            Vector3 l1 = v1-n;
            Vector3 l2 = v2-n;
            Vector3 l1_2D = new Vector3(l1.x, l1.z, 0);
            Vector3 l2_2D = new Vector3(l2.x, l2.z, 0);
            Vector3 cross = Vector3.Cross(l1_2D, l2_2D.normalized);
            float sign = (cross.z > 0)? -1:1;
            float angle = Vector3.Angle(l1_2D, l2_2D);
            return angle * sign;
        }
    }


}
