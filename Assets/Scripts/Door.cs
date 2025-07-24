using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject DoorModel;

    bool inTrigger;
    int CountInTrigger;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger && Input.GetButtonDown("Interact"))
        {
            DoorModel.SetActive(false);
            gamemanager.instance.InteractButton.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IOpen open = other.GetComponent<IOpen>();

        if (open != null)
        {
            CountInTrigger++;

            if (other.CompareTag("Player"))
            {
                gamemanager.instance.InteractButton.SetActive(true);
                inTrigger = true;
            }
            else
            {
                DoorModel.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
            return;

        IOpen open = other.GetComponent<IOpen>();

        if (open != null)
        {
            CountInTrigger--;

            if (CountInTrigger <= 0)
            {
                DoorModel.SetActive(true);
                CountInTrigger = 0;
            }

            DoorModel.SetActive(true);
            inTrigger = false;
            gamemanager.instance.InteractButton.SetActive(false);
        }
    }
}
