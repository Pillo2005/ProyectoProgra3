Imports System.Data.SqlClient
Imports System.Data

Public Class Conexion

    Private con As New SqlConnection("Data Source=DESKTOP-FJ5GJM4;Initial Catalog=SistemaVentas;Integrated Security=true")


    Public Function Conectar() As SqlConnection
        Try
            con.Open()
        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try
        Return con
    End Function


    Public Function Cerrar() As SqlConnection
        Try
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try
        Return con
    End Function


End Class
