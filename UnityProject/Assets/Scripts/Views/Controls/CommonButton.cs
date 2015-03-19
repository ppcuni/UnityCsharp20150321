using UnityEngine;
using UnityEngine.UI;
using UnityCommon.Controls;

/// <summary>
/// クリックとかのイベントをとれる要素
/// </summary>
public class CommonButton : CommonView
{
    [SerializeField]
    private Button _button = null;

    protected override void Initialized()
    {
        _button.onClick.AddListener(() => _Click.Invoke(this));
    }
}
