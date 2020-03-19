using UnityEngine;
using System.Collections;
using System.IO;

public class RTTSave : MonoBehaviour {
	
	private static int PIC_WIDTH = 256;
	public RenderTexture RTTTex;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		if(RTTTex == null)
			return ;
		
//		int width 	= RTTTex.width;
//		int height 	= RTTTex.height;
//		Texture2D tex2d = new Texture2D(width,height,TextureFormat.RGB24,false);
//		RenderTexture.active	= RTTTex;
//		tex2d.ReadPixels(new Rect(0,0,width,height),0,0);
//		tex2d.Apply();
//		
//		byte[] b = tex2d.EncodeToPNG();	
//		//Destroy(tex2d);
//		
//		File.WriteAllBytes(Application.dataPath + "1.jpg",b);
	}
}
