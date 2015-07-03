using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmo.Cms.Common
{
   public static class CmsPublishStatus
   {

      #region Enumerations

      /// <summary>
      /// Enumera los estados posibles de publicación de un objeto Cosmo.
      /// </summary>
      public enum PublishStatus : int
      {
         /// <summary>
         /// Despublicado. 
         /// El objeto no es visible.
         /// </summary>
         Unpublished = 0,
         /// <summary>
         /// Borrador. 
         /// El objeto no es visible externamente y sólo es accesible por el propietario.
         /// </summary>
         Draft = 1,
         /// <summary>
         /// Aprovación pendiente. 
         /// El objeto no es visible externamente y está a la espera de recibir aprovación por un usuario gerárquicamente por encima del propietario.
         /// </summary>
         ApprovalPending = 2,
         /// <summary>
         /// Publicado.
         /// El objeto es visible externamente y puede ser accedido por todos los usuarios o por los que tienen suficientes privilegios.
         /// </summary>
         Published = 3
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Convert an integer to a value of <see cref="PublishStatus"/> enumeration.
      /// </summary>
      /// <param name="value">Value to convert.</param>
      /// <returns>A value of enumeration <see cref="PublishStatus"/>.</returns>
      public static PublishStatus ToPublishStatus(int value)
      {
         if (value == (int)PublishStatus.Published)
         {
            return PublishStatus.Published;
         }
         else if (value == (int)PublishStatus.Draft)
         {
            return PublishStatus.Draft;
         }
         else if (value == (int)PublishStatus.ApprovalPending)
         {
            return PublishStatus.ApprovalPending;
         }
         else
         {
            return PublishStatus.Unpublished;
         }
      }

      #endregion

   }
}
