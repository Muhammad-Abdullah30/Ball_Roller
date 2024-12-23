using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    public TextMeshProUGUI headingText; // Assign your heading text in the inspector
    public Slider loadingBar; // Assign your loading bar in the inspector
    public GameObject mainMenuPanel; // Assign your main menu panel in the inspector
    public TextMeshProUGUI headingText2;
    [Header("Input Fields")]
    public TMP_InputField nameInput;
    public Button okButton;

    public TMP_InputField EnterAge;
    public Button confirmButton;


    [Header("Levels Panel")]
    public GameObject levelsPanel;
    public TextMeshProUGUI playerName;
    void Start()
    {
        mainMenuPanel.SetActive(false); // Hide the main menu panel initially
        PlayIntroSequence();
    }


    private void Update()
    {
        if(string.IsNullOrEmpty(nameInput.text))
        {
            okButton.interactable = false;
            Debug.Log("Check it");
        }
        else
        {
            okButton.interactable = true;
        }

        if (string.IsNullOrEmpty(EnterAge.text))
        {
            confirmButton.interactable = false;
        }
        else
        {
            confirmButton.interactable = true;
        }
    }


    void PlayIntroSequence()
    {
        // Step 1: Animate the heading text
        headingText.text = "Ball Rolling";
        headingText.color = new Color(headingText.color.r, headingText.color.g, headingText.color.b, 0); // Set initial alpha to 0
        headingText.DOFade(1, 1.5f) // Fade in the heading
                  .SetEase(Ease.InOutQuad);

        // Step 2: Animate the loading bar after heading
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1.5f); // Wait for heading animation
        sequence.Append(loadingBar.DOValue(1, 2f) // Fill loading bar over 2 seconds
                                   .SetEase(Ease.InOutQuad));
        sequence.AppendCallback(() =>
        {
            // Step 3: Show the main menu panel after loading
            mainMenuPanel.SetActive(true);
            mainMenuPanel.transform.localScale = Vector3.zero; // Set initial scale to 0
            mainMenuPanel.transform.DOScale(1, 0.5f) // Scale up the panel
                                  .SetEase(Ease.OutBounce);
        });
    }

    public void OkButton()
    {
        playerName.text = nameInput.text;
        nameInput.gameObject.SetActive(false);
        okButton.gameObject.SetActive(false);
        EnterAge.gameObject.SetActive(true);
        confirmButton.gameObject.SetActive(true);
        headingText2.text = "Enter the Age".ToString();
        Debug.Log("Button Clicked ");

    }

    public void ConfirmButton()
    {
        levelsPanel.gameObject.SetActive(true);
    }

    public void LevelSelect(int levelSelect) 
    {
        SceneManager.LoadScene(levelSelect);
    }



}
