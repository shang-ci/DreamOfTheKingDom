using UnityEngine;
using UnityEngine.Playables;

public class IntroController : MonoBehaviour
{
    public PlayableDirector director;

    public ObjectEventSO loadMenuEvent;

    private void Awake() 
    {
        director = GetComponent<PlayableDirector>();
        director.stopped += OnPlayableDirectorStopped;
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && director.state == PlayState.Playing)
        {
            director.Stop();
        } 
    }

    private void OnPlayableDirectorStopped(PlayableDirector playableDirector)
    {
        loadMenuEvent.RaiseEvent(null, this);
    }
}
