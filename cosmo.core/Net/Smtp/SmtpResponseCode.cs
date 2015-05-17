
namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Enumera los cigos de resultado de un comando Smtp.
   /// </summary>
   public enum SmtpCommandResultCode : int
   {
      /// <summary>Ning佖 cigo devuelto.</summary>
      None = 0,
      /// <summary>211:システムの状態。システムヘルプ準備完了。</summary>
      SystemStatus_OrSystemHelpReply = 211,
      /// <summary>214:ヘルプメッセージ。</summary>
      HelpMessage = 214,
      /// <summary>220:準備完了。</summary>
      ServiceReady = 220,
      /// <summary>221:接続を閉じる。</summary>
      ServiceClosingTransmissionChannel = 221,
      /// <summary>235:認証は成功。</summary>
      AuthenticationSuccessful = 235,
      /// <summary>250:要求された処理は実行可能。完了。</summary>
      RequestedMailActionOkay_Completed = 250,
      /// <summary>251:受信者が存在しないので[forward-path]に転送する。</summary>
      UserNotLocal_WillForwardTo = 251,
      /// <summary>252:ユーザーの確認に失敗。しかしメッセージ受信され配送される。</summary>
      CannotVerifyUser_ButWillAcceptMessageAndAttemptDelivery = 252,
      /// <summary>334:認証処理を待機中</summary>
      WaitingForAuthentication = 334,
      /// <summary>354:メールの入力開始。入力終了は「.」のみの行を送信。</summary>
      StartMailInput = 354,
      /// <summary>421:サービスは利用不能。接続を閉じる。</summary>
      ServiceNotAvailable_ClosingTransmissionChannel = 421,
      /// <summary>432:パスワードの変更が必要。</summary>
      APasswordTransitionIsNeeded = 432,
      /// <summary>450:メールボックスが利用できないため、要求された処理は実行不能。</summary>
      RequestedMailActionNotTaken_MailboxUnavailable = 450,
      /// <summary>451:処理中にエラーが発生。要求された処理は失敗。</summary>
      RequestedActionAborted_ErrorInProcessing = 451,
      /// <summary>454:一時的な認証失敗。</summary>
      TemporaryAuthenticationFailure = 454,
      /// <summary>500:文法に間違いがあるため、コマンドが理解できない。</summary>
      SyntaxError_CommandUnrecognized = 500,
      /// <summary>501:引数の文法に間違いがある。</summary>
      SyntaxErrorInParametersOrArguments = 501,
      /// <summary>502:指示されたコマンドはこのシステムには実装されていない。</summary>
      CommandNotImplemented = 502,
      /// <summary>503:コマンドの発行順序が間違っている。</summary>
      BadSequenceOfCommands = 503,
      /// <summary>504:コマンドの引数が未定義。</summary>
      CommandParameterNotImplemented = 504,
      /// <summary>530:認証が必要。</summary>
      AuthenticationRequired = 530,
      /// <summary>538:要求された認証処理には暗号化が必要。</summary>
      EncryptionRequiredForRequestedAuthenticationMechanism = 538,
      /// <summary>550:メールボックスが利用できないため、要求された処理は実行不能。</summary>
      RequestedActionNotTaken_MailboxUnavailable = 550,
      /// <summary>551:受信者が存在しない。[forward-path]に送信せよ。</summary>
      UserNotLocal_PleaseTry = 551,
      /// <summary>552:ディスク不足のため、要求された処理は実行不能。</summary>
      RequestedMailActionAborted_ExceededStorageAllocation = 552,
      /// <summary>553:ディスク不足のため、要求された処理は実行不能。</summary>
      RequestedActionNotTaken_MailboxNameNotAllowed = 553,
      /// <summary>554:処理失敗。</summary>
      TransactionFailed = 554
   }

}
