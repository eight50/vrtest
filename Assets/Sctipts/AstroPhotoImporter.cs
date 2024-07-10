using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


public class TextureImageImporter : AssetPostprocessor {

    static readonly string[] targetExtensions = { ".jpg" };

    void OnPreprocessAsset() {
        bool isValidExtension = false;
        foreach (var extension in targetExtensions) {
            if (Path.GetExtension(assetPath).ToLower().Equals(extension)) {
                // もし～Pixel以上の画像なら、
                // if()
                isValidExtension = true;
                break;
            }
        }

        if (!isValidExtension) {
            return;
        }

        // assetPath:AssetPostprocessorのメンバで、インポートされたアセットと、インポートしようとしているアセットに対してのパス
        var imagePath = assetPath;

        // ImageSharpで画像分割
        using (var image = Image.Load(imagePath)) {
            var size = image.Size();
            var x = 0;
            var y = 0;
            var width = size.Width / 2;
            var height = size.Height / 2;

            // Clone(deep copy)して、Cropping(矩形切り取り）する
            using (var separatedImage = image.Clone(i => i.Crop(new Rectangle(x, y, width, height)))) {
                separatedImage.Save(Path.Combine(Path.GetDirectoryName(imagePath), "separatedImage.png"));
            }
        }


        // 分割した画像をインポート

        // CreateTexture2D();
    }

    void OnPreprocessTexture() {
        bool isValidExtension = false;
        foreach (var extension in targetExtensions) {
            if (Path.GetExtension(assetPath).ToLower().Equals(extension)) {
                isValidExtension = true;
                break;
            }
        }

        if (!isValidExtension) {
            return;
        }

        var importer = assetImporter as TextureImporter;
        importer.textureType = TextureImporterType.Default;
        importer.npotScale = TextureImporterNPOTScale.None;
        importer.alphaIsTransparency = true;
        importer.mipmapEnabled = false;
        // importer.lightmap = false;
        // importer.normalmap = false;
        // importer.linearTexture = false;
        importer.wrapMode = TextureWrapMode.Repeat;
        // importer.generateCubemap = TextureImporterGenerateCubemap.None;
    }

    void kari() {


    }


    // 分割した画像からテクスチャ2Dを生成
    Texture2D CreateTexture2D(string fileName) {
        var assetsPath = Path.Combine(Application.streamingAssetsPath, fileName);
        var texture = new Texture2D(2, 2, TextureFormat.RGB24, true);
        var bytes = UnityEngine.Windows.File.ReadAllBytes(assetsPath);

        texture.LoadImage(bytes);
        return texture;
    }

}

