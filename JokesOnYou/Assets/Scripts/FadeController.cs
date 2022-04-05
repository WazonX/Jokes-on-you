using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(CanvasGroup))]
public class FadeController : MonoBehaviour
{
    public float CurrentAlpha => _canvasGroup.alpha;
    public float DesiredAlpha = 1f;

    [SerializeField] CanvasGroup _canvasGroup;

   
    private void Start()
    {
        Assert.IsNotNull(_canvasGroup);
    }

    void Update()
    {
        _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, DesiredAlpha, 2.0f * Time.deltaTime);
    }
}
