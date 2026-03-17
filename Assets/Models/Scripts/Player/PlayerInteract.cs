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
            Debug.Log("InHideout: " + inHideout);
            Debug.Log("CurrentInteractable: " + currentInteractable);
            Debug.Log("CurrentHideout: " + currentHideout);

            lastInHideout = inHideout;
            lastDebugInteractable = currentInteractable;
        }

        //sprawdzanie czy obiekt nie zosta³ zniszczony, jeœli tak to ustawiamy go na null, ¿eby nie próbowaæ siê z nim dalej komunikowaæ
        if (lastInteractable != null && lastInteractable.GetTransform() == null)
        {
            lastInteractable = null;
        }

        //debug
        if (currentInteractable != null)
        {
            Debug.Log(currentInteractable.GetInteractText(transform));
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
                InteractWith(currentHideout);
            }
            else if (currentInteractable != null)
            {
                InteractWith(currentInteractable);
            }
        }

        //OldVersionNoRaycast();

    }

    public IInteractable GetInteractableObject()
    {

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        //Raycast do dok³adnego trafiania w obiekty
        if (Physics.Raycast(ray, out hit, playerReach, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                return interactable;
            }
        }

        //OverlapSphere dla wiêkszych przedmiotów (np. drzwi, kryjówki), które mog¹ byæ trudne do trafienia Raycastem
        Collider[] colliders = Physics.OverlapSphere(transform.position, playerReach);

        IInteractable bestInteractable = null;
        float bestDot = 0.6f;

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent(out IInteractable interactable))
            {
                // sprawdzamy czy to nie pickup
                if (interactable.GetInteractableType() == InteractableType.Pickup)
                    continue;

                Vector3 dir = (interactable.GetTransform().position - Camera.main.transform.position).normalized;
                float dot = Vector3.Dot(Camera.main.transform.forward, dir);

                if (dot > bestDot)
                {
                    bestDot = dot;
                    bestInteractable = interactable;
                }
            }
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
