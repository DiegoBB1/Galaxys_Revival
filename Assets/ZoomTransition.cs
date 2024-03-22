using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoomTransition : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    //zoomInTime, zoomOutTime, and fov values may be different depending on the scene.
    //By default, each scene before runtime is set to 0 FOV to prepare for the transition effect.
    [SerializeField] private float zoomInTime = 1;
    [SerializeField] private float zoomOutTime = 1;
    [SerializeField] private float fov = 5;
    [SerializeField] private Player player;
    [SerializeField] private GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
                Debug.Log("Zoom out called in start");

        zoomOut();
    }

    //Only update on main game scene or when player initialized.
    void Update(){
        if(player != null){
            transform.position = player.transform.position + new Vector3(0, 0, -10);
        }
    }

    //zoomIn uses a coroutine to gradually change the fov from the current fov to 0. This is called before transitioning into a new scene.
    //After the zoom in effect finishes, the next scene is loaded.
    public void zoomIn(string sceneName){
        StartCoroutine(zoomInRoutine());
        IEnumerator zoomInRoutine(){
            float timer = 0;
            while (timer < zoomInTime){
                float t = timer/zoomInTime;
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 0, t);
                timer += Time.deltaTime;

                yield return null;
            }
            mainCamera.fieldOfView = 0;
            SceneManager.LoadScene(sceneName);
        }
    }

    //zoomOut uses a coroutine to gradually change the fov from 0 to the intended fov. 
    //This is called when a new scene is loaded to zoom out the camera and create a somewhat smooth transition.
    public void zoomOut(){
        StartCoroutine(zoomOutRoutine());
        IEnumerator zoomOutRoutine(){
            float timer = 0;
            while (timer < zoomOutTime){
                float t = timer/zoomOutTime;
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, fov, t);
                timer += Time.deltaTime;

                yield return null;
            }
            mainCamera.fieldOfView = fov;

            //Could add fade in effect for UI, for now it appears after zoomOutTime seconds
            if(UI != null){
                UI.SetActive(true);
            }

            if(SceneManager.GetActiveScene().name == "MainScene"){
                Spawner spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
                spawner.spawnEnemies();
            }
        }
    }

    public void swapCanvas(GameObject sourceCanvas, GameObject destCanvas, Camera zoomCamera){
        Debug.Log("Swap Canvas Started");
        StartCoroutine(swapRoutine());

        IEnumerator swapRoutine(){
            float timer = 0;
            while (timer < zoomInTime){
                float t = timer/zoomInTime;
                zoomCamera.fieldOfView = Mathf.Lerp(zoomCamera.fieldOfView, 0, t);
                timer += Time.deltaTime;

                yield return null;
            }
            zoomCamera.fieldOfView = 0;
            sourceCanvas.SetActive(false);
            destCanvas.SetActive(true);

            timer = 0;
            while (timer < zoomOutTime){
                float t = timer/zoomOutTime;
                zoomCamera.fieldOfView = Mathf.Lerp(zoomCamera.fieldOfView, fov, t);
                timer += Time.deltaTime;

                yield return null;
            }
            zoomCamera.fieldOfView = fov;

        }
    }
}