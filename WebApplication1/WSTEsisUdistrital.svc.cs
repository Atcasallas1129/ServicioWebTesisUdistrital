using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Security.Cryptography;

namespace WebApplication1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WSTEsisUdistrital" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WSTEsisUdistrital.svc or WSTEsisUdistrital.svc.cs at the Solution Explorer and start debugging.
    public class WSTEsisUdistrital : IWSTEsisUdistrital
    {
        
        public Int64 IdUsuarioLogeado;
        public List<vw_consultaCasoDocumentacionWS> GetRegistrosDocumentados(string usuario, string contrasena)
        {
            try
            {
                if (UsuarioValido(usuario, contrasena))
                {
                    DocumentacionDemoLocalEntities contextoL = new DocumentacionDemoLocalEntities();
                    string contrasenaEncriptada = EncriptarContrasena(contrasena);
                    var UsuarioLogeado = (from p in contextoL.usuario.AsNoTracking()
                                          where p.usuarioLogin == usuario && p.contrasena == contrasenaEncriptada
                                          select p.id);
                    foreach(var objeto in UsuarioLogeado)
                    {
                        IdUsuarioLogeado = objeto;
                    }
                    var registroDocumentado = (from p in contextoL.vw_consultaCasoDocumentacionWS.AsNoTracking()
                                               where p.usuarioModificacion == IdUsuarioLogeado
                                               select p);
                    return registroDocumentado.ToList();
                }
                else
                {
                    throw new Exception("Credenciales no validas entre el Entorno Local y Remoto");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool sincronizacionDatosServerLocal(string usuario, string contrasena, vw_consultaCasoDocumentacion objeto)
        {
            try
            {
                if (UsuarioValido(usuario, contrasena))
                {
                    DocumentacionDemoRemotoEntities contextoRemoto = new DocumentacionDemoRemotoEntities();
                    Guid codUniqueObjeto = objeto.uniqueIdentifier;
                    procesoDocumentacion registro = contextoRemoto.procesoDocumentacion.Single(X => X.uniqueIdentifier == codUniqueObjeto);
                    parentesco parentescoObjeto = contextoRemoto.parentesco.FirstOrDefault(X => X.nombreParentesco == objeto.nombreParentesco);
                    tipoDocumento tipoDocumentoObjeto = contextoRemoto.tipoDocumento.FirstOrDefault(X => X.descripcionDocumento == objeto.tipoDocumentoDestinatario);
                    genero generoObjeto = contextoRemoto.genero.FirstOrDefault(X=>X.descripcionGenero == objeto.GeneroDestinatario);
                    ///informacion del caso
                    registro.parentesco = parentescoObjeto.idPerentesco;
                    registro.porcentaje = objeto.porcentaje;
                    registro.usuarioModificacion = objeto.usuarioModificacion;
                    registro.fechaModificacion = objeto.fechaModificacion;
                    registro.regModificado = objeto.regModificado;
                    ///Informacion de la persona
                    registro.persona1.primerNombre = objeto.primerNombreDestinatario;
                    registro.persona1.segundoNombre = objeto.segundoNombreDestinatario;
                    registro.persona1.primerApellido = objeto.primerApellidoDestinatario;
                    registro.persona1.segundoApellido = objeto.segundoApellidoDestinatario;
                    registro.persona1.tipoDocumento = tipoDocumentoObjeto.idTipoDocumento;
                    registro.persona1.noDocumento = objeto.noDocumentoDestinatario;
                    registro.persona1.genero = generoObjeto.idGenero;
                    registro.persona1.fechaNacimiento = objeto.fechaNacimientoDestinatario;
                    return true;
                }
                else
                {
                    throw new Exception("Credenciales no validas entre el Entorno Local y Remoto");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string EncriptarContrasena(string contrasena)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(contrasena);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        private bool UsuarioValido(string usuario, string contrasena)
        {

            try
            {
                DocumentacionDemoRemotoEntities contextoR = new DocumentacionDemoRemotoEntities();
                DocumentacionDemoLocalEntities contextoL = new DocumentacionDemoLocalEntities();
                string contrasenaEncriptada = EncriptarContrasena(contrasena);

                bool valido = false;
                bool validoLocal = false;
                bool validoRemoto = false;

                validoLocal = contextoL.usuario.Any(X => X.usuarioLogin == usuario && X.contrasena == contrasenaEncriptada);
                validoRemoto = contextoR.usuario.Any(X => X.usuarioLogin == usuario);
                if(validoLocal == true && validoRemoto == true)
                {
                    valido = true;
                }
                return valido;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
