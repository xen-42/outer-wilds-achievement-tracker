using OWML.Common;
using System;
using System.IO;
using UnityEngine;

namespace AchievementTracker.Utit
{
    static class ImageUtilities
    {
        public static Texture2D GetTexture(IModBehaviour mod, string filename)
        {
            // Copied from OWML but without the print statement lol
            var path = mod.ModHelper.Manifest.ModFolderPath + filename;
            var data = File.ReadAllBytes(path);
            var texture = new Texture2D(2, 2);
            texture.LoadImage(data);
            return texture;
        }

        public static Sprite MakeSprite(Texture2D texture)
        {
            var rect = new Rect(0, 0, texture.width, texture.height);
            var pivot = new Vector2(texture.width / 2, texture.height / 2);
            return Sprite.Create(texture, rect, pivot);
        }

        public static Texture2D GreyscaleImage(Texture2D image)
        {
            var pixels = image.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                var grey = pixels[i].r * 0.3f + pixels[i].g * 0.59f + pixels[i].b * 0.11f;
                pixels[i].r = grey;
                pixels[i].g = grey;
                pixels[i].b = grey;
            }

            var newImage = new Texture2D(image.width, image.height);
            newImage.SetPixels(pixels);
            newImage.Apply();
            return newImage;
        }
    }
}