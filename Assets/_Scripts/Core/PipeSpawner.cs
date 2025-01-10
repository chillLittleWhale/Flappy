using System;
using System.Collections;
using System.Collections.Generic;
using AjaxNguyen.Core.ObjectPooling;
using UnityEngine;

namespace AjaxNguyen.Core
{
    public class PipeSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject pipePrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private List<GameObject> pipeList = new ();

        void Start()
        {
            Level.Instance.OnStateChange += Level_OnStateChange;
        }

        private void Level_OnStateChange(object sender, GameState e)
        {
            bool active = e == GameState.Playing;

            pipeList.ForEach(pipe =>{
                HorizontalMove horizontalMove = pipe.TryGetComponent(out HorizontalMove move) ? move : null;
                horizontalMove?.SetActive(active);

                if(!horizontalMove) Debug.LogWarning("Pipe does not have HorizontalMove component");
            });
        }

        public void CreatePipes(float gapSize, float gapYPos, bool isVeticalMove, float xSpeed = 0f, float ySpeed = 0f, float maxHeight = 0f, float minHeight = 0f)
        {
            var newPipe = PoolManager.Instance.GetFromPool(pipePrefab.gameObject);
            newPipe.transform.position = spawnPoint.position;

            var pipesController = newPipe.GetComponent<PipesController>();
            pipesController.SetupPipe(gapSize, gapYPos, isVeticalMove, ySpeed, maxHeight, minHeight);

            newPipe.GetComponent<HorizontalMove>().SetSpeed(xSpeed);
            newPipe.GetComponent<HorizontalMove>().SetActive(true);

            newPipe.transform.parent = transform;

            pipeList.Add(newPipe);
        }
    }
}
