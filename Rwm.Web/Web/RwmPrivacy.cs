using Cosmo.Net;
using Cosmo.UI.Controls;
using System.Reflection;

namespace Rwm.Web
{
   public class RwmPrivacy : Cosmo.UI.PageView
   {

      #region PageViewContainer Implementation

      public override void InitPage()
      {
         ActiveMenuId = "home";

         // Cabecera
         HeaderContent.Add(Workspace.UIService.GetNavbarMenu(this, "navbar"));

         // Barra de navegación lateral
         LeftContent.Add(Workspace.UIService.GetSidebarMenu(this, "sidebar"));

         HtmlContentControl html = new HtmlContentControl(this);
         html.AppendHeader(1, "Términos de uso y Política de privacidad");

         html.AppendHeader(2, "Tratamiento de los datos de carácter personal");
         html.AppendParagraph(@"Para cumplimentar la suscripción a " + HtmlContentControl.BoldText(Workspace.Name) + @" 
                              usted debe proporcionar los datos que se piden en el formulario. " + HtmlContentControl.BoldText(Workspace.Name) + @" 
                              guardará estos datos en la base de datos que es propiedad de " + HtmlContentControl.BoldText(Workspace.Name) + @".");

         html.AppendParagraph(@"Estos datos serán utilizados para la personalización del acceso a " + HtmlContentControl.BoldText(Workspace.Name) + @", 
                              el acceso a los servicios reservados a los suscriptores y para el envío de 
                              correos electrónicos con información del portal y publicidad de productos y 
                              servicios relacionados siempre con el ferrocarril.");

         html.AppendParagraph(@"Los datos que usted proporcione serán archivados en los sistemas de " + HtmlContentControl.BoldText(Workspace.Name) + @", 
                              donde se mantendrán accesibles para su verificación, modificación o eliminación 
                              en caso de que usted así lo deseara.");

         html.AppendParagraph(@"Los datos proporcionados por usted no serán, bajo ningún concepto, tratados o 
                              cedidos a otras empresas, entidades u organizaciones distintas de " + HtmlContentControl.BoldText(Workspace.Name) + @".");

         html.AppendHeader(2, "Condiciones generales de uso");
         html.AppendParagraph(@"Todos los derechos de propiedad industrial e intelectual de los servicios on-
                              line de " + HtmlContentControl.BoldText(Workspace.Name) + @" y de sus contenidos 
                              (textos, imágenes, sonidos, audio, vídeo, diseños, creatividades, software) 
                              pertenecen a " + HtmlContentControl.BoldText(Workspace.Name) + @" salvo mención 
                              expresa.");

         html.AppendParagraph(@"El usuario puede visualizar todos los elementos, imprimirlos, copiarlos y 
                              almacenarlos en el disco duro de su ordenador o en cualquier otro soporte 
                              físico siempre y cuando sea, única y exclusivamente, para su uso personal y 
                              privado, quedando, por tanto, terminantemente prohibida su utilización con 
                              fines comerciales, su distribución, así como su modificación, alteración o 
                              descompilación.");

         html.AppendParagraph(@"El usuario se compromete a hacer un uso adecuado de los contenidos y servicios 
                              (como por ejemplo los anuncios clasificados, los foros de opinión o páginas 
                              abiertas al lector) que " + HtmlContentControl.BoldText(Workspace.Name) + @" 
                              ofrece en su portal y a no emplearlos para incurrir en actividades ilícitas o 
                              contrarias a la buena fe y al ordenamiento legal; difundir contenidos o propaganda 
                              de carácter racista, xenófobo, pornográfico ilegal, de apología del terrorismo o 
                              atentatorio contra los derechos humanos; provocar daños en los sistemas físicos y 
                              lógicos de " + HtmlContentControl.BoldText(Workspace.Name) + @", de sus proveedores 
                              o de terceras personas, introducir o difundir en la red virus informáticos o 
                              cualesquiera otros sistemas físicos o lógicos que sean susceptibles de provocar 
                              los daños anteriormente mencionados. Las informaciones, comentarios o contenidos 
                              que usted, como suscriptor, publique se publican bajo su única responsabilidad. 
                              " + HtmlContentControl.BoldText(Workspace.Name) + @" se reserva el derecho de 
                              eliminar cualquier información, comentario o contenido que usted publique sin 
                              previo aviso e incluso de cancelar la suscripción si lo cree necesario.");

         html.AppendParagraph(HtmlContentControl.BoldText(Workspace.Name) + @" se reserva el derecho de efectuar 
                              sin previo aviso las modificaciones que considere oportunas en su Web, pudiendo 
                              cambiar, suprimir o añadir tanto los contenidos y servicios que presta como la 
                              forma en la que éstos aparezcan presentados o localizados.");

         html.AppendParagraph(HtmlContentControl.BoldText(Workspace.Name) + @" tan sólo autoriza menciones a 
                              sus contenidos en otras sedes Web, con el tratamiento que éstas consideren, 
                              siempre y cuando no reproduzcan los contenidos originalmente publicados en 
                              " + HtmlContentControl.BoldText(Workspace.Name) + @". En el caso de disponer de 
                              un enlace hipertexto a alguna de sus páginas, el usuario debe saber que está 
                              entrando en la sedes Web de " + HtmlContentControl.BoldText(Workspace.Name) + @" y 
                              debe percibir en su navegador su dirección URL.");

         html.AppendParagraph(HtmlContentControl.BoldText(Workspace.Name) + @" perseguirá el incumplimiento de 
                              las anteriores condiciones así como cualquier utilización indebida de los 
                              contenidos presentados en su sede Web ejerciendo todas las acciones civiles 
                              y penales que les puedan corresponder en derecho.");

         html.AppendParagraph(@"Por motivos legales, " + HtmlContentControl.BoldText(Workspace.Name) + @" podrá y 
                              deberá facilitar cuanta información le sea requerida a las autoridades competentes 
                              conforme a las leyes españolas en caso de mediar la pertinente orden judicial, 
                              la cual sólo se da cuando un juez tiene firme sospecha de que el usuario ha 
                              realizado actividades ilegales. Bajo este supuesto, y con la intención de colaborar 
                              con la justicia, " + HtmlContentControl.BoldText(Workspace.Name) + @" puede 
                              registrar y posteriormente facilitar a la policía, previa presentación de la 
                              orden judicial legalmente necesaria u otro requerimiento por parte de las 
                              autoridades competentes según las leyes vigentes en cada momento, información 
                              relativa a la dirección IP que identifica a la conexión del usuario, así como 
                              la hora exacta de la misma. El usuario acepta que la información relativa a su 
                              IP, así como fecha y hora exacta, queden asociadas a las actividades que realice 
                              en la red de portales de " + HtmlContentControl.BoldText(Workspace.Name) + @" y 
                              sean almacenadas en sus propios servidores.");

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

      #region Static Members

      /// <summary>
      /// Return the appropiate URL to call this view.
      /// </summary>
      /// <returns>A string containing the requested URL.</returns>
      public static string GetURL()
      {
         Url url = new Url(MethodBase.GetCurrentMethod().DeclaringType.Name);
         return url.ToString();
      }

      #endregion

   }
}
