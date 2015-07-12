using Cosmo.UI.Controls;
using System;
using System.Collections.Generic;

namespace Cosmo.Utils
{
   /// <summary>
   /// Implementa un contenedor de controles UI.
   /// </summary>
   public class ControlCollection : IEnumerable<Control>
   {
      // Internal data declarations
      private List<Control> _controls;
      private Type _ctrlType;

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="ControlCollection"/>.
      /// </summary>
      public ControlCollection()
      {
         Initialize();
      }

      /// <summary>
      /// Gets a new instance of <see cref="ControlCollection"/>.
      /// </summary>
      /// <param name="type">Tipo de objetos que admite la colección (todas las instancias deben descender directamente de este tipo).</param>
      public ControlCollection(Type type)
      {
         Initialize();

         _ctrlType = type;
      }

      #endregion

      #region Settings

      /// <summary>
      /// Devuelve el número de controles que contiene la colección.
      /// </summary>
      public int Count
      {
         get { return _controls.Count; }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Vacía la colección y elimina todo su contenido.
      /// </summary>
      public void Clear()
      {
         _controls.Clear();
      }

      /// <summary>
      /// Obtiene un determinado control.
      /// </summary>
      /// <param name="index">Índice del control a obtener.</param>
      /// <returns>La instancia de <see cref="Control"/> solicitada o <c>null</c> si el control no existe o no está accesible.</returns>
      public Control Get(int index)
      {
         return _controls[index];
      }

      /// <summary>
      /// Obtiene un determinado control.
      /// </summary>
      /// <param name="domId">Identificador del control que se desea obtener.</param>
      /// <returns>La instancia de <see cref="Control"/> solicitada o <c>null</c> si el control no existe o no está accesible.</returns>
      public Control Get(string domId)
      {
         return Get(domId, true);
      }

      /// <summary>
      /// Obtiene un determinado control.
      /// </summary>
      /// <param name="domId">Identificador del control que se desea obtener.</param>
      /// <param name="throwExceptionOnMissingControl">Indica si en caso de no encontrar el control solicitado debe generar una excepción (<c>true</c>) o debe devolver un valor <c>null</c> (<c>false</c>).</param>
      /// <returns>La instancia de <see cref="Control"/> solicitada o <c>null</c> si el control no existe o no está accesible.</returns>
      public Control Get(string domId, bool throwExceptionOnMissingControl)
      {
         Control ctrl = null;

         foreach (Control control in _controls)
         {
            if (control.DomID.ToLower().Equals(domId.ToLower()))
            {
               return control;
            }
            else if (control is IControlSingleContainer)
            {
               ctrl = ((IControlSingleContainer)control).Content.Get(domId, false);

               if (ctrl != null)
               {
                  return ctrl;
               }
            }
            else if (control is IControlCollectionContainer)
            {
               foreach (IControlSingleContainer container in ((IControlCollectionContainer)control).NestedContainers)
               {
                  ctrl = container.Content.Get(domId, false);

                  if (ctrl != null)
                  {
                     return ctrl;
                  }
               }
            }
         }

         if (throwExceptionOnMissingControl)
         {
            throw new ControlNotFoundException("No se encuentra el control solicitado: " + domId);
         }
         else
         {
            return null;
         }
      }

      /// <summary>
      /// Elimina un control de la colección.
      /// </summary>
      /// <param name="id">Identificador del elemento a eliminar.</param>
      public bool Remove(string domId)
      {
         foreach (Control control in _controls)
         {
            if (control.DomID.ToLower().Equals(domId.ToLower()))
            {
               return _controls.Remove(control);
            }
            else if (control is IControlSingleContainer)
            {
               if (((IControlSingleContainer)control).Content.Remove(domId))
               {
                  return true;
               }
            }
            else if (control is IControlCollectionContainer)
            {
               foreach (IControlSingleContainer container in ((IControlCollectionContainer)control).NestedContainers)
               {
                  if (container.Content.Remove(domId))
                  {
                     return true;
                  }
               }
            }
         }

         return false;
      }

      /// <summary>
      /// Obtiene todos los controles de un determinado tipo.
      /// </summary>
      /// <param name="controlType">Tipo de control a obtener.</param>
      /// <returns>La lista de controles solicitada.</returns>
      public List<Control> GetControlsByType(Type controlType)
      {
         List<Control> controls = new List<Control>();

         foreach (Control control in this)
         {
            if (controlType.IsAssignableFrom(control.GetType()))
            {
               controls.Add(control);
            }

            if (control is IControlSingleContainer)
            {
               controls.AddRange(((IControlSingleContainer)control).Content.GetControlsByType(controlType));
            }

            if (control is IControlCollectionContainer)
            {
               foreach (IControlSingleContainer container in ((IControlCollectionContainer)control).NestedContainers)
               {
                  controls.AddRange(container.Content.GetControlsByType(controlType));
               }
            }
         }

         return controls;
      }

      /// <summary>
      /// Obtiene todos los campos de formulario de la colección.
      /// </summary>
      /// <param name="controlType">Tipo de control a obtener.</param>
      /// <returns>La lista de controles solicitada.</returns>
      public List<FormField> GetFormFields()
      {
         List<FormField> keys = new List<FormField>();

         foreach (Control control in this)
         {
            if (control.GetType().BaseType == typeof(FormField)) 
            {
               keys.Add((FormField) control);
            }

            if (control.GetType().IsAssignableFrom(typeof(IControlSingleContainer)))
            {
               keys.AddRange(((IControlSingleContainer)control).Content.GetFormFields());
            }
         }

         return keys;
      }

      /// <summary>
      /// Agrega un nuevo control a la colección.
      /// </summary>
      /// <param name="control">Instancia de <see cref="Control"/> a agregar.</param>
      public void Add(Control control)
      {
         if (_ctrlType != null)
         {
            if (control.GetType().IsAssignableFrom(_ctrlType))
            {
               throw new Exception("La colección de controles solo acepta controles del tipo " + _ctrlType.GetType().Name);
            }
         }

         if (control != null)
         {
            _controls.Add(control);
         }
      }

      /// <summary>
      /// Agrega los controles contenidops en otra colección de controles.
      /// </summary>
      /// <param name="control">Instancia de <see cref="ControlCollection"/> a agregar.</param>
      public void Add(ControlCollection controls)
      {
         _controls.AddRange(controls);
      }

      /// <summary>
      /// Elimina un determinado control de la colección.
      /// </summary>
      /// <param name="control">Instancia de <see cref="Control"/> a eliminar.</param>
      public void Remove(Control control)
      {
         _controls.Remove(control);
      }

      /// <summary>
      /// Obtiene el <see cref="IEnumerator"/> que permite que la colección sea enumerable.
      /// </summary>
      /// <returns>Una instancia de <see cref="IEnumerator"/>.</returns>
      public IEnumerator<Control> GetEnumerator()
      {
         return _controls.GetEnumerator();
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Initializes the instance data.
      /// </summary>
      private void Initialize()
      {
         _controls = new List<Control>();
         _ctrlType = null;
      }

      /// <summary>
      /// Devuelve el enumerador de la coelcción.
      /// </summary>
      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      /// <summary>
      /// Indica si un determinado tipo implementa el interface <see cref="IControlCollectionContainer"/>.
      /// </summary>
      /// <param name="type">Tipo a evaluar.</param>
      /// <returns><c>true</c> si el tipo especificado implementa el interface o <c>false</c> en cualquir otro caso.</returns>
      private bool ImplementsIControlCollectionContainer(Type type)
      {
         foreach (Type iType in type.GetInterfaces())
         {
            if (iType.Name.StartsWith("IControlCollectionContainer"))
            {
               return true;
            }
         }

         return false;
      }

      #endregion

   }
}
