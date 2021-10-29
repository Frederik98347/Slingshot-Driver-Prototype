using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event System.Action OnGameReset;

    [SerializeField] TMPro.TextMeshProUGUI scoreValueText;
    [SerializeField] Button resetButton;
    [SerializeField] Button quitButton;
    [SerializeField] AudioSource audioSource;

    [SerializeField] Transform startLocation;
    public Vector3 StartPos => startLocation.position;

    [SerializeField] TMPro.TextMeshProUGUI dragText;
    [SerializeField] GameObject driver;

    public GameObject Driver => driver;

    private int _score = 0;

    public void AddScore(int amount)
    {
        _score += amount;
        UpdateScore();
    }

    private void Awake()
    {
        Instance = this;

        if (resetButton != null)
        {
            var resetEvent = new Button.ButtonClickedEvent();
            resetEvent.AddListener(() =>
            {
                driver.transform.position = startLocation.position;
                driver.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

                var rb = driver.GetComponent<Rigidbody>();
                rb.Sleep();

                OnGameReset?.Invoke();
                ResetScore();
            });

            resetButton.onClick = resetEvent;
        }

        if (quitButton != null)
        {
            var quitEvent = new Button.ButtonClickedEvent();
            quitEvent.AddListener(() =>
            {
                Application.Quit();
            });
            quitButton.onClick = quitEvent;
        }

        ResetScore();
    }

    void UpdateScore()
    {
        scoreValueText.text = _score.ToString();
    }

    void ResetScore()
    {
        _score = 0;
        scoreValueText.text = _score.ToString();
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void DragEventText(string text)
    {
        dragText.text = text;
    }
}