
namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Enumera los códigos de resultado de un comando Smtp.
   /// </summary>
   public enum SmtpCommandResultCode : int
   {
      /// <summary>Ningún código devuelto.</summary>
      None = 0,
      /// <summary>211:VXeÌóÔBVXewvõ®¹B</summary>
      SystemStatus_OrSystemHelpReply = 211,
      /// <summary>214:wvbZ[WB</summary>
      HelpMessage = 214,
      /// <summary>220:õ®¹B</summary>
      ServiceReady = 220,
      /// <summary>221:Ú±ðÂ¶éB</summary>
      ServiceClosingTransmissionChannel = 221,
      /// <summary>235:FØÍ¬÷B</summary>
      AuthenticationSuccessful = 235,
      /// <summary>250:v³ê½ÍÀsÂ\B®¹B</summary>
      RequestedMailActionOkay_Completed = 250,
      /// <summary>251:óMÒª¶ÝµÈ¢ÌÅ[forward-path]É]·éB</summary>
      UserNotLocal_WillForwardTo = 251,
      /// <summary>252:[U[ÌmFÉ¸sBµ©µbZ[WóM³êz³êéB</summary>
      CannotVerifyUser_ButWillAcceptMessageAndAttemptDelivery = 252,
      /// <summary>334:FØðÒ@</summary>
      WaitingForAuthentication = 334,
      /// <summary>354:[ÌüÍJnBüÍI¹Íu.vÌÝÌsðMB</summary>
      StartMailInput = 354,
      /// <summary>421:T[rXÍps\BÚ±ðÂ¶éB</summary>
      ServiceNotAvailable_ClosingTransmissionChannel = 421,
      /// <summary>432:pX[hÌÏXªKvB</summary>
      APasswordTransitionIsNeeded = 432,
      /// <summary>450:[{bNXªpÅ«È¢½ßAv³ê½ÍÀss\B</summary>
      RequestedMailActionNotTaken_MailboxUnavailable = 450,
      /// <summary>451:ÉG[ª­¶Bv³ê½Í¸sB</summary>
      RequestedActionAborted_ErrorInProcessing = 451,
      /// <summary>454:êIÈFØ¸sB</summary>
      TemporaryAuthenticationFailure = 454,
      /// <summary>500:¶@ÉÔá¢ª é½ßAR}hªðÅ«È¢B</summary>
      SyntaxError_CommandUnrecognized = 500,
      /// <summary>501:øÌ¶@ÉÔá¢ª éB</summary>
      SyntaxErrorInParametersOrArguments = 501,
      /// <summary>502:w¦³ê½R}hÍ±ÌVXeÉÍÀ³êÄ¢È¢B</summary>
      CommandNotImplemented = 502,
      /// <summary>503:R}hÌ­sªÔáÁÄ¢éB</summary>
      BadSequenceOfCommands = 503,
      /// <summary>504:R}hÌøª¢è`B</summary>
      CommandParameterNotImplemented = 504,
      /// <summary>530:FØªKvB</summary>
      AuthenticationRequired = 530,
      /// <summary>538:v³ê½FØÉÍÃ»ªKvB</summary>
      EncryptionRequiredForRequestedAuthenticationMechanism = 538,
      /// <summary>550:[{bNXªpÅ«È¢½ßAv³ê½ÍÀss\B</summary>
      RequestedActionNotTaken_MailboxUnavailable = 550,
      /// <summary>551:óMÒª¶ÝµÈ¢B[forward-path]ÉM¹æB</summary>
      UserNotLocal_PleaseTry = 551,
      /// <summary>552:fBXNs«Ì½ßAv³ê½ÍÀss\B</summary>
      RequestedMailActionAborted_ExceededStorageAllocation = 552,
      /// <summary>553:fBXNs«Ì½ßAv³ê½ÍÀss\B</summary>
      RequestedActionNotTaken_MailboxNameNotAllowed = 553,
      /// <summary>554:¸sB</summary>
      TransactionFailed = 554
   }

}
