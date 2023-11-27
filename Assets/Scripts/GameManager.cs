using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int TotalSkeletonsOnMap;
    public static int SkeletonsKilled;
    // Start is called before the first frame update
    void Awake()
    {
        TotalSkeletonsOnMap = 0;
        SkeletonsKilled = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
