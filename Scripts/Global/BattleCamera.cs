using System.Collections;
using UnityEngine;

[System.Serializable]
public class BattleCamera 
{
    [Header("Camera")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Collider2D _mapBounds;
    [SerializeField] [Range(0, 1)] private float _smoothSpeed;

    [Space]
    [Header("Shake")]
    [SerializeField] private AnimationCurve _shakeCurve;
    [SerializeField] private float _shakeDuration;
    private Coroutine _shakeRoutine;
    private MonoBehaviour _source;

    private float _minX, _minY, _maxX, _maxY;
    private float _cameraX, _cameraY;

    private float _cameraOrthographicSize;
    private float _cameraRatio;

    private Transform _from;
    private Transform _to;
    private Vector2 _followPoint;
    private Vector3 _smoothPosition;

    public void Setup(Transform from, Transform to, MonoBehaviour source) 
    {
        _from = from;
        _to = to;
        _source = source;

        _minX = _mapBounds.bounds.min.x;
        _minY = _mapBounds.bounds.min.y;
        _maxX = _mapBounds.bounds.max.x;
        _maxY = _mapBounds.bounds.max.y;

        _cameraOrthographicSize = _camera.orthographicSize;
        _cameraRatio = (_maxX + _cameraOrthographicSize) / 2.55f;
    }

    public void Update() 
    {
        _followPoint = PointBetweenUnits();

        _cameraY = Mathf.Clamp(_followPoint.y, _minY + _cameraOrthographicSize, _maxY - _cameraOrthographicSize);
        _cameraX = Mathf.Clamp(_followPoint.x, _minX + _cameraRatio, _maxX - _cameraRatio);

        _smoothPosition = Vector3.Lerp(_camera.transform.position,
            new Vector3(_cameraX, _cameraY, _camera.transform.position.z), _smoothSpeed);

        _camera.transform.position = _smoothPosition;
    }

    public void Shake() 
    {
        if (_shakeRoutine == null)
            _shakeRoutine = _source.StartCoroutine(Shacking());

        _source.StopCoroutine(_shakeRoutine);
        _shakeRoutine = null;
        _shakeRoutine = _source.StartCoroutine(Shacking());
    }

    private Vector3 PointBetweenUnits()
    {
        var positionX = (_from.position.x + _to.position.x) / 2;
        var positionY = (_from.position.y + _to.position.y) / 2;

        return new Vector3(positionX, positionY, _camera.transform.position.z);
    }

    private IEnumerator Shacking() 
    {
        var startPosition = _camera.transform.position;
        var elapsedTime = 0f;

        while (elapsedTime < _shakeDuration) 
        {
            elapsedTime += Time.deltaTime;

            var strength = _shakeCurve.Evaluate(elapsedTime / _shakeDuration);

            _camera.transform.position = startPosition + Random.insideUnitSphere * strength;

            yield return null;
        }

        _camera.transform.position = startPosition;
    }
}
