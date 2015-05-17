using System;
using System.Timers;

namespace Cosmo.Communications
{

   /// <summary>
   /// Implementa una clase que gestiona la cola de trabajos de Cosmo
   /// </summary>
   public class MWQueueService
   {

      #region Enumerations

      /// <summary>
      /// Enumera los estados del servicio.
      /// </summary>
      public enum ServiceStatus : int
      {
         /// <summary>Parado</summary>
         Stopped = 0,
         /// <summary>Activo</summary>
         Started = 1
      }

      #endregion

      private Timer _timer = null;
      private int[] _workspaces = null;

      /// <summary>
      /// Devuelve una instancia de MWQueueService.
      /// </summary>
      public MWQueueService()
      {
         // Creal el Timer
         _timer = new Timer();

         // Inicializa el evento del timer
         _timer.Elapsed += new ElapsedEventHandler(OnTimerEvent);
      }

      #region Properties

      /// <summary>
      /// Intervalo (en milisegundos) entre ejecuciones del servicio.
      /// </summary>
      public double Interval
      {
         get { return _timer.Interval; }
         set { _timer.Interval = value; }
      }

      /// <summary>
      /// Array de identificadores de los workspaces a los que dará salida el servicio.
      /// </summary>
      public int[] WorkspacesID
      {
         get { return _workspaces; }
         set { _workspaces = value; }
      }

      /// <summary>
      /// Indica si el servicio está iniciado o parado
      /// </summary>
      public ServiceStatus Status
      {
         get
         {
            if (_timer.Enabled) return ServiceStatus.Started;
            return ServiceStatus.Stopped;
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Inicia el servicio
      /// </summary>
      public void Start()
      {
         this.Start(0);
      }

      /// <summary>
      /// Inicia el servicio
      /// </summary>
      /// <param name="interval">Intervalo (en milisegundos) entre ejecuciones del servicio.</param>
      public void Start(double interval)
      {
         if ((_workspaces == null) || (_workspaces.Length <= 0))
            throw new Exception("No se ha definido ningun workspace al que prestar servicio.");

         if (interval > 0) _timer.Interval = interval;
         _timer.Start();
      }

      /// <summary>
      /// Detiene el servicio
      /// </summary>
      public void Stop()
      {
         _timer.Stop();
      }

      #endregion

      #region Private Members

      // Evento del timer que lanza la ejecución de la cola de salida.
      private static void OnTimerEvent(object source, ElapsedEventArgs e)
      {
         Console.WriteLine("Hello World!");
      }

      #endregion

   }

}
