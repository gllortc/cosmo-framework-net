using Cosmo.Utils;
using System;

namespace Cosmo.Communications.Impl
{
   /// <summary>
   /// Implementa un módulo de servicio para envios a una cuenta de Twitter.
   /// Basado en: https://tweetinvi.codeplex.com
   /// </summary>
   public class TwitterModuleImpl : ICommunicationsModule
   {
      private const string SETTING_TWITTER_ACCESSTOKEN = "twitter.access.token";
      private const string SETTING_TWITTER_ACCESSTOKENSECRET = "twitter.access.token.secret";
      private const string SETTING_TWITTER_CONSUMERKEY = "twitter.consumer.key";
      private const string SETTING_TWITTER_CONSUMERSECRET = "twitter.consumer.secret";

      // Internal data declarations
      private Workspace _ws;
      private string _accessToken;
      private string _accessTokenSecret;
      private string _consumerKey;
      private string _consumerSecret;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="SmtpMailModuleImpl"/>.
      /// </summary>
      /// <param name="workspace">Una instancia del workspace actual.</param>
      /// <param name="plugin">Una instancia de <see cref="Plugin"/> que contiene  todas las propiedades para instanciar y configurar el módulo.</param>
      public TwitterModuleImpl(Workspace workspace, Plugin plugin) 
         : base(workspace, plugin)
      {
         _ws = workspace;

         Initialize(plugin);
      }

      #endregion

      #region ICommunicationsModule Implementation

      /// <summary>
      /// Devuelve el nombre de la clase (nombre calificado) admitida como contenedor de mensajes.
      /// </summary>
      public override string MessageQualifiedName
      {
         get { throw new NotImplementedException(); }
      }

      /// <summary>
      /// Envia un mensaje usando el servidor de correo del workspace.
      /// </summary>
      /// <param name="message">Mensaje a enviar.</param>
      public override void Send(object message)
      {
         // Establece las credenciales de conexión
         // TwitterCredentials.SetCredentials(_accessToken, _accessTokenSecret, _consumerKey, _consumerSecret);

         // var tweet = Tweet.CreateTweet("Hello guys");
         // tweet.Publish();
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa el servidor de correo electrónico.
      /// </summary>
      private void Initialize(Plugin plugin)
      {
         _accessToken = plugin.GetString(SETTING_TWITTER_ACCESSTOKEN);
         _accessTokenSecret = plugin.GetString(SETTING_TWITTER_ACCESSTOKENSECRET);
         _consumerKey = plugin.GetString(SETTING_TWITTER_CONSUMERKEY);
         _consumerSecret = plugin.GetString(SETTING_TWITTER_CONSUMERSECRET);
      }

      #endregion

   }
}
