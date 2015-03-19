using IteratorTasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TaskInteraction;
using UnityCommon.Dialog;
using UnityCommon.Game;
using UnityEngine;
using P = Login.Progress;

/// <summary>
/// <see cref="Login.Progress.LoginProgressBase"/>をハンドルするクラス。
/// </summary>
public class LoginProgressHandler
{
    /// <summary>
    /// ダイアログ管理クラス。
    /// </summary>
    private readonly DialogManager _dialogManager;

    public LoginProgressHandler(DialogManager dialogManager)
    {
        _dialogManager = dialogManager;
    }

    /// <summary>
    /// <see cref="Login.Progress.LoginProgressBase"/>のリスナーを取得。
    /// </summary>
    /// <returns></returns>
    public Listener<P.LoginProgressBase> GetListener()
    {
        var listener = new Listener<P.LoginProgressBase>();
        listener.SetAsyncHandler<P.UserPolicyConfirm>(OnUserPolicyConfirm);
        listener.SetAsyncHandler<P.InputUserName>(OnInputUserName);
        listener.SetAsyncHandler<P.LoggedIn>(OnLoggedIn);
        return listener;
    }

    /// <summary>
    /// 利用規約同意待ち。
    /// </summary>
    /// <param name="arg1"></param>
    /// <returns></returns>
    private IEnumerator OnUserPolicyConfirm(P.UserPolicyConfirm arg1)
    {
        var accept = _dialogManager.ShowAsync("利用規約同意",
            arg1.Policy, CommonDialogMode.OkCancel, buttonLabels: new []{"同意しない", "同意する"});
        yield return accept;
        // Viewからの返答をセット。
        arg1.Response = accept.Result;
    }

    /// <summary>
    /// ユーザー名入力待ち。
    /// </summary>
    /// <param name="arg1"></param>
    /// <returns></returns>
    private IEnumerator OnInputUserName(P.InputUserName arg1)
    {
        var inputDialogModel = new InputDialogModel("ユーザー名入力");
        var inputUserName = _dialogManager.ShowAsync<string>(inputDialogModel);
        yield return inputUserName;
        arg1.Response = inputUserName.Result;
    }

    /// <summary>
    /// ログイン成功。ユーザーデータ表示。
    /// </summary>
    /// <param name="arg1"></param>
    /// <returns></returns>
    private IEnumerator OnLoggedIn(P.LoggedIn arg1)
    {
        yield return _dialogManager.ShowAsync("ログインしました。",
            string.Format("ユーザー名:{0}\n所持金:{1} Gold", arg1.UserData.UserName, arg1.UserData.Gold));
    }
}