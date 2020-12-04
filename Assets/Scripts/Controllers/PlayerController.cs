using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    public Interactable focus;

    public LayerMask movementMask;
    Camera cam;
    PlayerMotor motor;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject()){
            return;
        }

        // On Mouse Left Click We will move the player to the clicked location
        if(Input.GetMouseButtonDown(0)){
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If the ray hits
            if(Physics.Raycast(ray, out hit, 100, movementMask)){
                //Debug.Log("We hit "+ hit.collider.name + " "+ hit.point);
                motor.MoveToPoint(hit.point);

                // Stop focusing any objetcs
                RemoveFocus();
            }
        }

        // On Mouse Right Click We will highlight any interactable object
        if(Input.GetMouseButtonDown(1)){
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100)){
                //Check if we hit an interactable, if we did then set it as our focus
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if(interactable != null){
                    SetFocus(interactable);
                }
            }
        }
    }

    void SetFocus(Interactable newFocus){

        if(newFocus != focus){
            if(focus != null)
                focus.OnDefocused();
            focus = newFocus;
            motor.FollowTarget(newFocus);
        }

        newFocus.OnFocused(transform);
    }

    void RemoveFocus(){
        if(focus != null)
            focus.OnDefocused();
        focus = null;
        motor.StopFollowingTarget();
    }
}
