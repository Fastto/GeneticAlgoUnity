using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Evolution.Scripts
{
    public class LightingManager : Singleton<LightingManager>
    {
        [SerializeField] protected SpriteRenderer m_SpriteRenderer;
        [SerializeField] protected Image m_Image;
        [SerializeField] protected List<Sprite> m_Options;
        
        protected Texture2D m_Texture;
        
        private void Start()
        {
            SetLighting(0);
        }

        private void SetLighting(int id)
        {
            if (m_Options == null || m_Options.Count < (id + 1))
            {
                Debug.LogError("Lighting Option is not set!");
                return;
            }
            
            m_Image.sprite = m_Options[id];
            m_Texture = m_Image.mainTexture as Texture2D;
            
            m_SpriteRenderer.sprite = m_Options[id];
            m_SpriteRenderer.GetComponent<SpriteFillsScreen>().Fill();
        }
        
        public float GetIntensivityForPoint(Vector3 pos)
        {
            if (m_Texture == null)
            {
                Debug.LogError("Texture is not set!");
                return 0;
            }
            
            Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
            float textureX = (screenPos.x * m_Texture.width ) / Camera.main.pixelWidth;
            float textureY = (screenPos.y * m_Texture.height ) / Camera.main.pixelHeight;
        
            if (textureX < 0 || textureY < 0 || textureX >= m_Texture.width || textureY >= m_Texture.height)
                return 0;

            return  m_Texture.GetPixel((int)textureX, (int)textureY).a;
        }
    }
}
