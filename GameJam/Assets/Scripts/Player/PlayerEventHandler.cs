using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class PlayerEventHandler : MonoBehaviour
{
    public UnityEvent OnWalkStart;
    public UnityEvent OnWalkFinished;
    public UnityEvent OnTurn;
    public UnityEvent<int> OnDamage;
    public UnityEvent OnDeath;
    public UnityEvent OnPowerDownEvent;

    private IVelocity velocityComp;
    private Vector2 prevVelocity;


    private bool isWalking;
    private int numberOfContacts=0;
    private int counter=0;
    [SerializeField] float power=50f;
    [SerializeField] float eyesLightIntensity=1f;
    [SerializeField] float backLightIntensity = 0.8f;
    [SerializeField] Light2D Eyeslight;
    [SerializeField] Light2D backlight;
    private float maxPower;
    private GameObject interactableObject;
    void Start()
    {
        Eyeslight.intensity = eyesLightIntensity;
        backlight.intensity = backLightIntensity;
        maxPower = power;
        velocityComp = GetComponent<IVelocity>();
        prevVelocity = Vector2.right;
        if(velocityComp.GetVelocity() == Vector2.zero)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }
       
    }

    void Update()
    {
        if (isWalking)
        {
            Vector2 velocity = velocityComp.GetVelocity();
            if (prevVelocity.x > 0 && velocity.x < 0)
            {
                OnTurn?.Invoke();
            }
            else if (prevVelocity.x < 0 && velocity.x > 0)
            {
                OnTurn?.Invoke();
            }

            if (velocity.x != 0)
                prevVelocity = velocity;
        }

        if (!isWalking && velocityComp.GetVelocity() != Vector2.zero)
        {
            OnWalkStart?.Invoke();
            isWalking = true;
        }
        else if (isWalking && velocityComp.GetVelocity() == Vector2.zero)
        {
            OnWalkFinished?.Invoke();
            isWalking = false;
        }



    }
    public void Interact()
    {
        if (numberOfContacts > 0)
        {
            //interactableObject.GetComponent<IActiveDevice>().onPowerUpEvent.
            Debug.Log("OTWIERAM DZRWI");
            var sw = interactableObject.GetComponent<IActiveDevice>();
            Debug.Log(sw);
            Debug.Log(sw.getMaxPowerLelvel());
            if (sw.getPowerLelvel() < sw.getMaxPowerLelvel())
            {
                interactableObject.GetComponent<IActiveDevice>().Active();
                PowerDown();

            }
        }
        
    }
    private void OnAnimationFinished(string name)
    {
        //if(name == "death")
        //{
        //    Destroy(gameObject);
        //}
    }

    private void PowerDown()
    {
        OnPowerDownEvent?.Invoke();
        power--;
        ScaleLight();

    }
    private void ScaleLight()
    {
        Eyeslight.intensity = eyesLightIntensity * (power / maxPower);
        backlight.intensity = backLightIntensity * (power / maxPower);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check to see if the tag on the collider is equal to Enemy
        //Debug.Log("punkt");
        if (other.tag == "Interactable")
        {
            
            numberOfContacts++;
            interactableObject = other.gameObject;
            interactableObject.GetComponent<IActiveDevice>().OnPlayerEntry?.Invoke();
            
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //Check to see if the tag on the collider is equal to Enemy
        //Debug.Log("punkt");
        if (other.tag == "Interactable")
        {
            interactableObject.GetComponent<IActiveDevice>().OnPlayerLeave?.Invoke();
            numberOfContacts--;
        }

    }
}
