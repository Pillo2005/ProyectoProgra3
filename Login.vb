Imports System.Data.SqlClient
Public Class Login

    Dim conBD As Conexion = New Conexion()
    Private Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        'Esto es una prueba
        Dim consulta As String = "SELECT * FROM Empleados WHERE Usuario=@usuario AND Contraseña=@contraseña"
        Dim conexion As SqlConnection = conBD.Conectar()
        Dim cmd As New SqlCommand(consulta, conexion)
        cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text)
        cmd.Parameters.AddWithValue("@contraseña", txtContraseña.Text)

        Dim lector As SqlDataReader = cmd.ExecuteReader()

        If lector.Read() Then
            Dim rol As String = lector("Rol").ToString()

            ' Guardar datos globales si lo necesitas
            ModuloGlobal.NombreUsuario = lector("Usuario").ToString()
            ModuloGlobal.RolUsuario = rol

            MsgBox("Bienvenido " & rol, MsgBoxStyle.Information, "Acceso correcto")

            Me.Hide()
            FrmMenuPrincipal.Show()
        Else
            MsgBox("Usuario o contraseña incorrectos.", MsgBoxStyle.Critical, "Error de acceso")
        End If

        conBD.Cerrar()


    End Sub
End Class
