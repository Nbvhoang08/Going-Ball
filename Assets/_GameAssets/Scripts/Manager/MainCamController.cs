using Cinemachine;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;
public class MainCamController : MonoBehaviour
{
    [SerializeField] Skybox _skybox;
    public CinemachineFreeLook cinemachineFreeLook;
    public Transform CinemachineTransform { get { return cinemachineFreeLook.transform; } }
    private Quaternion spawnRotation;
    int _indexSkybox = 0;
    [HideInInspector] public ShowButton _lateButton;
    


    private void OnEnable()
    {
        GameEvent.OnChangeSkybox += ChangeSkybox;

    }
    private void OnDisable()
    {
        GameEvent.OnChangeSkybox -= ChangeSkybox;

    }

    private void Start()
    {
        spawnRotation = transform.rotation;
        InitialWorld();
    }

    
   

    #region CamController
    public void StartFollow(Transform target)
    {
        cinemachineFreeLook.LookAt = target;
        cinemachineFreeLook.Follow = target;
        cinemachineFreeLook.PreviousStateIsValid = false;
        cinemachineFreeLook.m_YAxis.Value = 0.5f;
        //cinemachineFreeLook.m_XAxis.Value = rotate;
    }

    public void SetTransform(Vector3 spawn,float rotate)
    {
        cinemachineFreeLook.ForceCameraPosition(spawn, spawnRotation);
        cinemachineFreeLook.m_YAxis.Value = 0.5f;
        cinemachineFreeLook.m_XAxis.Value = rotate;
        //cinemachineFreeLook.LookAt = null;
        //cinemachineFreeLook.Follow = null;
        Debug.Log($"{cinemachineFreeLook.m_XAxis.Value} + {rotate}");
        cinemachineFreeLook.PreviousStateIsValid = false;

    }
    public void ChangeYAxis(float value)
    {
        cinemachineFreeLook.m_YAxis.m_InputAxisValue = value;
    }
    public void ChangeXAxis(float value)
    {
        cinemachineFreeLook.m_XAxis.m_InputAxisValue = value;
    }
    public void ChangeValueAxis(float value)
    {
        cinemachineFreeLook.m_YAxis.Value = value;

    }
    public void ChangeValueXAxis(float value)
    {
        cinemachineFreeLook.m_XAxis.Value = value;

    }
    #endregion

    #region Skybox
    public void ChangeSkyBox(int indexSkybox, Vector3 position, Quaternion rotation)
    {
        SkyBoxInfors skyboxInfor = GameManager.Instance.GameData.GetWorldInfor(indexSkybox);
        _skybox.material = skyboxInfor.material;
        RenderSettings.fogColor = skyboxInfor.fogColor;
        cinemachineFreeLook.transform.position = position;
        cinemachineFreeLook.transform.rotation = rotation;
    }
    public void ChangeSkyBox(SkyBoxInfors skyboxInfor)
    {
        _skybox.material = skyboxInfor.material;
        RenderSettings.fogColor = skyboxInfor.fogColor;
    }
    public void ChangeTheme(SkyBoxTheme theme) 
    {
        switch (theme) 
        {
            case SkyBoxTheme.current:
                break;
            case SkyBoxTheme.Beach:
                _indexSkybox = 0;
                ChangeSkyBox(GameManager.Instance.GameData.skyBoxInfors[_indexSkybox]);
                GameManager.Instance.GameData.SelectWorld(_indexSkybox);
                break;
            case SkyBoxTheme.Candy:
                _indexSkybox = 3;
                ChangeSkyBox(GameManager.Instance.GameData.skyBoxInfors[_indexSkybox]);
                GameManager.Instance.GameData.SelectWorld(_indexSkybox);
                break;
            case SkyBoxTheme.Galaxy:
                _indexSkybox = 6;
                ChangeSkyBox(GameManager.Instance.GameData.skyBoxInfors[_indexSkybox]);
                GameManager.Instance.GameData.SelectWorld(_indexSkybox);
                break;
        }

    }
    public void InitialWorld()
    {
        _indexSkybox = GameManager.Instance.GameData.IndexWorldUsing();
        SkyBoxInfors skyboxInfor = GameManager.Instance.GameData.GetWorldInfor(_indexSkybox);
        ChangeSkyBox(skyboxInfor);
    }
    public void ChangeSkybox(Material skyMat)
    {
        if (_skybox != null)
        {
            _skybox.material = skyMat;

        }
    }

    public void ShowPreviousWorld(ref int indexSkybox, bool backToUsing = false, UnityAction callback = null)
    {
        _indexSkybox--;

        if (_indexSkybox < 0)
            _indexSkybox = GameManager.Instance.GameData.TotalSkyBox - 1;
        indexSkybox = _indexSkybox;
        _lateButton = ShowButton.Previous;
        if (backToUsing)
            ChangeSkyBox(GameManager.Instance.GameData.GetWorldInfor(_indexSkybox));
        else
            ChangeSkyBox(GameManager.Instance.GameData.GetWorldInfor(_indexSkybox % GameManager.Instance.GameData.TotalSkyBox));
    }

    public void ShowNextWorld(ref int indexSkybox, bool backToUsing = false, UnityAction callback = null)
    {
        _indexSkybox++;

        if (_indexSkybox > GameManager.Instance.GameData.TotalSkyBox - 1)
            _indexSkybox = 0;


        indexSkybox = _indexSkybox;
        _lateButton = ShowButton.next;

        if (backToUsing)
            ChangeSkyBox(GameManager.Instance.GameData.GetWorldInfor(_indexSkybox));
        else
            ChangeSkyBox(GameManager.Instance.GameData.GetWorldInfor(_indexSkybox % GameManager.Instance.GameData.TotalSkyBox));
    }

    


    #endregion
}

