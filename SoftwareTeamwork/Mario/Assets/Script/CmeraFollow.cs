using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmeraFollow : MonoBehaviour
{
    public Transform mplayer;

    public float speed;

    public float minPosx;

    public float maxPosx;

    // Start is called before the first frame update
    void Start()
    {
        mplayer = GameObject.Find("PlayerPrefab").GetComponent<Transform>();
        speed = mplayer.position.x- transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        FixCameraPos();

    }

    void FixCameraPos()
    {
        
        if(mplayer.position.x>0&& mplayer.position.x<289)
            transform.position = new Vector3(mplayer.position.x, transform.position.y, transform.position.z);
        
        
        //float realPosX = Mathf.Clamp(transform.position.x, minPosx, maxPosx);
        //transform.position = new Vector3(realPosX, transform.position.y, transform.position.z);
    }
}
