using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    FirstPersonController firstPersonController;

    private IInteractable lastInteractable = null;
    private IInteractable currentInteractable;
    private IInteractable currentHideout = null;
    //debug
    private bool lastInHideout;
    private IInteractable lastDebugInteractable;

    bool inHideout = false;
    Vector3 playerLastPostion;

    [SerializeField] float playerReach = 2f;
    [SerializeField] LayerMask interactableLayer;

    void Start()
    {
        firstPersonController = GetComponent<FirstPersonController>();
    }

    void Update()
    {

        if (inHideout && currentHideout != null)
        {
            currentInteractable = currentHideout;
        }
        else
        {
            currentInteractable = GetInteractableObject();
        }


        //debugowanie, pokazuje w konsoli czy gracz jest w kryjówce i jaki obiekt jest aktualnie interaktywny
        if (inHideout != lastInHideout || currentInteractable != lastDebugInteractable)
        {

            lastInHideout = inHideout;
            lastDebugInteractable = currentInteractable;
        }

        //sprawdzanie czy obiekt nie zosta³ zniszczony, jeœli tak to ustawiamy go na null, ¿eby nie próbowaæ siê z nim dalej komunikowaæ
        if (lastInteractable != null && lastInteractable.GetTransform() == null)
        {
            lastInteractable = null;
        }

        //debug
        if (currentInteractable != lastDebugInteractable)
        {
            if(currentInteractable != null) Debug.Log(currentInteractable.GetInteractText(transform));

            lastDebugInteractable = currentInteractable;
        }
        //debug
        if(currentInteractable == null && lastDebugInteractable != null)
        {
            Debug.Log("No interactable");
        }

        //podœwietlanie obiektu, jeœli jest inny ni¿ ostatnio podœwietlany
        if (lastInteractable != currentInteractable)
        {
            if (lastInteractable != null)
                ToggleHighlight(lastInteractable, false);

            if (currentInteractable != null)
                ToggleHighlight(currentInteractable, true);

            lastInteractable = currentInteractable;
        }

        if (currentInteractable == null && lastInteractable != null)
        {
            ToggleHighlight(lastInteractable, false);
            lastInteractable = null;
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inHideout && currentHideout != null)
            {
                Debug.Log("INTERACT pressed on: " + currentInteractable);
                InteractWith(currentHideout);
            }
            else if (currentInteractable != null)
            {
                Debug.Log("INTERACT pressed on: " + currentInteractable);
                InteractWith(currentInteractable);
            }
        }

        //OldVersionNoRaycast();

    }

    public IInteractable GetInteractableObject()
    {
        //blokada interakcji gdy w kryjówce
        if (inHideout) return null;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Debugowanie Raycasta, pokazuje czerwon¹ liniê w kierunku, w którym jest rzutowany promieñ
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * playerReach, Color.red);

        //Raycast do dok³adnego trafiania w obiekty
        if (Physics.Raycast(ray, out hit, playerReach, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                return interactable;
            }
        }

        //OverlapSphere dla wiêkszych przedmiotów (np. drzwi, kryjówki), które mog¹ byæ trudne do trafienia Raycastem
        Collider[] colliders = Physics.OverlapSphere(transform.position, playerReach, interactableLayer);

        IInteractable bestInteractable = null;
        float bestDot = 0.5f; //ni¿sza wartoœæ zwiêksza szansa na interakcjê

        IInteractable bestPickup = null;
        float bestPickupDot = 0.3f;//ustawiæ w zakresie [0.1, 0,5], im ni¿ej tym ³atwiej podnosiæ przedmioty

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent(out IInteractable interactable))
            {
                //nie wyœwietlamy kryjówki, gdy ju¿ jesteœmy w kryjówce
                if (interactable == currentHideout) continue;

                Vector3 dir = (interactable.GetTransform().position - Camera.main.transform.position).normalized;
                float dot = Vector3.Dot(Camera.main.transform.forward, dir);

                // sprawdzamy czy to nie pickup
                if (interactable.GetInteractableType() == InteractableType.Pickup)
                {
                    float distance = Vector3.Distance(Camera.main.transform.position, interactable.GetTransform().position);
                    float score = dot - (distance * 0.1f); //ustawiæ w zakresie [0.05, 0.2], im wy¿ej tym bardziej preferowane s¹ bliskie przedmioty

                    if (score > bestPickupDot)
                    {
                        bestPickupDot = score;
                        bestPickup = interactable; 
                    }
                }
                else
                {
                    //inne obiekty ni¿ pickup
                    if (dot > bestDot)
                    {
                        bestDot = dot;
                        bestInteractable = interactable;
                    }
                }


                
            }
        }

        //najpierw podnoœ pickupy 
        if(bestPickup != null)
        {
            return bestPickup;
        }

        return bestInteractable;




        //return OldRayCastMethod();

        //return OldOverlapSphereMethod();
    }

    void InteractWith(IInteractable interactable)
    {
        if (interactable.GetInteractableType() == InteractableType.Hideout)
        {
            if (!inHideout)
            {
                // wejœcie do kryjówki
                inHideout = true;
                currentHideout = interactable;

                playerLastPostion = transform.position;

                firstPersonController.SetImmovable();
                gameObject.SetActive(false);

                interactable.Interact(transform);

                transform.Rotate(0, 180, 0);
                gameObject.SetActive(true);
            }
            else
            {
                // wyjœcie z kryjówki
                inHideout = false;
                currentHideout = null;

                gameObject.SetActive(false);
                transform.position = playerLastPostion;
                gameObject.SetActive(true);

                firstPersonController.SetMovable();
            }
        }
        else
        {
            //blokada interakcji gdy w kryjówce
            if (inHideout) return;

            interactable.Interact(transform);

            if (interactable == currentInteractable)
            {
                currentInteractable = null;
                lastInteractable = null;
            }
        }
    }

    //podœwietlanie obiektu
    void ToggleHighlight(IInteractable interactable, bool state)
    {
        if (interactable == null) return;

        Transform t = interactable.GetTransform();
        if (t == null) return;

        var highlight = t.GetComponent<InteractableHighlight>();
        if (highlight != null)
        {
            highlight.SetHighlight(state);
        }
    }

    public bool IsInHideout()
    {
        return inHideout;
    }
    public IInteractable GetCurrentInteractable()
    {
        if (inHideout && currentHideout != null)
            return currentHideout;

        return currentInteractable;
    }

    void ToggleOutline(IInteractable obj, bool enable)
    {
        var outline = obj.GetTransform().GetComponent<Outline>();
        if (outline != null) outline.enabled = enable;
    }

    private IInteractable OldOverlapSphereMethod()
    {
        //Stara wersja bez RayCast, zostawiam na wszelki wypadek
        List<IInteractable> interactables = new List<IInteractable>();
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, playerReach);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                interactables.Add(interactable);
            }
        }

        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactables)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) < Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                {
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }

    private IInteractable OldRayCastMethod()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Debugowanie Raycasta, pokazuje czerwon¹ liniê w kierunku, w którym jest rzutowany promieñ
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * playerReach, Color.red);

        //sprawdza, czy Raycast trafi³ w obiekt na warstwie interactableLayer w zasiêgu playerReach
        if (Physics.Raycast(ray, out hit, playerReach, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                return interactable;
            }
        }

        return null;
    }

    private void OldVersionNoRaycast()
    {
        //Stara wersja bez RayCast, zostawiam na wszelki wypadek
        if (Input.GetKeyDown(KeyCode.E) && !inHideout)
        {
            IInteractable interactable = GetInteractableObject();
            if (interactable != null)
            {
                if (interactable.ToString().Contains("Hideout") && !inHideout)
                {
                    Debug.Log("Interacting with hideout");
                    inHideout = true;
                    playerLastPostion = this.transform.position;
                    Debug.Log("Player last position: " + playerLastPostion);
                    firstPersonController.SetImmovable();
                    this.gameObject.SetActive(false);
                    interactable.Interact(transform);
                    this.gameObject.transform.Rotate(0, 180, 0);
                    this.gameObject.SetActive(true);
                }
                else interactable.Interact(transform);

            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && inHideout)
        {
            if (playerLastPostion != null)
            {
                Debug.Log("Leaving hideout");
                inHideout = false;
                this.gameObject.SetActive(false);
                gameObject.transform.position = playerLastPostion;
                this.gameObject.SetActive(true);
                firstPersonController.SetMovable();
            }
            else Debug.LogError("Player last position is null, cant't leave hideout!");
        }
    }
}
