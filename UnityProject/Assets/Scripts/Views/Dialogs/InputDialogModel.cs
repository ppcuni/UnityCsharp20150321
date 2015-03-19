using System.ComponentModel;

public class InputDialogModel : BindableBase
{
    /// <summary>
    /// タイトル。
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// 入力テキスト。
    /// </summary>
    public string Input
    {
        get { return _input; }
        set
        {
            if(SetProperty(ref _input, value, "Input"))
            {
                CanSubmit = _input.Length > 0;
            }
        }
    }
    public string _input = "";

    /// <summary>
    /// 決定ボタンを押せるかどうか。
    /// </summary>
    public bool CanSubmit { get { return _canSubmit; } private set { SetProperty(ref _canSubmit, value, "CanSubmit"); } }
    public bool _canSubmit;

    public InputDialogModel(string title)
    {
        Title = title;
    }
}
