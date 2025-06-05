using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

namespace HapigaIAP
{
    [CreateAssetMenu(fileName = "IAPConfigs")]
    public class IAPConfigs : ScriptableObject
    {
        public ProductDetails[] ProductDetailses;
        public ProductDetails GetPDByID(string id)
        {
            return ProductDetailses.FirstOrDefault(x => x.Id == id);
        }

        public void OnRestore()
        {
            //if(GameManager.Instance != null)
            //{
            //    GameManager.Instance.OnBuyNoAds();
            //}
        }
    }

    [System.Serializable]
    public class ProductDetails
    {
        public ProductType productType;
        public string Id;
        [HideInInspector] public string localizedPrice;
        [HideInInspector] public string currencyCode;
    }
}
