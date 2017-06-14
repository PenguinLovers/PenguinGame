using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using live2d;
using live2d.framework;

[ExecuteInEditMode]
public class MikuModel : MonoBehaviour {
    public TextAsset mocFile;
    public TextAsset physicsFile;
    public Texture2D[] textures;

    private Live2DModelUnity live2DModel;
    private EyeBlinkMotion eyeBlink = new EyeBlinkMotion();
    private L2DTargetPoint dragMgr = new L2DTargetPoint();
    private L2DPhysics physics;
    private Matrix4x4 live2DCanvasPos;
    
	// Use this for initialization
	void Start () {
        Live2D.init();
        load();
	}

    void load()
    {
        live2DModel = Live2DModelUnity.loadModel(mocFile.bytes);

        int len = textures.Length;
        for (int i = 0; i < len; ++i)
        {
            live2DModel.setTexture(i, textures[i]);
        }

        float modelWidth = live2DModel.getCanvasWidth();
        live2DCanvasPos = Matrix4x4.Ortho(0, modelWidth, modelWidth, 0, -50.0f, 50.0f);
        if(physicsFile != null)
        {
            physics = L2DPhysics.load(physicsFile.bytes);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if(live2DModel == null)
        {
            load();
        }
        live2DModel.setMatrix(transform.localToWorldMatrix * live2DCanvasPos);
        if (!Application.isPlaying)
        {
            live2DModel.update();
            return;
        }

        var pos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            //
        }
        else if (Input.GetMouseButton(0))
        {
            dragMgr.Set(pos.x / Screen.width * 2 - 1, pos.y / Screen.height * 2 - 1);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            dragMgr.Set(0, 0);
        }

        dragMgr.update();
        live2DModel.setParamFloat("PARAM_ANGLE_X", dragMgr.getX() * 50);
        live2DModel.setParamFloat("PARAM_ANGLE_Y", dragMgr.getY() * 50);

        live2DModel.setParamFloat("PARAM_BODY_ANGLE_X", dragMgr.getX() * 10);

        live2DModel.setParamFloat("PARAM_EYE_BALL_X", dragMgr.getX()*100);
        live2DModel.setParamFloat("PARAM_EYE_BALL_Y", dragMgr.getY()*100);

        double timeSec = UtSystem.getUserTimeMSec() / 1000.0;
        double t = timeSec * 2 * Math.PI;
        live2DModel.setParamFloat("PARAM_BREATH", (float)(0.5f + 0.5f * Math.Sin(t / 3.0)));

        eyeBlink.setParam(live2DModel);

        if (physics != null)
        {
            physics.updateParam(live2DModel);
        }

        live2DModel.update();
    }


    // Model drawing command
    private void OnRenderObject()
    {
        if (live2DModel == null)
        {
            load();
        }
        if (live2DModel.getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH_NOW)
        {
            live2DModel.draw();
        }
    }
}
