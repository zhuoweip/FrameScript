//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Collections;
using UnityEngine;

public abstract class SMTransition : MonoBehaviour {

    protected SMTransitionState state = SMTransitionState.Out;

    public SMTransitionState CurrentTransitionState {
        get {
            return state;
        }
    }

    public bool timeScaleIndependent = true;
    public bool loadAsync = false;
#if !UNITY_3_5
    // Prefetching needs Unity 4 therefore we hide it in Unity 3.5
    public bool prefetchLevel = false;
#endif
    /// <summary>
    /// The id of the screen that is being loaded.
    /// </summary>
    [HideInInspector]
    public string screenId;

    void Start() {
        if (Time.timeScale <= 0f && !timeScaleIndependent) {
            Debug.LogWarning("Time.timeScale is set to 0 and you have not enabled 'Time Scale Independent' at the transition prefab. " +
                             "Therefore the transition animation would never start to play. Please either check the 'Time Scale Independent' checkbox" +
                             "at the transition prefab or set Time.timeScale to a positive value before changing the level.", this);
            return; // do not do anything in this case.
        }

#if !UNITY_3_5
        if (prefetchLevel && !loadAsync) {
            Debug.LogWarning("You can only prefetch the level when using asynchronous loading. " +
                "Please either uncheck the 'Prefetch Level' checkbox on your level transition prefab or check the " +
                "'Load Async' checkbox. Note, that asynchronous loading (and therefore level prefetching) requires a Unity Pro license.", this);
            return; // don't do anything in this case.
        }
#endif
        StartCoroutine(DoTransition());
    }

    public Action HoldAction
    {
        set { holdAction = value; }
        get { return holdAction; }
    }

    public Action InAction
    {
        set { inAction = value; }
        get { return inAction; }
    }

    private Action holdAction;
    private Action inAction;

	protected virtual IEnumerator DoTransition() {
		DontDestroyOnLoad(gameObject);
        if (inAction != null)
            inAction();

		state = SMTransitionState.Out;
		Prepare();
		float time = 0;
		
		while(Process(time)) {
			time += DeltaTime;
			yield return 0;
		}

        if (holdAction != null)
            holdAction();

		state = SMTransitionState.In;
		Prepare();
		time = 0;
		while(Process(time)) {
			time += DeltaTime;
			yield return 0;
		}

		yield return 0;

		Destroy(gameObject);
	}
	
	
	protected virtual void Prepare() {}
	
	protected abstract bool Process(float elapsedTime);

	protected virtual float DeltaTime {
		get {
			if (timeScaleIndependent) {
				return SMRealTimeHelper.deltaTime;
			}
			else {
				return Time.deltaTime;
			}
		}
	}
}
