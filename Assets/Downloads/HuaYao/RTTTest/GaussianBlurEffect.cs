using UnityEngine;
using System.Collections;

public class GaussianBlurEffect : MonoBehaviour {
	
	private Material mBlurMat;
	public Shader BlurShader;
	
	

	// Use this for initialization
	void Start () {
		mBlurMat	= new Material(BlurShader);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnRenderImage(RenderTexture src,RenderTexture target)
	{
		if(mBlurMat == null)
			return ;
		
//		RenderTexture temp = RenderTexture.GetTemporary(src.width,src.height);
//		
//		for(int i = 1; i < 10; i++)
//		{
//			mBlurMat.SetFloat("_Offset",offset);
//			Graphics.Blit(src,target);
//		}
//		
		float offset = 1.0f / src.width;
		
		//mBlurMat.SetFloat("_Offset",offset);
		//mBlurMat.SetVector("offsets", Vector4(offset, 0.0, 0.0, 0.0));
		//mBlurMat.SetVector("offsets",new Vector4(offset,0.0f,0.0f,0.0f));
		mBlurMat.SetVector("offsets",new Vector4(offset,offset,offset,offset));
		Graphics.Blit(src,target,mBlurMat);
		
		
		
		
	}
}
