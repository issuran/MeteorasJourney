using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessParallaxScroll : MonoBehaviour {

    //   float scrollSpeed = -2f;
    //   Vector2 startPos;

    //// Use this for initialization
    //void Start () {
    //       startPos = transform.position;
    //}

    //// Update is called once per frame
    //void Update () {
    //       float newPos = Mathf.Repeat(Time.time * scrollSpeed, 20);
    //       transform.position = startPos + Vector2.right * newPos;
    //}
    private float lenght, startPos;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + lenght) startPos += lenght;
        else if (temp < startPos - lenght) startPos -= lenght;
    }
}
