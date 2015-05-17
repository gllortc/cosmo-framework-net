
namespace Cosmo.Net.Smtp
{

   /// <summary>
   /// Enumera los c�digos de resultado de un comando Smtp.
   /// </summary>
   public enum SmtpCommandResultCode : int
   {
      /// <summary>Ning�n c�digo devuelto.</summary>
      None = 0,
      /// <summary>211:�V�X�e���̏�ԁB�V�X�e���w���v���������B</summary>
      SystemStatus_OrSystemHelpReply = 211,
      /// <summary>214:�w���v���b�Z�[�W�B</summary>
      HelpMessage = 214,
      /// <summary>220:���������B</summary>
      ServiceReady = 220,
      /// <summary>221:�ڑ������B</summary>
      ServiceClosingTransmissionChannel = 221,
      /// <summary>235:�F�؂͐����B</summary>
      AuthenticationSuccessful = 235,
      /// <summary>250:�v�����ꂽ�����͎��s�\�B�����B</summary>
      RequestedMailActionOkay_Completed = 250,
      /// <summary>251:��M�҂����݂��Ȃ��̂�[forward-path]�ɓ]������B</summary>
      UserNotLocal_WillForwardTo = 251,
      /// <summary>252:���[�U�[�̊m�F�Ɏ��s�B���������b�Z�[�W��M����z�������B</summary>
      CannotVerifyUser_ButWillAcceptMessageAndAttemptDelivery = 252,
      /// <summary>334:�F�؏�����ҋ@��</summary>
      WaitingForAuthentication = 334,
      /// <summary>354:���[���̓��͊J�n�B���͏I���́u.�v�݂̂̍s�𑗐M�B</summary>
      StartMailInput = 354,
      /// <summary>421:�T�[�r�X�͗��p�s�\�B�ڑ������B</summary>
      ServiceNotAvailable_ClosingTransmissionChannel = 421,
      /// <summary>432:�p�X���[�h�̕ύX���K�v�B</summary>
      APasswordTransitionIsNeeded = 432,
      /// <summary>450:���[���{�b�N�X�����p�ł��Ȃ����߁A�v�����ꂽ�����͎��s�s�\�B</summary>
      RequestedMailActionNotTaken_MailboxUnavailable = 450,
      /// <summary>451:�������ɃG���[�������B�v�����ꂽ�����͎��s�B</summary>
      RequestedActionAborted_ErrorInProcessing = 451,
      /// <summary>454:�ꎞ�I�ȔF�؎��s�B</summary>
      TemporaryAuthenticationFailure = 454,
      /// <summary>500:���@�ɊԈႢ�����邽�߁A�R�}���h�������ł��Ȃ��B</summary>
      SyntaxError_CommandUnrecognized = 500,
      /// <summary>501:�����̕��@�ɊԈႢ������B</summary>
      SyntaxErrorInParametersOrArguments = 501,
      /// <summary>502:�w�����ꂽ�R�}���h�͂��̃V�X�e���ɂ͎�������Ă��Ȃ��B</summary>
      CommandNotImplemented = 502,
      /// <summary>503:�R�}���h�̔��s�������Ԉ���Ă���B</summary>
      BadSequenceOfCommands = 503,
      /// <summary>504:�R�}���h�̈���������`�B</summary>
      CommandParameterNotImplemented = 504,
      /// <summary>530:�F�؂��K�v�B</summary>
      AuthenticationRequired = 530,
      /// <summary>538:�v�����ꂽ�F�؏����ɂ͈Í������K�v�B</summary>
      EncryptionRequiredForRequestedAuthenticationMechanism = 538,
      /// <summary>550:���[���{�b�N�X�����p�ł��Ȃ����߁A�v�����ꂽ�����͎��s�s�\�B</summary>
      RequestedActionNotTaken_MailboxUnavailable = 550,
      /// <summary>551:��M�҂����݂��Ȃ��B[forward-path]�ɑ��M����B</summary>
      UserNotLocal_PleaseTry = 551,
      /// <summary>552:�f�B�X�N�s���̂��߁A�v�����ꂽ�����͎��s�s�\�B</summary>
      RequestedMailActionAborted_ExceededStorageAllocation = 552,
      /// <summary>553:�f�B�X�N�s���̂��߁A�v�����ꂽ�����͎��s�s�\�B</summary>
      RequestedActionNotTaken_MailboxNameNotAllowed = 553,
      /// <summary>554:�������s�B</summary>
      TransactionFailed = 554
   }

}
