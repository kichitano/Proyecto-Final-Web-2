using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.PowerPoint;
using System.IO;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections;
using Microsoft.Office.Core;
//Luis Eduardo MAMAMNI BEDREGAL
namespace SistemaPortafolio.Models
{
    public class Meta
    {

        public void registrarmetada(string archivo, string extension, string cod_curso, int persona_id, int semestre_id, int tipodocumento_id, int unidad_id, DateTime fecha_subida, string taman)
        {
            int pagtotal = 0;
            int palabratotal = 0;
            int caractertotal = 0;
            int lineatotal = 0;
            int parrafototal = 0;
            int diapositiva = 0;
            string tamanio = "";
            string programa = "";
            string fechacreacion = "";

            int documentoid = Convert.ToInt32(HttpContext.Current.Session["id_documento"].ToString());

            try
            {
                if (extension.Equals("xlsx") || extension.Equals("xls"))
                {
                    Microsoft.Office.Interop.Excel.Application excelObject = new Microsoft.Office.Interop.Excel.Application();
                    string file = archivo;
                    object nullobject = System.Reflection.Missing.Value;
                    Microsoft.Office.Interop.Excel.Workbook sheets = excelObject.Workbooks.Open(file, nullobject, nullobject, nullobject,
                    nullobject, nullobject, nullobject, nullobject, nullobject, nullobject, nullobject, nullobject,
                    nullobject, nullobject, nullobject);

                    //consultar las propiedades en ingles para buscar aqui http://www.siddharthrout.com/2012/01/18/vb-net-to-read-and-set-excels-inbuilt-document-properties/

                    object excelProperties = sheets.BuiltinDocumentProperties;
                    Type typeDocBuiltInProps = excelProperties.GetType();

                    tamanio = taman;

                    Object oprograma = typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, excelProperties, new object[] { "Application name" });
                    Type tprograma = oprograma.GetType();
                    programa = tprograma.InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, oprograma, new object[] { }).ToString();

                    Object ofecha = typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, excelProperties, new object[] { "Creation date" });
                    Type tfecha = ofecha.GetType();
                    fechacreacion = tfecha.InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, ofecha, new object[] { }).ToString();

                    //obtener las filas y columnas del excel
                    Microsoft.Office.Interop.Excel.Worksheet sheet = sheets.ActiveSheet;

                    int celda = 0;
                    int columna = 0;

                    if (sheet != null)

                    {

                        columna = sheet.UsedRange.Columns.Count;

                        celda = sheet.UsedRange.Rows.Count;

                    }

                    using (var db = new ModeloDatos())
                    {
                        string fec;
                        fec = fecha_subida.ToString("yyyyMMdd");

                        DateTime dt;
                        dt = Convert.ToDateTime(fechacreacion);

                        string fecc;
                        fecc = dt.ToString("yyyyMMdd");

                        int nada = 0;

                        db.Database.ExecuteSqlCommand(
                            "insert into MetadataDocumento values(@documento_id,@cod_curso,@persona_id,@semestre_id,@tipodocumento_id,@id_unidad,@pagina_total,@palabra_total,@caracter_total,@linea_total,@parrafo_total,@celda,@columna,@diapositiva,@tamanio,@programa_nombre,cast(@fecha_creacion as smalldatetime),cast(@fecha_subida as smalldatetime))",
                            new SqlParameter("documento_id", documentoid),
                            new SqlParameter("cod_curso", cod_curso),
                            new SqlParameter("persona_id", persona_id),
                            new SqlParameter("semestre_id", semestre_id),
                            new SqlParameter("tipodocumento_id", tipodocumento_id),
                            new SqlParameter("id_unidad", unidad_id),
                            new SqlParameter("pagina_total", nada),
                            new SqlParameter("palabra_total", nada),
                            new SqlParameter("caracter_total", nada),
                            new SqlParameter("linea_total", nada),
                            new SqlParameter("parrafo_total", nada),
                            new SqlParameter("celda", celda),
                            new SqlParameter("columna", columna),
                            new SqlParameter("diapositiva", diapositiva),
                            new SqlParameter("tamanio", tamanio),
                            new SqlParameter("programa_nombre", programa),
                            new SqlParameter("fecha_creacion", fecc),
                            new SqlParameter("fecha_subida", fec)

                            );
                    }

                    excelObject.Quit();

                    LiberarAplicacion(excelObject);

