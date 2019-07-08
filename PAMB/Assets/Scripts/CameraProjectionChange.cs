using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CameraProjectionChange : MonoBehaviour
{
	public static CameraProjectionChange Instance;

    public float ProjectionChangeTime = 0.5f;
    public bool ChangeProjection = false;

    
	private IEnumerator MoveCamCo;

	public Animator Anim;
	public Camera MainCamera;

    private bool _changing = false;
    private float _currentT = 0.0f;
	[Range(0,10)]
	public float IntroSpeed = 2;
	private bool currentlyOrthographic;
	private void Awake()
	{
		Instance = this;
	}

	public void SetChangeProjection(OrtoPersType Orthographic)
	{
		_changing = true;
		currentlyOrthographic = Orthographic == OrtoPersType.Orto ? false : true;
		_currentT = 0.0f;
	}

    private void LateUpdate()
    {
        if (!_changing)
        {
            return;
        }
        Matrix4x4 orthoMat, persMat;
        if (currentlyOrthographic)
        {
            orthoMat = MainCamera.projectionMatrix;

            MainCamera.orthographic = false;
            MainCamera.ResetProjectionMatrix();
            persMat = MainCamera.projectionMatrix;
        }
        else
        {
            persMat = MainCamera.projectionMatrix;

            MainCamera.orthographic = true;
            MainCamera.ResetProjectionMatrix();
            orthoMat = MainCamera.projectionMatrix;
        }
        MainCamera.orthographic = currentlyOrthographic;

        _currentT += Time.deltaTime;
		if (_currentT < IntroSpeed)
        {
            if (currentlyOrthographic)
            {
				MainCamera.projectionMatrix = MatrixLerp(orthoMat, persMat, _currentT / IntroSpeed);
				//Debug.Log(MainCamera.projectionMatrix.determinant);
				if (MainCamera.projectionMatrix.determinant < -1f)
                {
                    _currentT = IntroSpeed;
                }
            }
            else
            {
				MainCamera.projectionMatrix = MatrixLerp(persMat, orthoMat, _currentT / IntroSpeed);

				if(MainCamera.projectionMatrix.determinant > -0.005f)
				{
					_currentT = IntroSpeed;
				}
				//Debug.Log(MainCamera.projectionMatrix.determinant);
            }
        }
        else
        {
			if(currentlyOrthographic)
			{
				SetCameraAnim(currentlyOrthographic);
			}
            _changing = false;
            MainCamera.orthographic = !currentlyOrthographic;
            MainCamera.ResetProjectionMatrix();
        }
    }

    private Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float t)
    {
        t = Mathf.Clamp(t, 0.0f, 1.0f);
        var newMatrix = new Matrix4x4();
        newMatrix.SetRow(0, Vector4.Lerp(from.GetRow(0), to.GetRow(0), t));
        newMatrix.SetRow(1, Vector4.Lerp(from.GetRow(1), to.GetRow(1), t));
        newMatrix.SetRow(2, Vector4.Lerp(from.GetRow(2), to.GetRow(2), t));
        newMatrix.SetRow(3, Vector4.Lerp(from.GetRow(3), to.GetRow(3), t));
        return newMatrix;
    }


	public void MoveToNext(float x)
	{
		if(GameManagerScript.Instance.CurrentLevel.y < Rotator.Instance.Levels.Count)
		{
			if (MoveCamCo != null)
            {
                StopCoroutine(MoveCamCo);
            }

            MoveCamCo = MoveCamera(transform.parent.transform.position + (Vector3.right * x), x);
            StartCoroutine(MoveCamCo);
		}
		else
		{
			
		}
	}


	private IEnumerator MoveCamera(Vector3 dest, float x)
	{
		float Timer = 0;
		Vector3 OffsetPos = transform.parent.transform.position;
		bool MoveEnvironment = false;
		while(Timer < 1)
		{
			if(Timer > 0.005f && !MoveEnvironment)
			{
				MoveEnvironment = true;
				Rotator.Instance.GoToNextLevel();
				UIManagerScript.Instance.SetStageCompletetdAnim(false);
				EnvironmentManagerScript.Instance.MoveToNext(x);
			}
			transform.parent.transform.position = Vector3.Lerp(OffsetPos, dest, Timer);
			yield return new WaitForFixedUpdate();
			Timer += Time.fixedDeltaTime * 2f;
		}
        transform.parent.transform.position = dest;
        Anim.enabled = true;
		SetCameraAnim(false);
		MoveCamCo = null;                                       
	}

	public void SetGameStateToStart()
	{
		GameManagerScript.Instance.SetGameStateToStart();
	}


	public void SetCameraAnim(bool v)
	{
		Anim.SetBool("UpDown", v);
	}

	public void SetCameraShakeAnim()
    {
		Anim.SetBool("Shake", true);
    }

    public void SetCamraShake()
	{
		Anim.SetBool("Shake", false);
	}
}