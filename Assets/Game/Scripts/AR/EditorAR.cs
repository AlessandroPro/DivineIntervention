using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorAR : MonoBehaviour
{
    public LayerMask blockMask;

    // Start is called before the first frame update
    void Start()
    {
        if(!DeviceManager.Instance.devMode)
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, blockMask))
            {
                Block block = hit.collider.gameObject.GetComponent<Block>();
                if (block)
                {
                    if(block.canMove)
                    {
                        block.toggleMove();
                    }
                }
            }
        }
    }
}
