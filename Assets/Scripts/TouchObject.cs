using UnityEngine;

public class TouchObject : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        // 터치 입력 (모바일)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 touchPos = Input.GetTouch(0).position;
            CheckTouch(touchPos);
        }
    }
    void CheckTouch(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) // 3D 오브젝트
        {
            if (hit.collider.gameObject == gameObject)
            {

            }
        }
    }
}