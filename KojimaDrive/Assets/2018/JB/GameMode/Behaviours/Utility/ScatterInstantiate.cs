using UnityEngine;


namespace JB
{
    public class ScatterInstantiate : MonoBehaviour
    {
        [SerializeField] GameObject objectToSpawn = null;
        [SerializeField] uint spawnCount = 10;
        [SerializeField] float spawnRadius = 5;      
        [SerializeField] [ColorUsage(false)] Color gizmoColour = Color.yellow;// Don't allow alpha
        

        void Start()
        {
            SpawnInRadius();
        }


        public void SpawnInRadius()
        {
            if (objectToSpawn == null)
                return;

            for (uint i = 0; i < spawnCount; ++i)
            {
                float randomRadius = Random.Range(0, spawnRadius);// Random radius position
                Vector3 randomPosition = Random.insideUnitSphere * randomRadius;// Random position on sphere
                Transform spawnedTransform = Instantiate(objectToSpawn, transform).transform;// Spawn object and get transform
                spawnedTransform.position = transform.position + randomPosition;// Give random position offset from this transform
                spawnedTransform.rotation = transform.rotation;
            }
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = gizmoColour;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }

    }
}// JB namespace
