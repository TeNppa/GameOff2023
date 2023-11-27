using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents Instance { get; private set; }

    [field: Header("Overworld Ambience SFX")]
    [field: SerializeField]
    public EventReference OverworldAmbience { get; private set; }

    [field: Header("Cave Ambience SFX")]
    [field: SerializeField]
    public EventReference CaveAmbience { get; private set; }

    [field: Header("Music")]
    [field: SerializeField]
    public EventReference Music { get; private set; }

    [field: Header("Test SFX")]
    [field: SerializeField]
    public EventReference Test { get; private set; }

    [field: Header("Footsteps SFX")]
    [field: SerializeField]
    public EventReference Footsteps { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found another instance of FMODEvents. Destroying this one.");
        }

        Instance = this;
    }
}