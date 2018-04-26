using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Drive - FluffyHedgehog 2018 ====================//
//
// Author:		Dudley 
// Purpose:		This handles the HUD Canvas and items.
// Namespace:	FH
//
//===============================================================================//



namespace FH
{
    public class AircraftHUD : MonoBehaviour
    {
        [SerializeField]
        private Canvas HUD;
        [SerializeField]
        private Image fuelIndicator;
        [SerializeField]
        private Image bombStock;


        private void Start()
        {
            HUD = GetComponentInChildren<Canvas>();
            if(!HUD)
            {
                Debug.Log("ERROR: Canvas HUD cannot be found in Aircraft objects children");
                return;
            }

            var images = HUD.GetComponentsInChildren<Image>();
            foreach(var image in images)
            {
                if(image.name == "Fuel Indicator")
                {
                    fuelIndicator = image;
                }

                if(image.name == "Bomb Stock")
                {
                    bombStock = image;
                }
            }

            if(!fuelIndicator)
            {
                Debug.Log("ERROR: Fuel Indictor image cannot be found in HUD Canvas children");
            }

            if(!bombStock)
            {
                Debug.Log("ERROR: Bomb Stock image cannot be found in HUD Canvas children");
            }


            FuelIndicatorOff();
            BombStockOff();
        }

        public void FuelIndicatorOn()
        {
            fuelIndicator.color = new Color(fuelIndicator.color.r, fuelIndicator.color.g, fuelIndicator.color.b, 1f);
        }

        public void FuelIndicatorOff()
        {
            //Debug.Log("Fuel indicator off");
            //fuelIndicator.color = new Color(fuelIndicator.color.r, fuelIndicator.color.g, fuelIndicator.color.b, 0.25f);
        }


        public void BombStockOn()
        {
            //bombStock.color = new Color(bombStock.color.r, bombStock.color.g, bombStock.color.b, 1f);
        }

        public void BombStockOff()
        {
         //   Debug.Log("bomb indicator off");
            //bombStock.color = new Color(bombStock.color.r, bombStock.color.g, bombStock.color.b, 0.25f);
        }




    }
}
