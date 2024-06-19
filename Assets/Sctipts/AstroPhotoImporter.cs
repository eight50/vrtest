using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Drawing;

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

        // System.Drawingで画像を読み込み、分割して保存

        //assetPath:AssetPostprocessorの変数で、インポートされたアセットと、インポートしようとしているアセットに対してのパス
        string imagePath = assetPath;
        Bitmap bitmap = new Bitmap(imagePath);



        // 分割した画像をインポート
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
        importer.lightmap = false;
        importer.normalmap = false;
        importer.linearTexture = false;
        importer.wrapMode = TextureWrapMode.Repeat;
        importer.generateCubemap = TextureImporterGenerateCubemap.None;
    }


}
