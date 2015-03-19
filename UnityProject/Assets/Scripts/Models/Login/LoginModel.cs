using IteratorTasks;
using System.Collections;
using TaskInteraction;

namespace Login
{
    using System;
    using P = Login.Progress;

    /// <summary>
    /// ログインシーケンスの進行管理クラス。
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// ログイン用のAPIセット。
        /// </summary>
        private ILoginApi _api = new LoginApiDummy();

        /// <summary>
        /// 端末識別子。
        /// </summary>
        private const string _deviceId = "XXXX-YYYY-ZZZZ-9999";

        /// <summary>
        /// ログインを開始する。
        /// </summary>
        /// <param name="listener"><see cref="P.LoginProgressBase"/>をハンドルするためのリスナー。</param>
        /// <returns></returns>
        public Task LoginAsync(Listener<P.LoginProgressBase> listener)
        {
            var ch = CreateChannel();
            ch.AddManualListener(listener);

            return ch.ExecuteAsync(CancellationToken.None);
        }

        /// <summary>
        /// View-Model間の相互連絡通路となるChannelを作成。
        /// </summary>
        /// <returns></returns>
        private Channel<P.LoginProgressBase> CreateChannel()
        {
            return new Channel<P.LoginProgressBase>(RunAsync);
        }

        /// <summary>
        /// ログインのイテレータをタスク化して実行。
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private Task RunAsync(Channel<P.LoginProgressBase> channel, CancellationToken ct)
        {
            return Task.Run(RunIterator(channel, ct));
        }

        /// <summary>
        /// ログイン処理の実態となるイテレータ。
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        private IEnumerator RunIterator(Channel<P.LoginProgressBase> channel, CancellationToken ct)
        {
            // ログインAPIを呼ぶ。
            var login = _api.LoginAsync(_deviceId, ct);

            // ログインAPIの応答を待つ。
            yield return login;

            // 利用規約に同意してないなら利用規約同意シーケンスに。
            if(login.Result.UserPolicy != null)
            {
                // 利用規約同意待ち
                var confirm = new P.UserPolicyConfirm(login.Result.UserPolicy);
                yield return channel.SendAsync(confirm);

                // 同意しない場合はエラー出してイテレータ停止。
                if (!confirm.Response)
                    throw new Exception("利用規約に同意しないとプレイ出来ません。");

                yield return _api.AcceptUserPolicyAsync(ct);
            }

            // 未登録なら登録シーケンスに。
            if(!login.Result.IsRegistered)
            {
                // ユーザー名入力待ち
                var userName = new P.InputUserName();
                yield return channel.SendAsync(userName);

                var register = _api.RegisterAsync(userName.Response, ct);
                yield return register;

                // ユーザー名が不正ならエラー文言出してイテレータ停止。（本来なら名前入力からやり直しがベスト）
                if(register.Result.IsInvalidUserName)
                    throw new Exception("利用不可能なユーザー名です。");
            }

            // ユーザー情報取得
            var getUserData = _api.GetUserDataAsync(ct);
            yield return getUserData;

            // ログイン完了を通知。
            yield return channel.SendAsync(new P.LoggedIn(getUserData.Result));
        }
    }
}
