using IteratorTasks;
using Login;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityCommon.Controls;
using UnityCommon.Dialog;
using UnityCommon.Game;
using UnityEngine;

/// <summary>
/// 起点になるView
/// </summary>
public class MainView : MonoBehaviour
{
    /// <summary>
    /// 開始ボタン。
    /// </summary>
    [SerializeField]
    private CommonView _startButton = null;

    /// <summary>
    /// ダイアログ管理クラス。
    /// </summary>
    private DialogManager _dialogManager = null;

    /// <summary>
    /// アップデートを毎フレーム呼び出すやつ。
    /// タスクスケジューラを登録しておく。
    /// </summary>
    private IUpdatable _updatables = null;

    /// <summary>
    /// ログインモデル。
    /// </summary>
    private LoginModel _loginModel = new LoginModel();

    private void Update()
    {
        // 中で実行中のTaskクラスのイテレータが進む。
        _updatables.Update();
    }

    /// <summary>
    /// 本アプリのエントリーポイント。
    /// </summary>
    private void Awake()
    {
        // ダイアログマネージャの作成。
        _dialogManager = new DialogManager(this.gameObject, this.gameObject, CancellationToken.None);

        // タスクアップデーターの初期化。
        var updatables = new UpdatableCollection();
        updatables.Add(new TaskRunner(OnUnhandledException));
        _updatables = updatables;

        // スタートボタンのクリック購読。
        _startButton.Click.Subscribe(() => Login());
    }

    /// <summary>
    /// エラーのハンドリング。
    /// </summary>
    /// <param name="task"></param>
    private void OnUnhandledException(IEnumerable<Exception> exceptions)
    {
        Task.Run(ShowErrorDialogIterator(exceptions));
    }

    /// <summary>
    /// エラーダイアログを表示。
    /// 複数のエラーがあるときは順番に表示する。
    /// </summary>
    /// <param name="exceptions"></param>
    /// <returns></returns>
    private IEnumerator ShowErrorDialogIterator(IEnumerable<Exception> exceptions)
    {
        foreach (var e in exceptions)
        {
            Debug.LogError(e.ToString());
            yield return _dialogManager.ShowAsync("エラー", e.Message);
        }
    }

    /// <summary>
    /// ログインを開始する。
    /// </summary>
    private void Login()
    {
        var handler = new LoginProgressHandler(_dialogManager);
        _loginModel.LoginAsync(handler.GetListener());
    }
}