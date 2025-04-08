Imports System.Data.SqlClient

Public Class FrmEmpleados
    Dim conBD As Conexion = New Conexion()
    Private Sub FrmEmpleados_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboRol.Items.AddRange(New String() {"Administrador", "Vendedor"})
        MostrarEmpleados()
    End Sub

    Private Sub MostrarEmpleados()

        Dim consulta As String = "SELECT * FROM Empleados"
        Dim comando As New SqlCommand(consulta, conBD.Conectar())
        Dim adaptador As New SqlDataAdapter(comando)
        Dim tabla As New DataTable()
        adaptador.Fill(tabla)
        dgvEmpleados.DataSource = tabla
        conBD.Cerrar()
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim consulta As String = "INSERT INTO Empleados (Nombre, Usuario, Contraseña, Rol) VALUES (@Nombre, @Usuario, @Contraseña, @Rol)"
        Dim comando As New SqlCommand(consulta, conBD.Conectar())
        comando.Parameters.AddWithValue("@Nombre", txtNombre.Text)
        comando.Parameters.AddWithValue("@Usuario", txtUsuario.Text)
        comando.Parameters.AddWithValue("@Contraseña", txtContraseña.Text)
        comando.Parameters.AddWithValue("@Rol", cboRol.Text)
        comando.ExecuteNonQuery()
        MsgBox("Empleado registrado correctamente.", MsgBoxStyle.Information)
        conBD.Cerrar()
        Limpiar()
        MostrarEmpleados()
    End Sub


    Private Sub Limpiar()
        txtIdEmpleado.Clear()
        txtNombre.Clear()
        txtUsuario.Clear()
        txtContraseña.Clear()
        cboRol.SelectedIndex = -1
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        Dim consulta As String = "UPDATE Empleados SET Nombre=@Nombre, Usuario=@Usuario, Contraseña=@Contraseña, Rol=@Rol WHERE IdEmpleado=@Id"
        Dim comando As New SqlCommand(consulta, conBD.Conectar())
        comando.Parameters.AddWithValue("@Nombre", txtNombre.Text)
        comando.Parameters.AddWithValue("@Usuario", txtUsuario.Text)
        comando.Parameters.AddWithValue("@Contraseña", txtContraseña.Text)
        comando.Parameters.AddWithValue("@Rol", cboRol.Text)
        comando.Parameters.AddWithValue("@Id", txtIdEmpleado.Text)
        comando.ExecuteNonQuery()
        MsgBox("Empleado actualizado correctamente.", MsgBoxStyle.Information)
        conBD.Cerrar()
        Limpiar()
        MostrarEmpleados()

    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click

        If txtIdEmpleado.Text <> "" Then
            Dim resultado = MsgBox("¿Está seguro que desea eliminar este producto?", MsgBoxStyle.YesNo)
            If resultado = MsgBoxResult.Yes Then

                Dim consulta As String = "DELETE FROM Empleados WHERE IdEmpleado=@id"

                Dim comando As New SqlCommand(consulta, conBD.Conectar())
                comando.Parameters.AddWithValue("@id", Convert.ToInt32(txtIdEmpleado.Text))
                comando.ExecuteNonQuery()
                conBD.Cerrar()

                MostrarEmpleados()
                Limpiar()
                MsgBox("Producto eliminado correctamente", MsgBoxStyle.Information)

            End If
        End If






    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        Limpiar()
    End Sub
End Class