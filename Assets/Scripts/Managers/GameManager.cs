using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton Instance 
    private static GameManager _gameManager;

    public static GameManager Instance
    {
        get
        {
            if (!_gameManager)
            {
                _gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (!_gameManager)
                {
                    Debug.LogError("There needs to be one active SceneController script on a GameObject in your scene.");
                }
            }

            return _gameManager;
        }
    }

    [SerializeField] private PlayerData _playerData; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerGoalCameraPanCompleted(Component sender)
    {
        Debug.Log("Start player timer, etc");
    }

    public void OnPlayerCaught(Component sender)
    {
        Debug.Log("Kleptocrat captured. Off to jail with you!");
    }

    public void OnPlayerStoleItem(Component sender)
    {
        Debug.Log("Kleptocrat has the item.");
    }

    public void OnPlayerReachedExit(Component sender)
    {
        Debug.Log("Kleptocrat has reached an extraction point.");
    }
}