                    //sheets.Close(XlSaveAction.xlDoNotSaveChanges, nullobject, nullobject);
                }
                if (extension.Equals("docx") || extension.Equals("doc"))
                {
                    Microsoft.Office.Interop.Word.Application wordObject = new Microsoft.Office.Interop.Word.Application();
                    object file = archivo;
                    object nullobject = Missing.Value;
                    Document docs = wordObject.Documents.Open(file, nullobject, nullobject, nullobject,
                    nullobject, nullobject, nullobject, nullobject, nullobject, nullobject, nullobject, nullobject, nullobject, nullobject, nullobject, nullobject);

                    //consultar las propiedades en ingles para buscar aqui http://www.siddharthrout.com/2012/01/18/vb-net-to-read-and-set-excels-inbuilt-document-properties/

                    object wordProperties = docs.BuiltInDocumentProperties;
                    Type typeDocBuiltInProps = wordProperties.GetType();

                    Object opagtotal = typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, wordProperties, new object[] { "Number of pages" });
                    Type tpagtotal = opagtotal.GetType();
                    pagtotal = Convert.ToInt32(tpagtotal.InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, opagtotal, new object[] { }).ToString());

                    Object opalabratotal = typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, wordProperties, new object[] { "Number of words" });
                    Type tpalabratotal = opalabratotal.GetType();
                    palabratotal = Convert.ToInt32(tpalabratotal.InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, opalabratotal, new object[] { }).ToString());

                    Object ocaractertotal = typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, wordProperties, new object[] { "Number of characters" });
                    Type tcaractertotal = ocaractertotal.GetType();
                    caractertotal = Convert.ToInt32(tcaractertotal.InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, ocaractertotal, new object[] { }).ToString());

                    Object olineatotal = typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, wordProperties, new object[] { "Number of lines" });
                    Type tlineatotal = olineatotal.GetType();
                    lineatotal = Convert.ToInt32(tlineatotal.InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, olineatotal, new object[] { }).ToString());

                    Object oparrafototal = typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, wordProperties, new object[] { "Number of paragraphs" });
                    Type tparrafototal = oparrafototal.GetType();
                    parrafototal = Convert.ToInt32(tparrafototal.InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, oparrafototal, new object[] { }).ToString());

                    Object otamanio = typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, wordProperties, new object[] { "Number of bytes" });
                    Type ttamanio = otamanio.GetType();
                    tamanio = (Convert.ToInt32(ttamanio.InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, otamanio, new object[] { }).ToString()) / 1024).ToString() + " KB";

                    Object oprograma = typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, wordProperties, new object[] { "Application name" });
                    Type tprograma = oprograma.GetType();
                    programa = tprograma.InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, oprograma, new object[] { }).ToString();

                    Object ofecha = typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, wordProperties, new object[] { "Creation date" });
                    Type tfecha = ofecha.GetType();
                    fechacreacion = tfecha.InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, ofecha, new object[] { }).ToString();

                    using (var db = new ModeloDatos())
                    {
                        string fec;
                        fec = fecha_subida.ToString("yyyyMMdd");

                        DateTime dt;
                        dt = Convert.ToDateTime(fechacreacion);

                        string fecc;
                        fecc = dt.ToString("yyyyMMdd");
                        int nada = 0;
                        db.Database.ExecuteSqlCommand(
                            "insert into MetadataDocumento values(@documento_id,@cod_curso,@persona_id,@semestre_id,@tipodocumento_id,@id_unidad,@pagina_total,@palabra_total,@caracter_total,@linea_total,@parrafo_total,@celda,@columna,@diapositiva,@tamanio,@programa_nombre,cast(@fecha_creacion as smalldatetime),cast(@fecha_subida as smalldatetime))",
                             new SqlParameter("documento_id", documentoid),
                            new SqlParameter("cod_curso", cod_curso),
                            new SqlParameter("persona_id", persona_id),
                            new SqlParameter("semestre_id", semestre_id),
                            new SqlParameter("tipodocumento_id", tipodocumento_id),
                            new SqlParameter("id_unidad", unidad_id),
                            new SqlParameter("pagina_total", pagtotal),
                            new SqlParameter("palabra_total", palabratotal),
                            new SqlParameter("caracter_total", caractertotal),
                            new SqlParameter("linea_total", lineatotal),
                            new SqlParameter("parrafo_total", parrafototal),
                            new SqlParameter("celda", nada),
                            new SqlParameter("columna", nada),
                            new SqlParameter("diapositiva", diapositiva),
                            new SqlParameter("tamanio", tamanio),
                            new SqlParameter("programa_nombre", programa),
                            new SqlParameter("fecha_creacion", fecc),
                            new SqlParameter("fecha_subida", fec)

                            );
                    }

                    //docs.Close(WdSaveOptions.wdDoNotSaveChanges, nullobject, nullobject);
                    wordObject.Quit();

                    LiberarAplicacion(wordObject);
                }
                if (extension.Equals("pptx") || extension.Equals("ppt"))
                {
                    Microsoft.Office.Interop.PowerPoint.Application pptObject = new Microsoft.Office.Interop.PowerPoint.Application();
                    string file = archivo;
                    object nullobject = System.Reflection.Missing.Value;
                    Microsoft.Office.Interop.PowerPoint.Presentation sheets = pptObject.Presentations.Open(file, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);
                    //Get Author Name
                    object pptProperties = sheets.BuiltInDocumentProperties;
                    Type typeDocBuiltInProps = pptProperties.GetType();

                    tamanio = taman;

                    Object opalabratotal = typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, pptProperties, new object[] { "Number of words" });
                    Type tpalabratotal = opalabratotal.GetType();
                    palabratotal = Convert.ToInt32(tpalabratotal.InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, opalabratotal, new object[] { }).ToString());

                    Object oparrafototal = typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, pptProperties, new object[] { "Number of paragraphs" });
                    Type tparrafototal = oparrafototal.GetType();
                    parrafototal = Convert.ToInt32(tparrafototal.InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, oparrafototal, new object[] { }).ToString());

                    Object oprograma = typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, pptProperties, new object[] { "Application name" });
                    Type tprograma = oprograma.GetType();
                    programa = tprograma.InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, oprograma, new object[] { }).ToString();

                    Object odiapositiva = typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, pptProperties, new object[] { "Number of slides" });
                    Type tdiapositiva = odiapositiva.GetType();
                    diapositiva = (Convert.ToInt32(tdiapositiva.InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, odiapositiva, new object[] { }).ToString()));

                    Object ofecha = typeDocBuiltInProps.InvokeMember("Item", BindingFlags.Default | BindingFlags.GetProperty, null, pptProperties, new object[] { "Creation date" });
                    Type tfecha = ofecha.GetType();
                    fechacreacion = tfecha.InvokeMember("Value", BindingFlags.Default | BindingFlags.GetProperty, null, ofecha, new object[] { }).ToString();

                    using (var db = new ModeloDatos())
                    {
                        string fec;
                        fec = fecha_subida.ToString("yyyyMMdd");

                        DateTime dt;
                        dt = Convert.ToDateTime(fechacreacion);

                        string fecc;
                        fecc = dt.ToString("yyyyMMdd");
                        int nada = 0;
                        db.Database.ExecuteSqlCommand(
                            "insert into MetadataDocumento values(@documento_id,@cod_curso,@persona_id,@semestre_id,@tipodocumento_id,@id_unidad,@pagina_total,@palabra_total,@caracter_total,@linea_total,@parrafo_total,@celda,@columna,@diapositiva,@tamanio,@programa_nombre,cast(@fecha_creacion as smalldatetime),cast(@fecha_subida as smalldatetime))",
                             new SqlParameter("documento_id", documentoid),
                            new SqlParameter("cod_curso", cod_curso),
                            new SqlParameter("persona_id", persona_id),
                            new SqlParameter("semestre_id", semestre_id),
                            new SqlParameter("tipodocumento_id", tipodocumento_id),
                            new SqlParameter("id_unidad", unidad_id),
                            new SqlParameter("pagina_total", pagtotal),
                            new SqlParameter("palabra_total", palabratotal),
                            new SqlParameter("caracter_total", caractertotal),
                            new SqlParameter("linea_total", lineatotal),
                            new SqlParameter("parrafo_total", parrafototal),
                            new SqlParameter("celda", nada),
                            new SqlParameter("columna", nada),
                            new SqlParameter("diapositiva", diapositiva),
                            new SqlParameter("tamanio", tamanio),
                            new SqlParameter("programa_nombre", programa),
                            new SqlParameter("fecha_creacion", fecc),
                            new SqlParameter("fecha_subida", fec)

                            );
                    }

                    //sheets.Close();
                    pptObject.Quit();

                    LiberarAplicacion(pptObject);
                }
            }

            catch (Exception e)
            {
                throw;
            }
        }

        //para cerrar el documento en segundo plano
        private void LiberarAplicacion(object o)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(o);

            }
            catch
            {
                

            }
            finally
            {
                o = null;

            }

        }
    }
}