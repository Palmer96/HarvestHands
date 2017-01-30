using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{
    public enum Resource
    {
        Wood,
        Water,
        Rock,
        Dirt
    }
    [System.Serializable]
    public class ResourceRequired
    {
        public Resource resource;
        public int numRequired;
        public int numHave;
    }

    public GameObject builtVersion;
    public ResourceRequired[] resources;
    

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int count = 0;
        for (int i = 0; i < resources.Length; i++)
        {
            if (resources[i].numHave >= resources[i].numRequired)
            {
                count++;
            }
        }
        if (resources.Length == count)
        {
            Build();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        for (int i = 0; i < resources.Length; i++)
        {
            if (col.transform.CompareTag(resources[i].resource.ToString()))
            {
                if (resources[i].numRequired > resources[i].numHave)
                {
                    resources[i].numHave++;
                    Destroy(col.gameObject);

                }
            }
        }
    }

    void Build()
    {
        Instantiate(builtVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
