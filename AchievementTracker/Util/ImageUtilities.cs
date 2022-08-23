using OWML.Common;
using System;
using System.IO;
using UnityEngine;
using Logger = AchievementTracker.Util.Logger;

namespace AchievementTracker.Utit
{
    static class ImageUtilities
    {
        public static Texture2D GetTexture(IModBehaviour mod, string filename)
        {
            try
            {
                var path = mod.ModHelper.Manifest.ModFolderPath + filename;

                byte[] data = null;
                if(File.Exists(path + ".png"))
                {
                    data = File.ReadAllBytes(path + ".png");
                }
                else if (File.Exists(path + ".jpg"))
                {
                    data = File.ReadAllBytes(path + ".jpg");
                }
                else
                {
                    Logger.Log($"Couldn't find jpg or png for {filename}.");
                    return null;
                }

                var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                texture.LoadImage(data);
                return texture;
            }
            catch(Exception)
            {
                return null;
            }
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
            for (var i = 0; i < pixels.Length; i++)
            {
                var grey = pixels[i].grayscale;
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