using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RefManager : MonoBehaviour
{
    public static RefManager Instance;

    public TMP_Text healthTextRef;

    public GameObject wallicon;

    public GameObject particlesystem;


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);
    }
    
}
