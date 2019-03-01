using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
public class MouseD : MonoBehaviour{

        private float distanceToEndPos = 2, speed = 2;
        List<GameObject> objects = new List<GameObject>();
        List <GameObject> desObj = new List<GameObject>();
        List<Vector2> initialPosition = new List<Vector2>();
        List<Vector2> desiredPosition = new List<Vector2>();
        public GameMan _GameMan;
        List<bool> isPlaced = new List<bool>();
        Vector3 mousePos;
        Transform curObj;
        int curIndex;
        bool isDraging = false;
        public Animator animator;
        public ParticleSystem particlem;
        public ParticleSystem endSceneParticle;
        public AudioSource audSource;
        public AudioClip[] correct;
        public AudioClip clik;
        public AudioClip offClik;
      


       
        
        void Start(){
            
                objects.AddRange(GameObject.FindGameObjectsWithTag("DragAndDropObjects"));
                foreach(GameObject g in objects){
                    initialPosition.Add(g.transform.position);
                    isPlaced.Add(false);
                }
                desObj.AddRange(GameObject.FindGameObjectsWithTag("DragAndDropEndPos"));
                foreach(GameObject g in desObj){
                    desiredPosition.Add(g.transform.position);
                }
        }
        void Update(){
            mousePos = GetMousePos();
            if(!isDraging){
                GetRayCast();
            }
            if(isDraging){
                DragAndDrop();
            }
            if(isPlaced.All(x => x)){
                // Debug.Log("Next Level // Updating Event");
            }
        }
        public void GetRayCast(){
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(mousePos.x,mousePos.y),Vector2.one);
            if(hit.collider != null && Input.GetMouseButtonDown(0) && hit.transform.tag == "DragAndDropObjects"){
                for(int i = 0; i<objects.Count;i++){
                    if(hit.collider != null && Input.GetMouseButtonDown(0) && hit.transform.gameObject == objects[i] && !isPlaced[i]){
                        curIndex = i;
                        curObj = objects[i].transform;
                        isDraging = true;
                        audSource.clip= clik;
                        audSource.Play();
                        
                    }
                }
            }
        }
        
        public Vector3 GetMousePos(){
            Vector3 mp = new Vector3();
            mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return new Vector3(mp.x,mp.y,0);
        }
        public void DragAndDrop(){
            if(curObj != null){
                if(Input.GetMouseButton(0)){
                    curObj.position = Vector3.Lerp(curObj.position,mousePos,0.1f);
                   
                    
                }
                if(Input.GetMouseButtonUp(0)){
                    
                    if(Vector2.Distance(mousePos,desiredPosition[curIndex]) <= distanceToEndPos){ 
                        audSource.clip= offClik;
                        audSource.Play();                       
                        StartCoroutine(SetLerpPos(desiredPosition[curIndex],curObj));
                        curObj = null;
                        isPlaced[curIndex] = true;
                        Debug.Log("cur obj: " + curIndex);
                        
                        
                        
                        if(isPlaced.All(x => x)){
                            audSource.clip= correct[Random.Range(0, correct.Length)];
                            audSource.Play();
                            particlem.Play();
                           /*  StartCoroutine(routine: EndParticle ());
                            StartCoroutine(routine: FadeWait ());*/
                            StartCoroutine(routine: EndScene ());
                            
                            Debug.Log("Next Level // Single Event");
                        }

                    }else{
                    audSource.clip= offClik;
                    audSource.Play();
                        StartCoroutine(SetLerpPos(initialPosition[curIndex],curObj));
                        curObj = null;
                    }
                }
            }
        }
        IEnumerator SetLerpPos(Vector3 endPos,Transform t)
            {
        
            float elapsedTime = 0;
        
            while (elapsedTime < speed)
            {
                Vector3 startPos = t.position;
                t.position = Vector3.Lerp(startPos, endPos, (elapsedTime / speed));
                elapsedTime += Time.deltaTime;
                if(elapsedTime>= speed || Vector3.Distance(t.position,endPos) <= 0.001){
                    t.position = endPos;
                    isDraging = false;
                    yield break;
                }else{
                    yield return null;
                }
            }
        }
        private IEnumerator EndScene()
     {
         yield return new WaitForSeconds(.5f);
         endSceneParticle.Play();
 
         yield return new WaitForSeconds(5.5f);
         animator.SetTrigger("Fade_out");
 
         yield return new WaitForSeconds(2f);
         _GameMan.LoadNextScene();
     }
}
        /*  IEnumerator EndScene (){
             yield return new WaitForSeconds(8.0f);
             _GameMan.LoadNextScene();
         }
         IEnumerator EndParticle (){
             yield return new WaitForSeconds(.5f);
             endSceneParticle.Play();
         }
         IEnumerator FadeWait (){
             yield return new WaitForSeconds(6.0f);
             animator.SetTrigger("Fade_out");
         }*/
