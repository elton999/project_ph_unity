using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    public Transform[] bgs;
    public float[] parallaxVelocity;
    public float smooth;
    public Transform cam;

    private Vector3 previewCam;

	void Start () {
        previewCam = cam.position;
	}
	
	void Update () {
		for (int i = 0; i < bgs.Length; i++)
        {
            float parallax = (previewCam.x - cam.position.x) * parallaxVelocity[i];
            float targetPosX = bgs[i].position.x - parallax;
            Vector3 targetPos = new Vector3(targetPosX, bgs[i].position.y, bgs[i].position.z);

            bgs[i].position = Vector3.Lerp(bgs[i].position, targetPos, smooth * Time.deltaTime);
        }

        previewCam = cam.position;
	}
}
