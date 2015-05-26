using Cosmo.UI.Controls;
using System.Collections.Generic;

namespace Rwm.WebApp
{
   public class RwmForumRules : Cosmo.UI.PageViewContainer
   {

      #region PageViewContainer Implementation

      public override void InitPage()
      {
         List<string> list = null;

         ActiveMenuId = "home";

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar", this.ActiveMenuId));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar", this.ActiveMenuId));

         HtmlContentControl html = new HtmlContentControl(this);
         html.AppendHeader(1, "Normas de los foros");

         html.AppendHeader(2, "Aspectos generales");
         list = new List<string>();
         list.Add("La participación en los Foros de " + HtmlContentControl.BoldText(Workspace.Name) + " implica la aceptación de estas normas en su totalidad y su desconocimiento no exime de su aplicación.");
         list.Add(HtmlContentControl.BoldText(Workspace.Name) + " no se hace responsable de las opiniones vertidas por los usuarios en este foro ni necesariamente comparte las mismas.");
         list.Add("Los mensajes son publicados bajo la responsabilidad de la persona que los envía, quedando por tanto, dentro del marco legal vigente en España.");
         list.Add("Si algún mensaje o hilo ofende la sensibilidad de alguna persona, organización o empresa puede pedir su revisión a: " + HtmlContentControl.Link(Workspace.Mail, Workspace.Mail, false));
         html.AppendUnorderedList(list);

         html.AppendHeader(2, "Los Moderadores");
         html.AppendParagraph(@"Los moderadores tienen la capacidad para modificar o eliminar, sin filtro 
                                previo, cualquier mensaje que haya sido enviado a instancia propia o de los 
                                usuarios, sin que sea necesaria la previa consulta a su autor, en caso de 
                                incumplimiento por los usuarios de las reglas abajo expuestas. Los moderadores 
                                tienen también la capacidad de cerrar o eliminar cualquier hilo por dichas 
                                razones, así como la de mover los hilos que estén fuera de tema al foro más 
                                adecuado.");

         html.AppendHeader(2, "Los Usuarios");
         html.AppendParagraph(@"Los usuarios de " + HtmlContentControl.BoldText(Workspace.Name) + @" están sujetos a las normas generales 
                                descritas por la " + HtmlContentControl.Link("http://es.wikipedia.org/wiki/Netiquette", "Netiquette", true) + @", 
                                además de al respeto genérico al ordenamiento jurídico español. Por ello deberán 
                                comportarse siempre de buena fe y especialmente sujetos a las siguientes reglas:");

         html.AppendHeader(3, "Prohibiciones");
         list = new List<string>();
         list.Add("No se permite insultar o injuriar a otros miembros del portal, ni a terceros (ya sean personas físicas, agrupaciones, empresas o instituciones).");
         list.Add("No se permiten críticas de carácter personal, ya que el objeto de los foros es debatir y las ideas y no las personas.");
         list.Add("No se permite la utilización de los foros para imputar la comisión de faltas o delitos a ninguna persona, física o jurídica.");
         list.Add("No se permite la utilización de los foros para fomentar comportamientos o actos ilícitos.");
         list.Add("No se permite la utilización de los foros para fines comerciales tales como la realización de propaganda o la venta de bienes o servicios. Para ello, " + HtmlContentControl.BoldText(Workspace.Name) + " dispone del <em>Tablón de anuncios</em>.");
         list.Add("Con el fin de salvaguardar el buen entendimiento en los foros, no está permitido iniciar discusiones deliberadamente, mediante el envío de mensajes provocativos que no expongan ideas y se limiten a provocar la respuesta airada de otros miembros. Igualmente se prohíbe la creación de cuentas fantasma que no correspondan a personalidades reales, para opinar bajo más de un seudónimo, para lo cual se articularán las medidas que lo garanticen.");
         list.Add("No se permite el uso de expresiones vejatorias o discriminatorias, especialmente las referentes al sexo, la raza, la religión, la región/país o la condición política. Igualmente, no está permitido el uso continuo y desproporcionado de expresiones malsonantes.");
         list.Add("En los Foros no se permite la publicación directa y sin elaboración posterior (copia), de materiales publicados en otros medios de comunicación sin la mención correspondiente (y si fuese necesario, el permiso del mismo).");
         list.Add("No se permite la publicación de comentarios de contenido político, a excepción de aquellos que se refieran estrictamente al diseño, construcción y mantenimiento de infraestructuras ferroviarias, a la regulación jurídica y comercial del sector ferroviario, o a la gestión de los servicios de transporte de mercancías y viajeros.");
         list.Add("No se permite la publicación simultánea del mismo mensaje en varios foros o hilos distintos, tengan o no el mismo contenido literal. Serán eliminados por los moderadores todos los mensajes excepto el remitido en primer lugar.");
         html.AppendUnorderedList(list);

         html.AppendHeader(3, "Obligaciones");
         list = new List<string>();
         list.Add("El usuario se comportará siempre de buena fe y con buena educación. El uso del humor para las exposiciones está permitido, aunque estará limitado por ambos principios, con el fin de evitar el mal gusto en los comentarios y el sarcasmo, en el que se utiliza la burla y el escarnio para ofender.");
         list.Add("El usuario se compromete a ser lo más claro posible en sus exposiciones, y tratará de no duplicar discusiones que ya han sido abiertas. No remitirá el mismo mensaje a varios foros o en varios hilos simultáneamente, dentro de " + HtmlContentControl.BoldText(Workspace.Name) + ". En caso de solicitar información, debe comprobar si su duda ha sido ya resuelta con anterioridad en otros mensajes (usando el buscador de los Foros).");
         list.Add("El usuario se compromete a enviar sus mensajes al foro de temática más conveniente, dejando el canal <em>General</em> para aquellas temáticas no clasificables en los otros canales o como canal de comunicación entre el equipo de " + HtmlContentControl.BoldText(Workspace.Name) + " y los usuarios.");
         list.Add("El usuario se compromete a no responder a las provocaciones o insultos de otros miembros, y a denunciar dichas conductas a la mayor brevedad al equipo de moderadores. El derecho de réplica a alusiones personales se usará siempre dentro de términos adecuados de educación, dándose tras el mismo por finalizada cualquier polémica.");
         list.Add("El usuario se compromete a no suscitar o a responder discusiones de contenido político no ferroviario.");
         html.AppendUnorderedList(list);

         html.AppendHeader(3, "Idiomas");
         list = new List<string>();
         list.Add("El idioma vehicular del foro es el castellano (o español).");
         list.Add("Se permite usar un idioma distinto en todo el mensaje para formular una pregunta siempre que sea un caso esporádico y quede claro que se trate de una persona no castellanohablante.");
         list.Add("Se permite añadir expresiones y/o saludos en idioma distinto siempre que el fondo del mensaje pueda ser entendido por una persona castellanohablante sin dificultad.");
         html.AppendUnorderedList(list);

         MainContent.Add(html);
      }

      public override void FormDataReceived(Cosmo.UI.Controls.FormControl receivedForm)
      {
         // Nothing to do
      }

      public override void FormDataLoad(string formDomID)
      {
         // Nothing to do
      }

      public override void LoadPage()
      {
         // Nothing to do
      }

      #endregion

   }
}
