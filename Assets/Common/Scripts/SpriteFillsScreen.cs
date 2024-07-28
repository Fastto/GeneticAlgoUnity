using UnityEngine;

namespace Common.Scripts
{
    public class SpriteFillsScreen : MonoBehaviour
    {
        public void Fill()
        {
            // Get the Sprite Renderer component
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("Sprite Renderer component not found!");
                return;
            }

            // Get the main camera
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main camera not found!");
                return;
            }

            // Get the size of the sprite
            float spriteWidth = spriteRenderer.sprite.bounds.size.x;
            float spriteHeight = spriteRenderer.sprite.bounds.size.y;

            // Get the orthographic size of the camera
            float cameraHeight = mainCamera.orthographicSize * 2;
            float cameraWidth = cameraHeight * mainCamera.aspect;

            // Calculate the scale needed to fill the viewport
            Vector3 scale = new Vector3(cameraWidth / spriteWidth, cameraHeight / spriteHeight, 1);

            // Apply the scale to the GameObject
            transform.localScale = scale;
        }
    }
}
