using Cosmo.UI.Controls;
using Cosmo.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Cosmo.UI
{
   /// <summary>
   /// Implementa una vista que permite generarse sin estar dentro de una página.
   /// </summary>
   public abstract class PartialView : View 
   {
      // Internal data declarations
      private List<ModalView> _modals;

      #region Constructors

      /// <summary>
      /// Devuelve una instancia de <see cref="PartialView"/>.
      /// </summary>
      protected PartialView()
      {
         Initialize();
      }

      #endregion

      #region Properties

      /// <summary>
      /// Devuelve o establece el contenido de la página.
      /// </summary>
      public ControlCollection Content { get; set; }

      /// <summary>
      /// Gets a modal views used by this view.
      /// </summary>
      public List<ModalView> Modals
      {
         get
         {
            if (_modals == null) _modals = new List<ModalView>();
            return _modals;
         }
         set { _modals = value; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Inicia el ciclo de vida de la vista.
      /// </summary>
      internal override void StartViewLifecycle()
      {
         bool canLoadData = true;
         string receivedFormID = string.Empty;
         var watch = Stopwatch.StartNew();

         try
         {
            // Inicialización de la página
            InitPage();

            // Comprueba si la llamada corresponde a un envio de datos desde un formulario
            if (IsFormReceived)
            {
               FormControl recvForm = GetProcessedForm();
               if (recvForm != null)
               {
                  receivedFormID = recvForm.DomID;
                  FormDataReceived(recvForm);
               }
               else
               {
                  canLoadData = false;
               }
            }

            // Carga la página
            if (canLoadData)
            {
               LoadPage();
            }
         }
         catch (Exception ex)
         {
            ShowError(ex);
         }

         // Renderiza la página
         Response.ContentType = "text/html";
         Response.Write(Workspace.UIService.Render(Content, receivedFormID));

         watch.Stop();
         Response.Write("<!-- Content created in " + watch.ElapsedMilliseconds + "mS -->");
      }

      /*
      /// <summary>
      /// Redirige al cliente hacia otra URL.
      /// </summary>
      /// <param name="destinationUrl">La URL de destino.</param>
      public void Redirect(string destinationUrl)
      {
         _context.Response.Redirect(destinationUrl, true);
      }
      */

      /// <summary>
      /// Muestra un mensaje de error.
      /// </summary>
      /// <param name="title">Título del error.</param>
      /// <param name="description">Descripción del error.</param>
      public void ShowError(string title, string description)
      {
         StringBuilder xhtml = new StringBuilder();

         // Limpia el contenido de la zona principal dónde se va a mostrar el mensaje
         Content.Clear();
         Content.Clear();

         // Genera el mensaje de error
         CalloutControl callout = new CalloutControl(this);
         callout.Type = ComponentColorScheme.Error;
         callout.Title = title;
         callout.Text = description;

         // Agrega el mensaje al contenido
         Content.Add(callout);
      }

      /// <summary>
      /// Muestra un mensaje de error.
      /// </summary>
      /// <param name="exception">Excepción a mostrar.</param>
      public void ShowError(Exception exception)
      {
         StringBuilder xhtml = new StringBuilder();

         // Limpia el contenido de la zona principal dónde se va a mostrar el mensaje
         Content.Clear();
         Content.Clear();

         // Agrega el mensaje al contenido
         Content.Add(new ErrorControl(this, exception));
      }

      #endregion

      #region Abstract Members
      /*
      /// <summary>
      /// Método invocado al iniciar la carga de la página, antes de procesar los datos recibidos.
      /// </summary>
      public abstract void InitPage();

      /// <summary>
      /// Método invocado durante la carga de la página.
      /// </summary>
      public abstract void LoadPage();

      /// <summary>
      /// Método invocado al recibir datos de un formulario.
      /// </summary>
      /// <param name="receivedForm">Una instancia de <see cref="Form"/> que representa el formulario recibido. El formulario está actualizado con los datos recibidos.</param>
      public abstract void FormDataReceived(Form receivedForm);
      */
      #endregion

      #region Private Members

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         this.Content = new ControlCollection();
         this._modals = null;
      }
      
      /// <summary>
      /// Devuelve la instancia de <see cref="FormControl"/> correspondiente al formulario recibido (con los datos actualizados) si la validación 
      /// es correcta o <c>null</c> en cualquier otro caso.
      /// </summary>
      private FormControl GetProcessedForm()
      {
         FormControl recvForm = null;
         string formDomId = Parameters.GetString(FormControl.FORM_ID);

         // Obtiene el formulario
         recvForm = (FormControl)Content.Get(formDomId);

         // Si condigue encontrar el formulario, lo proceso y lo devuelve
         if (recvForm.ProcessForm(Parameters))
         {
            return recvForm;
         }
         else
         {
            return null;
         }
      }

      /*
      /// <summary>
      /// Comprueba las reglas de seguridad para acceder a la página actual.
      /// </summary>
      private void CheckSecurityConstrains()
      {
         System.Reflection.MemberInfo info = this.GetType();
         object[] attributes = info.GetCustomAttributes(true);

         foreach (object attr in attributes)
         {
            if (attr.GetType() == typeof(AuthenticationRequired))
            {
               if (!IsAuthenticated)
               {
                  Url url = new Url(Cosmo.Workspace.COSMO_URL_LOGIN);
                  url.AddParameter(Cosmo.Workspace.PARAM_LOGIN_REDIRECT, Request.RawUrl);

                  Redirect(url.ToString(true));
               }
            }
            else if (attr.GetType() == typeof(AuthorizationRequired))
            {
               if (!IsAuthenticated)
               {
                  Url url = new Url(Cosmo.Workspace.COSMO_URL_LOGIN);
                  url.AddParameter(Cosmo.Workspace.PARAM_LOGIN_REDIRECT, Request.RawUrl);

                  Redirect(url.ToString(true));
               }

               if (!Workspace.CurrentUser.CheckAuthorization(((AuthorizationRequired)attr).RequiredRole))
               {
                  throw new AuthenticationException("ACCESO DENEGADO: Su cuenta de usuario no tiene suficientes permisos para acceder a esta página o recurso.");
               }
            }
         }
      }
      */
      #endregion

   }
}
