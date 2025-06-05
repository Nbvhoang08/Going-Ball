using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallSkin : MonoBehaviour
{
    public Transform models;
    public Transform trailContainer;
    [SerializeField] float _distance = 4f;
    [SerializeField] Vector3 _direction;
    private List<Material> _materials = new List<Material>();
    private List<GameObject> CantDissovel = new List<GameObject>();
    [SerializeField] private float _edgeWidthMax = 0.05f;
    private List<Tween> _amountTweens = new List<Tween>();
    private List<Tween> _widthTweens = new List<Tween>();
    private BallRotation _ballRotation;
    Tween TShowBall;
    ShowButton _lateButton;
    int _indexBall;
    int _indexTrail;
    [SerializeField] float _timeTransition = 0.3f;

    [Header("Movement Settings")]
    public Transform target;
    public float moveDistance = 2f;
    public float moveDuration = 1f;
    private bool _facingForward = true;
    private Vector3 _startLocalPos;
    private Tween _moveTween;
    public float rotateDuration = 0.1f;
    public float rotateSpeed = 1f;
    private Coroutine _rollingCoroutine;


    private void Awake()
    {
        if (target == null)
            target = transform;

        _startLocalPos = target.localPosition;
        _ballRotation = GetComponent<BallRotation>();
    }
    public void Start()
    {
        InitBallSkin(GameManager.Instance.GameData.BallUsing());
        InitTrail(GameManager.Instance.GameData.TrailUsing());
    }

    #region Skin

    public void InitBallSkin(TypeBall typeModel)
    {
        BallModel[] ballModels = models.GetComponentsInChildren<BallModel>();
        foreach (BallModel ball in ballModels)
        {
            ball.ReturnToPool();
        }

        BallModel newBallModel = Pool.Instance.Ball(typeModel);
        newBallModel.transform.SetParent(models);
        newBallModel.transform.localPosition = Vector3.zero;
        newBallModel.transform.localRotation = Quaternion.identity;
        newBallModel.OnSetModel();
        _materials.Clear();
        CantDissovel.Clear();
        Renderer[] renderers = newBallModel.renderers;
        foreach (Renderer r in renderers)
        {
            foreach (Material mat in r.materials)
            {
                if (mat.HasProperty("_DissolveAmount") && mat.HasProperty("_EdgeWidth"))
                {
                    _materials.Add(mat);
                }
                else 
                {
                    CantDissovel.Add(r.gameObject);
                }
            }
        }
    }

    public void Appear()
    {
        foreach (var obj in CantDissovel)
        {
            obj.SetActive(true);
        }
        PlayDissolve(1f, 0f, 1);
    }

    public void Disappear()
    {
        foreach (var obj in CantDissovel)
        {
            obj.SetActive(false);
        }
        PlayDissolve(0f, 1f, 0.4f);
    }

    private void PlayDissolve(float from, float to, float _duration)
    {
        
        foreach (var t in _amountTweens) t?.Kill();
        foreach (var t in _widthTweens) t?.Kill();
        _amountTweens.Clear();
        _widthTweens.Clear();
        if (_materials.Count <= 0) return;
        foreach (var mat in _materials)
        {
            mat.SetFloat("_DissolveAmount", from);

            var widthUp = DOTween.To(
                () => mat.GetFloat("_EdgeWidth"),
                x => mat.SetFloat("_EdgeWidth", x),
                _edgeWidthMax,
                _duration * 0.5f
            ).SetEase(Ease.OutSine);
            _widthTweens.Add(widthUp);

            var dissolve = DOTween.To(
                () => mat.GetFloat("_DissolveAmount"),
                x => mat.SetFloat("_DissolveAmount", x),
                to,
                _duration
            ).SetEase(Ease.InOutSine)
             .OnComplete(() =>
             {
                 var widthDown = DOTween.To(
                     () => mat.GetFloat("_EdgeWidth"),
                     x => mat.SetFloat("_EdgeWidth", x),
                     0f,
                     _duration * 0.25f
                 ).SetEase(Ease.InSine);
                 _widthTweens.Add(widthDown);
             });

            _amountTweens.Add(dissolve);
        }
        
    }

    public void ShowAllSkin(Vector3 direction)
    {
        _direction = direction;
        _lateButton = ShowButton.Using;
        _indexBall = GameManager.Instance.GameData.BallIndexUsing;
    }
    public void HideShowSkin(UnityAction callback)
    {
        int temp = 0;
        if (_indexBall.Equals((int)GameManager.Instance.GameData.BallUsing()))
        {
            if (TShowBall != null)
                TShowBall.Kill(true);
            FinishHide(callback);
        }
        else if (_lateButton.Equals(ShowButton.Previous))
        {
            ShowNextSkin(ref temp, true, () => { FinishHide(callback); });
        }
        else if (_lateButton.Equals(ShowButton.next))
        {
            ShowPreviousSkin(ref temp, true, () => { FinishHide(callback); });
        }
    }
    void FinishHide(UnityAction callback)
    {
        models.localPosition = Vector3.zero;
        models.GetChild(0).localPosition = Vector3.zero;
        models.GetChild(0).localRotation = Quaternion.identity;
        //callback();
    }

    public void ShowPreviousSkin(ref int indexBall, bool backToUsing = false, UnityAction callback = null)
    {
        _indexBall--;

        if (_indexBall < 0)
            _indexBall = GameManager.Instance.GameData.totalBall - 1;
        Debug.Log($"Index Ball : {_indexBall} { GameManager.Instance.GameData.totalBall - 1}");
        indexBall = _indexBall;
        _lateButton = ShowButton.Previous;
        if (TShowBall != null)
            TShowBall.Kill(true);
        Vector3 TargetPosition = models.position - _distance * _direction;
        Vector3 ballPosition = models.GetChild(0).position + _distance * _direction;

        BallModel newBallModel;
        if (backToUsing)
            newBallModel = Pool.Instance.Ball(GameManager.Instance.GameData.BallUsing());
        else
            newBallModel = Pool.Instance.Ball((TypeBall)(_indexBall % GameManager.Instance.GameData.totalBall));

        newBallModel.transform.SetParent(models);
        newBallModel.transform.position = ballPosition;
        newBallModel.transform.localRotation = Quaternion.identity;
        newBallModel.OnSetModel();
        TShowBall = models.DOMove(TargetPosition, _timeTransition).OnComplete(() =>
        {
            models.transform.GetChild(0).GetComponent<BallModel>().ReturnToPool();

            if (callback != null)
                callback();
        });
    }
    public void ShowNextSkin(ref int indexBall, bool backToUsing = false, UnityAction callback = null)
    {
        _indexBall++;
        if (_indexBall >= GameManager.Instance.GameData.totalBall-1)
            _indexBall = 0;

        indexBall = _indexBall;
        _lateButton = ShowButton.next;

        if (TShowBall != null)
            TShowBall.Kill(true);
        Vector3 TargetPosition = models.position + _distance * _direction;
        Vector3 ballPosition = models.GetChild(0).position - _distance * _direction;
        BallModel newBallModel;

        if (backToUsing)
            newBallModel = Pool.Instance.Ball(GameManager.Instance.GameData.BallUsing());
        else
            newBallModel = Pool.Instance.Ball((TypeBall)(_indexBall % GameManager.Instance.GameData.totalBall));

        newBallModel.transform.SetParent(models);
        newBallModel.transform.position = ballPosition;
        newBallModel.transform.localRotation = Quaternion.identity;
        newBallModel.OnSetModel();
        TShowBall = models.DOMove(TargetPosition, _timeTransition).OnComplete(() =>
        {
            models.transform.GetChild(0).GetComponent<BallModel>().ReturnToPool();

            if (callback != null)
                callback();
        });
    }

    #endregion


    #region Trail
    public void InitTrail(TrailType type)
    {
        TrailFX[] trails = trailContainer.GetComponentsInChildren<TrailFX>();
        foreach (TrailFX trail in trails)
        {
            trail.ReturnToPool();
        }
        TrailFX newTrail = Pool.Instance.GetTrailFX(type);
        newTrail.transform.SetParent(trailContainer);
        newTrail.PrepareToUse();
        newTrail.transform.localRotation = Quaternion.identity;
    }
    public void SetUsingTrail(TrailType type)
    {
        InitTrail(type);
    }
    public void ShowPreviousTrails(ref int indexTrail, bool backToUsing = false, UnityAction callback = null)
    {
        _indexTrail--;
       
        if (_indexTrail < 0)
            _indexTrail = GameManager.Instance.GameData.totalBall - 1;

        indexTrail = _indexTrail;
        _lateButton = ShowButton.Previous;
        TrailFX newTrail;
        if (backToUsing)
            newTrail = Pool.Instance.GetTrailFX(GameManager.Instance.GameData.TrailUsing());
        else
            newTrail = Pool.Instance.GetTrailFX((TrailType)(_indexTrail % GameManager.Instance.GameData.TotalTrail));

        newTrail.transform.SetParent(trailContainer);
        newTrail.transform.localRotation = Quaternion.identity;
        trailContainer.transform.GetChild(0).GetComponent<TrailFX>().ReturnToPool();
        newTrail.PrepareToUse();
    }

    public void ShowNextTrails(ref int indexTrail, bool backToUsing = false, UnityAction callback = null)
    {
        _indexTrail++;

        if (_indexTrail >= GameManager.Instance.GameData.TotalTrail)
            _indexTrail = 0;

        indexTrail = _indexTrail;
        _lateButton = ShowButton.Previous;
      
        TrailFX newTrail;
        if (backToUsing)
            newTrail = Pool.Instance.GetTrailFX(GameManager.Instance.GameData.TrailUsing());
        else
            newTrail = Pool.Instance.GetTrailFX((TrailType)(_indexTrail % GameManager.Instance.GameData.TotalTrail));

      
        newTrail.transform.SetParent(trailContainer);
        newTrail.transform.localRotation = Quaternion.identity;
        trailContainer.transform.GetChild(0).GetComponent<TrailFX>().ReturnToPool();
        newTrail.PrepareToUse();
    }
    public void ShowAllTrails()
    {
        _lateButton = ShowButton.Using;
        _indexTrail = GameManager.Instance.GameData.TrailIndexUsing;
        StartMoving();
    }
    public void HideShowTrail(UnityAction callback)
    {
        int temp = 0;
        Stop();
        if (_indexBall.Equals((int)GameManager.Instance.GameData.BallUsing()))
        {
            FinishHide(callback);
        }
        else if (_lateButton.Equals(ShowButton.Previous))
        {
            ShowNextTrails(ref temp, true, () => { FinishHide(callback); });
        }
        else if (_lateButton.Equals(ShowButton.next))
        {
            ShowPreviousTrails(ref temp, true, () => { FinishHide(callback); });
        }
    }
    public void StartMoving()
    {
        Stop();
        _facingForward = true;
        target.localRotation = Quaternion.Euler(0f, 0f, 0f);

        _moveTween = target.DOLocalMoveZ(_startLocalPos.z + moveDistance, moveDuration)
            .SetEase(Ease.InOutSine)
            .From(_startLocalPos.z - moveDistance)
            .SetLoops(-1, LoopType.Yoyo)
            .OnStepComplete(() =>
            {
                _facingForward = !_facingForward;
                float yRot = _facingForward ? 0f : 180f;
                target.DOLocalRotate(new Vector3(0f, yRot, 0f), rotateDuration, RotateMode.Fast);
            });
 
        if (_rollingCoroutine != null) StopCoroutine(_rollingCoroutine);
        _rollingCoroutine = StartCoroutine(BallRollingCoroutine());
    }

    public void Stop()
    {
        if (_moveTween != null && _moveTween.IsActive())
        {
            _moveTween.Kill();
        }
        if (_rollingCoroutine != null)
            StopCoroutine(_rollingCoroutine);
        target.localPosition = _startLocalPos;
        target.localRotation = Quaternion.identity;
        _ballRotation.ResetRotation();
    }
    
    private IEnumerator BallRollingCoroutine()
    {
        while (true)
        {
            _ballRotation.RollingInnerBall(500f * Time.smoothDeltaTime);
            yield return null; 
        }
    }

    #endregion
}





