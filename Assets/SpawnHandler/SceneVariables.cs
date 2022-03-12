using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneVariables : MonoBehaviour
{
    #region Variables
    [Header("The indices for the three Asteroid types to determine their order in Layer")]
    public int orderInLayerTier1;
    public int orderInLayerTier2;
    public int orderInLayerTier3;

    public int maxOrderValue;
    #endregion






    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /**
     * returns the current order Index, based on the value of "currentTier".
     * Also updates the order Index to only assign it once
     */
    public void newOrder(int currentTier)
    {
        switch (currentTier)
        {
            case 1:
                updateT1Index();
                break;
            case 2:
                updateT2Index();
                break;
            case 3:
                updateT3Index();
                break;

            default:
                break;
        }
    }

    /**
     * Increase orderInLayerTier1 by +1; When the upper Limit is reached, reset the number and start from 1 again
     */
    public void updateT1Index()
    {
        orderInLayerTier1++;

        if (orderInLayerTier1 > maxOrderValue)
        {
            orderInLayerTier1 = 1;
        }
    }

    /**
    * Increase orderInLayerTier2 by +1; When the upper Limit is reached, reset the number and start from 1 again
    */
    public void updateT2Index()
    {
        orderInLayerTier2++;

        if (orderInLayerTier2 > maxOrderValue)
        {
            orderInLayerTier2 = 1;
        }
    }

    /**
    * Increase orderInLayerTier3 by +1; When the upper Limit is reached, reset the number and start from 1 again
    */
    public void updateT3Index()
    {
        orderInLayerTier3++;

        if (orderInLayerTier3 > maxOrderValue)
        {
            orderInLayerTier3 = 1;
        }
    }


    public int getOrderInLayerTier1()
    {
        print("orderInLayerTier1    " + orderInLayerTier1);
        return orderInLayerTier1;
    }

    public int getOrderInLayerTier2()
    {
        print("orderInLayerTier2    " + orderInLayerTier2);
        return orderInLayerTier2;
    }

    public int getOrderInLayerTier3()
    {
        print("orderInLayerTier3    " + orderInLayerTier3);
        return orderInLayerTier3;
    }
}
