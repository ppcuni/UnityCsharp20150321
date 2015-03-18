using OcCommon;
using System;
using UnityCommon.Controls;
using UnityCommon.Dialog;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// テキスト入力ダイアログ
/// </summary>
[DataContextType(typeof(InputDialogModel))] 
public partial class InputDialog : ResultDialogBase<string>
{
    [SerializeField]
    private Text _title = null;

    [SerializeField]
    private InputField _input = null;

    [SerializeField]
    private CommonView _submitButton = null;

    [BindingProperty("Title")]
    public string Title { set { _title.text = value; } }

    [BindingProperty("Input")]
    public string Input { get { return _input.text; } set { _input.text = value; } }

    [BindingProperty("CanSubmit")]
    public bool CanSubmit { set { _submitButton.GetComponent<Button>().interactable = value; } }

    /// <summary>
    /// View の初期化処理。
    /// </summary>
    protected override void Initialized()
    {
        _input.onEndEdit.AddListener(_ => OnPropertyChanged("Input"));
        _submitButton.Click.Subscribe(() => Close(Input));
    }
}

